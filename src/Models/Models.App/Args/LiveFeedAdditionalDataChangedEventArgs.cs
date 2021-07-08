// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using Richasy.Bili.Models.BiliBili;

namespace Richasy.Bili.Models.App.Args
{
    /// <summary>
    /// 直播源附加数据更改事件参数.
    /// </summary>
    public class LiveFeedAdditionalDataChangedEventArgs : EventArgs
    {
        internal LiveFeedAdditionalDataChangedEventArgs()
        {
        }

        /// <summary>
        /// 横幅列表.
        /// </summary>
        public List<LiveFeedBanner> BannerList { get; internal set; }

        /// <summary>
        /// 关注用户列表.
        /// </summary>
        public List<LiveFeedFollowUser> FollowUserList { get; internal set; }

        /// <summary>
        /// 热门标签区域列表.
        /// </summary>
        public List<LiveFeedHotArea> HotAreaList { get; internal set; }

        /// <summary>
        /// 根据响应结果创建事件参数，如果没有所需的数据，则返回<c>null</c>.
        /// </summary>
        /// <param name="response">直播源响应结果.</param>
        /// <returns><see cref="LiveFeedAdditionalDataChangedEventArgs"/>.</returns>
        public static LiveFeedAdditionalDataChangedEventArgs Create(LiveFeedResponse response)
        {
            var bannerList = response.CardList.Where(p => p.CardType.Contains("banner")).FirstOrDefault();
            var areaList = response.CardList.Where(p => p.CardType.Contains("area")).FirstOrDefault();
            var followUserList = response.CardList.Where(p => p.CardType.Contains("idol")).FirstOrDefault();

            if (bannerList == null && areaList == null && followUserList == null)
            {
                return null;
            }

            var data = new LiveFeedAdditionalDataChangedEventArgs();
            data.BannerList = bannerList?.CardData.Banners.List;
            data.FollowUserList = followUserList?.CardData.FollowList.List;
            data.HotAreaList = areaList?.CardData.HotAreas.List;

            return data;
        }
    }
}
