// Copyright (c) GodLeaveMe. All rights reserved.

using System;
using System.Linq;
using System.Threading.Tasks;
using static Richasy.Bili.Models.App.Constants.ServiceConstants;

namespace Richasy.Bili.Controller.Uwp
{
    /// <summary>
    /// 首页推荐部分.
    /// </summary>
    public partial class BiliController
    {
        /// <summary>
        /// 请求推荐视频列表，视频列表会通过<see cref="RecommendVideoIteration"/>事件传递.
        /// </summary>
        /// <param name="offsetIndex">偏移标识符.</param>
        /// <returns><see cref="Task"/>.</returns>
        public async Task RequestRecommendCardsAsync(int offsetIndex)
        {
            try
            {
                ThrowWhenNetworkUnavaliable();
                var cards = await _homeProvider.RequestRecommendCardsAsync(offsetIndex);
                var result = cards.Where(p => p.CardGoto == Av || p.CardGoto == Bangumi || p.CardGoto == Pgc).ToList();

                RecommendVideoIteration?.Invoke(this, new Models.App.Args.RecommendVideoIterationEventArgs(result));
            }
            catch (Exception ex)
            {
                _loggerModule.LogError(ex, offsetIndex > 0);
                if (offsetIndex == 0)
                {
                    throw;
                }
            }
        }
    }
}
