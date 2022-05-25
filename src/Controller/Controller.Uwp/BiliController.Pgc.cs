﻿// Copyright (c) Richasy. All rights reserved.

using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Bili.Models.App;
using Bili.Models.BiliBili;
using Bili.Models.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Bili.Controller.Uwp
{
    /// <summary>
    /// 控制器的PGC部分.
    /// </summary>
    public partial class BiliController
    {
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
        /// 从 BiliPlus 获取番剧信息.
        /// </summary>
        /// <param name="videoId">视频 Aid.</param>
        /// <returns><see cref="BiliPlusBangumi"/>.</returns>
        public async Task<BiliPlusBangumi> GetBiliPlusBangumiAsync(string videoId)
        {
            try
            {
                var handler = new HttpClientHandler()
                {
                    AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
                };
                using (var client = new HttpClient(handler))
                {
                    var url = $"https://www.biliplus.com/api/view?id={videoId}";
                    var response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    var bytes = await response.Content.ReadAsByteArrayAsync();
                    var str = Encoding.UTF8.GetString(bytes);
                    var jObj = JObject.Parse(str);
                    var bangumi = jObj["bangumi"].ToString();
                    return JsonConvert.DeserializeObject<BiliPlusBangumi>(bangumi);
                }
            }
            catch (Exception ex)
            {
                _loggerModule.LogError(ex, true);
                return null;
            }
        }
    }
}
