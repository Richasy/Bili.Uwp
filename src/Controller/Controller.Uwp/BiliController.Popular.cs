// Copyright (c) Richasy. All rights reserved.

using System.Linq;
using System.Threading.Tasks;

namespace Richasy.Bili.Controller.Uwp
{
    /// <summary>
    /// 热门.
    /// </summary>
    public partial class BiliController
    {
        /// <summary>
        /// 获取热门信息.
        /// </summary>
        /// <param name="offsetIndex">偏移值.</param>
        /// <returns><see cref="Task"/>.</returns>
        public async Task RequestPopularCardsAsync(int offsetIndex)
        {
            try
            {
                ThrowWhenNetworkUnavaliable();
                var cards = await _popularProvider.GetPopularDetailAsync(offsetIndex);
                cards = cards.Where(p =>
                    p.ItemCase == Bilibili.App.Card.V1.Card.ItemOneofCase.SmallCoverV5
                    && p.SmallCoverV5 != null
                    && p.SmallCoverV5.Base.CardGoto == "av")
                    .ToList();
                PopularVideoIteration?.Invoke(this, new Models.App.Args.PopularVideoIterationEventArgs(cards));
            }
            catch
            {
                if (offsetIndex == 0)
                {
                    throw;
                }
            }
        }
    }
}
