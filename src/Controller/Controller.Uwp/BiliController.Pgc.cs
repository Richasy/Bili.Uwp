// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Richasy.Bili.Models.App.Args;
using Richasy.Bili.Models.App.Other;
using Richasy.Bili.Models.BiliBili;
using Richasy.Bili.Models.Enums;
using static Richasy.Bili.Models.App.Constants.ControllerConstants;

namespace Richasy.Bili.Controller.Uwp
{
    /// <summary>
    /// 控制器的PGC部分.
    /// </summary>
    public partial class BiliController
    {
        /// <summary>
        /// 请求PGC的顶部标签（仅限于番剧和国创）.
        /// </summary>
        /// <param name="type">PGC类型.</param>
        /// <returns><see cref="Task"/>.</returns>
        public async Task<List<PgcTab>> RequestPgcTabsAsync(PgcType type)
        {
            if (type != PgcType.Bangumi && type != PgcType.Domestic)
            {
                throw new NotSupportedException("当前内容不支持顶部标签");
            }

            var cacheData = await _fileToolkit.ReadLocalDataAsync<LocalCache<List<PgcTab>>>($"{type}Tab.json", folderName: Names.ServerFolder);
            var needRequest = true;
            List<PgcTab> data;
            if (cacheData != null)
            {
                needRequest = cacheData.ExpiryTime < DateTimeOffset.Now;
            }

            if (needRequest)
            {
                ThrowWhenNetworkUnavaliable();
                try
                {
                    var webResult = await _pgcProvider.GetTabAsync(type);
                    data = webResult;
                    var localCache = new LocalCache<List<PgcTab>>(DateTimeOffset.Now.AddDays(1), data);
                    await _fileToolkit.WriteLocalDataAsync($"{type}Tab.json", localCache, Names.ServerFolder);
                }
                catch (Exception ex)
                {
                    _loggerModule.LogError(ex);
                    throw;
                }
            }
            else
            {
                data = cacheData.Data;
            }

            // 仅传出可供解析的标签页.
            data = data.Where(p => p.Link.Contains("sub_page_id")).ToList();
            return data;
        }

        /// <summary>
        /// 获取PGC页面详情.
        /// </summary>
        /// <param name="tabId">标签页Id.</param>
        /// <returns><see cref="Task"/>.</returns>
        public async Task RequestPgcPageDetailAsync(int tabId)
        {
            ThrowWhenNetworkUnavaliable();
            try
            {
                var response = await _pgcProvider.GetPageDetailAsync(tabId);

                var additionalArgs = PgcModuleAdditionalDataChangedEventArgs.Create(response, tabId);
                if (additionalArgs != null)
                {
                    PgcModuleAdditionalDataChanged?.Invoke(this, additionalArgs);
                }

                var iterationArgs = PgcModuleIterationEventArgs.Create(response, tabId);
                PgcModuleIteration?.Invoke(this, iterationArgs);
            }
            catch (Exception ex)
            {
                _loggerModule.LogError(ex);
                throw;
            }
        }

        /// <summary>
        /// 获取PGC页面详情.
        /// </summary>
        /// <param name="type">PGC类型.</param>
        /// <param name="cursor">增量加载的滚动标识，不存在于番剧和国创中.</param>
        /// <returns><see cref="Task"/>.</returns>
        public async Task RequestPgcPageDetailAsync(PgcType type, string cursor = null)
        {
            ThrowWhenNetworkUnavaliable();
            try
            {
                var response = await _pgcProvider.GetPageDetailAsync(type, cursor);

                if (string.IsNullOrEmpty(cursor))
                {
                    var additionalArgs = PgcModuleAdditionalDataChangedEventArgs.Create(response, type);
                    if (additionalArgs != null)
                    {
                        PgcModuleAdditionalDataChanged?.Invoke(this, additionalArgs);
                    }
                }

                var iterationArgs = PgcModuleIterationEventArgs.Create(response, type);
                PgcModuleIteration?.Invoke(this, iterationArgs);
            }
            catch (Exception ex)
            {
                _loggerModule.LogError(ex, !string.IsNullOrEmpty(cursor));
                if (string.IsNullOrEmpty(cursor))
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// 请求PGC分区数据.
        /// </summary>
        /// <param name="partitionId">分区Id.</param>
        /// <param name="offsetId">偏移Id.</param>
        /// <returns><see cref="Task"/>.</returns>
        public async Task RequestSubPartitionDataAsync(int partitionId, int offsetId = 0)
        {
            ThrowWhenNetworkUnavaliable();
            try
            {
                var requestDateTime = DateTimeOffset.Now;
                var data = await _pgcProvider.GetPartitionRecommendVideoAsync(partitionId, offsetId);
                SubPartitionVideoIteration?.Invoke(this, new PartitionVideoIterationEventArgs(partitionId, requestDateTime, data, 1));
            }
            catch (Exception ex)
            {
                _loggerModule.LogError(ex, offsetId > 0);
                if (offsetId == 0)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// 获取PGC内容详情.
        /// </summary>
        /// <param name="episodeId">单集Id.</param>
        /// <param name="seasonId">剧集/系列Id.</param>
        /// <param name="proxy">代理地址.</param>
        /// <param name="area">地区.</param>
        /// <returns>详细内容.</returns>
        public async Task<PgcDisplayInformation> GetPgcDisplayInformationAsync(int episodeId = 0, int seasonId = 0, string proxy = "", string area = "")
        {
            if (episodeId < 0 || seasonId < 0)
            {
                throw new ArgumentOutOfRangeException("Id不会小于0");
            }

            if (episodeId == 0 && seasonId == 0)
            {
                throw new ArgumentException("无效的参数");
            }

            try
            {
                return await _pgcProvider.GetDisplayInformationAsync(episodeId, seasonId, proxy, area);
            }
            catch (Exception ex)
            {
                _loggerModule.LogError(ex);
                throw;
            }
        }

        /// <summary>
        /// 获取PGC分集的交互信息.
        /// </summary>
        /// <param name="episodeId">分集Id.</param>
        /// <returns>分集交互信息.</returns>
        public async Task<EpisodeInteraction> GetPgcEpisodeInteractionAsync(int episodeId)
            => await _pgcProvider.GetEpisodeInteractionAsync(episodeId);

        /// <summary>
        /// 追番/取消追番.
        /// </summary>
        /// <param name="seasonId">番剧/影视Id.</param>
        /// <param name="isFollow">是否关注.</param>
        /// <returns>关注结果.</returns>
        public async Task<bool> FollowPgcSeasonAsync(int seasonId, bool isFollow)
        {
            ThrowWhenNetworkUnavaliable();
            try
            {
                var result = await _pgcProvider.FollowAsync(seasonId, isFollow);
                return result;
            }
            catch (Exception ex)
            {
                _loggerModule.LogError(ex, true);
                return false;
            }
        }

        /// <summary>
        /// 获取PGC索引条件.
        /// </summary>
        /// <param name="type">PGC类型.</param>
        /// <returns>索引结果.</returns>
        public async Task<PgcIndexConditionResponse> GetPgcIndexConditionsAsync(PgcType type)
            => await _pgcProvider.GetPgcIndexConditionsAsync(type);

        /// <summary>
        /// 请求PGC索引内容.
        /// </summary>
        /// <param name="type">类型.</param>
        /// <param name="pageNumber">页码.</param>
        /// <param name="conditions">条件集合.</param>
        /// <returns><see cref="Task"/>.</returns>
        public async Task RequestPgcIndexResultAsync(PgcType type, int pageNumber, Dictionary<string, string> conditions)
        {
            ThrowWhenNetworkUnavaliable();

            try
            {
                var data = await _pgcProvider.GetPgcIndexResultAsync(type, pageNumber, conditions);
                var args = new PgcIndexResultIterationEventArgs(data, type);
                PgcIndexResultIteration?.Invoke(this, args);
            }
            catch (Exception ex)
            {
                _loggerModule.LogError(ex, pageNumber <= 1);
                if (pageNumber > 1)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// 获取PGC时间线.
        /// </summary>
        /// <param name="type">PGC内容类型.</param>
        /// <returns><see cref="PgcTimeLineResponse"/>.</returns>
        public Task<PgcTimeLineResponse> GetPgcTimeLineAsync(PgcType type)
        {
            ThrowWhenNetworkUnavaliable();

            try
            {
                return _pgcProvider.GetPgcTimeLineAsync(type);
            }
            catch (Exception ex)
            {
                _loggerModule.LogError(ex);
                throw;
            }
        }

        /// <summary>
        /// 获取PGC播放列表详情.
        /// </summary>
        /// <param name="listId">列表Id.</param>
        /// <returns>播放列表内容.</returns>
        public Task<PgcPlayListResponse> GetPgcPlayListAsync(int listId)
        {
            ThrowWhenNetworkUnavaliable();

            try
            {
                return _pgcProvider.GetPgcPlayListAsync(listId);
            }
            catch (Exception ex)
            {
                _loggerModule.LogError(ex);
                throw;
            }
        }
    }
}
