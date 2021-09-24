// Copyright (c) GodLeaveMe. All rights reserved.

using Newtonsoft.Json;

namespace Richasy.Bili.Models.BiliBili
{
    /// <summary>
    /// 直播源卡片.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class LiveFeedCard
    {
        /// <summary>
        /// 直播卡片类型.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "card_type", Required = Required.Default)]
        public string CardType { get; set; }

        /// <summary>
        /// 卡片数据.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "card_data", Required = Required.Default)]
        public LiveFeedCardData CardData { get; set; }
    }

    /// <summary>
    /// 直播源卡片数据.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class LiveFeedCardData
    {
        /// <summary>
        /// 横幅列表.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "banner_v1", Required = Required.Default)]
        public LiveFeedBannerList Banners { get; set; }

        /// <summary>
        /// 热门分区.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "area_entrance_v1", Required = Required.Default)]
        public LiveFeedHotAreaList HotAreas { get; set; }

        /// <summary>
        /// 我关注的用户列表.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "my_idol_v1", Required = Required.Default)]
        public LiveFeedFollowUserList FollowList { get; set; }

        /// <summary>
        /// 直播卡片.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "small_card_v1", Required = Required.Default)]
        public LiveRoomCard LiveCard { get; set; }
    }
}
