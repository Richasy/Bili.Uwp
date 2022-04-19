// Copyright (c) Richasy. All rights reserved.

using Newtonsoft.Json;

namespace Richasy.Bili.Models.App
{
    /// <summary>
    /// Bili Plus 返回的动漫信息.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class BiliPlusBangumi
    {
        /// <summary>
        /// 剧集 Id.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "season_id", Required = Required.Default)]
        public string SeasonId { get; set; }

        /// <summary>
        /// 是否需要调转，1-需要，0-不需要.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "is_jump", Required = Required.Default)]
        public int IsJump { get; set; }

        /// <summary>
        /// 剧集名.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "title", Required = Required.Default)]
        public string Title { get; set; }

        /// <summary>
        /// 封面.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "cover", Required = Required.Default)]
        public string Cover { get; set; }

        /// <summary>
        /// 是否已完结.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "is_finish", Required = Required.Default)]
        public string IsFinish { get; set; }

        /// <summary>
        /// 播放地址.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "ogv_play_url", Required = Required.Default)]
        public string PlayUrl { get; set; }
    }
}
