// Copyright (c) Richasy. All rights reserved.

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
        /// 请求推荐视频列表，视频列表会通过<see cref="HomeVideoIteration"/>事件传递.
        /// </summary>
        /// <param name="offsetIdx">偏移标识符.</param>
        /// <returns><see cref="Task"/>.</returns>
        public async Task RequestRecommendCardsAsync(int offsetIdx)
        {
            try
            {
                var cards = await _homeProvider.RequestRecommendCardsAsync(offsetIdx);
                var result = cards.Where(p => p.CardGoto == Av || p.CardGoto == Bangumi || p.CardGoto == Pgc).ToList();

                HomeVideoIteration?.Invoke(this, new Models.App.Args.HomeVideoIterationEventArgs(result));
            }
            catch
            {
                if (offsetIdx == 0)
                {
                    throw;
                }
            }
        }
    }
}
