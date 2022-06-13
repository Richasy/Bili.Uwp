// Copyright (c) Richasy. All rights reserved.

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
        public Task<PgcDisplayInformation> GetPgcDisplayInformationAsync(int episodeId = 0, int seasonId = 0, string proxy = "", string area = "")
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
                return Task.FromResult(new PgcDisplayInformation());
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
        public Task<EpisodeInteraction> GetPgcEpisodeInteractionAsync(int episodeId)
            => Task.FromResult(new EpisodeInteraction());

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
                var result = await _pgcProvider.FollowAsync(seasonId.ToString(), isFollow);
                return result;
            }
            catch (Exception ex)
            {
                _loggerModule.LogError(ex, true);
                return false;
            }
        }

        /// <summary>
        /// 从 BiliPlus 获取番剧信息.
        /// </summary>
        /// <param name="videoId">视频 Aid.</param>
        /// <returns><see cref="BiliPlusBangumi"/>.</returns>
        public Task<BiliPlusBangumi> GetBiliPlusBangumiAsync(string videoId)
        {
            try
            {
                return Task.FromResult(new BiliPlusBangumi());
            }
            catch (Exception ex)
            {
                _loggerModule.LogError(ex, true);
                return null;
            }
        }
    }
}
