// Copyright (c) GodLeaveMe. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using Bilibili.App.Card.V1;

namespace Richasy.Bili.Models.App.Args
{
    /// <summary>
    /// 热门视频批量传输事件.
    /// </summary>
    public class PopularVideoIterationEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PopularVideoIterationEventArgs"/> class.
        /// </summary>
        /// <param name="cards">卡片列表.</param>
        public PopularVideoIterationEventArgs(List<Card> cards)
        {
            OffsetIndex = Convert.ToInt32(cards.Last().SmallCoverV5.Base.Idx);
            Cards = cards;
        }

        /// <summary>
        /// 偏移标识符.
        /// </summary>
        public int OffsetIndex { get; set; }

        /// <summary>
        /// 卡片列表.
        /// </summary>
        public List<Card> Cards { get; set; }
    }
}
