// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using System.Linq;
using Richasy.Bili.Models.BiliBili;

namespace Richasy.Bili.Models.App.Args
{
    /// <summary>
    /// 首页视频批量传输事件，用于首页视频的增量加载.
    /// </summary>
    public class HomeVideoIterationEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HomeVideoIterationEventArgs"/> class.
        /// </summary>
        /// <param name="cards">卡片列表.</param>
        public HomeVideoIterationEventArgs(List<RecommendCard> cards)
        {
            OffsetIndex = cards.Last().Index;
            Cards = cards;
        }

        /// <summary>
        /// 偏移标识符.
        /// </summary>
        public int OffsetIndex { get; set; }

        /// <summary>
        /// 卡片列表.
        /// </summary>
        public List<RecommendCard> Cards { get; set; }
    }
}
