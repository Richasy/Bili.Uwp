// Copyright (c) GodLeaveMe. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using Richasy.Bili.Models.BiliBili;

namespace Richasy.Bili.Models.App.Args
{
    /// <summary>
    /// 直播源直播间卡片迭代事件参数.
    /// </summary>
    public class LiveFeedRoomIterationEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LiveFeedRoomIterationEventArgs"/> class.
        /// </summary>
        /// <param name="response">直播源响应结果.</param>
        /// <param name="nextPage">下一页页码.</param>
        public LiveFeedRoomIterationEventArgs(LiveFeedResponse response, int nextPage)
        {
            var rooms = response.CardList.Where(p => p.CardType.Contains("small_card")).ToList();
            List = new List<LiveRoomCard>();
            foreach (var item in rooms)
            {
                List.Add(item.CardData.LiveCard);
            }

            NextPageNumber = nextPage;
        }

        /// <summary>
        /// 直播间列表.
        /// </summary>
        public List<LiveRoomCard> List { get; internal set; }

        /// <summary>
        /// 下一页码.
        /// </summary>
        public int NextPageNumber { get; internal set; }
    }
}
