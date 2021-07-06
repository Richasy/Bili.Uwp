// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using Newtonsoft.Json;

namespace Richasy.Bili.Models.BiliBili
{
    /// <summary>
    /// 直播间卡片.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class LiveRoomCard : LiveRoomBase
    {
        /// <summary>
        /// 直播间Id.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "id", Required = Required.Default)]
        public int RoomId { get; set; }

        /// <summary>
        /// 所属分区Id.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "area_id", Required = Required.Default)]
        public int AreaId { get; set; }

        /// <summary>
        /// 显示分区名.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "area_name", Required = Required.Default)]
        public string AreaName { get; set; }

        /// <summary>
        /// 父分区Id.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "parent_area_id", Required = Required.Default)]
        public int ParentAreaId { get; set; }

        /// <summary>
        /// 父分区名称.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "parent_area_name", Required = Required.Default)]
        public string ParentAreaName { get; set; }

        /// <summary>
        /// 封面左侧文本，指用户名.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "cover_left_style", Required = Required.Default)]
        public CoverContent CoverLeftContent { get; set; }

        /// <summary>
        /// 封面右侧文本，指观看人数.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "cover_right_style", Required = Required.Default)]
        public CoverContent CoverRightContent { get; set; }

        /// <summary>
        /// 反馈列表.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "feedback", Required = Required.Default)]
        public List<LiveFeedback> Feedback { get; set; }

        /// <summary>
        /// 序号.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "index", Required = Required.Default)]
        public int Index { get; set; }

        /// <summary>
        /// 是否隐藏反馈.0-不隐藏，1-隐藏.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "is_hide_feedback", Required = Required.Default)]
        public int IsHideFeedback { get; set; }

        /// <summary>
        /// 封面内容样式.
        /// </summary>
        public class CoverContent
        {
            /// <summary>
            /// 封面文本.
            /// </summary>
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "text", Required = Required.Default)]
            public string Text { get; set; }
        }

        /// <summary>
        /// 反馈.
        /// </summary>
        public class LiveFeedback
        {
            /// <summary>
            /// 标题.
            /// </summary>
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "title", Required = Required.Default)]
            public string Title { get; set; }

            /// <summary>
            /// 副标题.
            /// </summary>
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "subtitle", Required = Required.Default)]
            public string Subtitle { get; set; }

            /// <summary>
            /// 类型.
            /// </summary>
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "type", Required = Required.Default)]
            public string Type { get; set; }

            /// <summary>
            /// 理由集合.
            /// </summary>
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "reasons", Required = Required.Default)]
            public List<LiveFeedbackReason> Reasons { get; set; }

            /// <summary>
            /// 直播反馈理由.
            /// </summary>
            public class LiveFeedbackReason
            {
                /// <summary>
                /// 标识符.
                /// </summary>
                [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "id", Required = Required.Default)]
                public int Id { get; set; }

                /// <summary>
                /// 名称.
                /// </summary>
                [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "name", Required = Required.Default)]
                public string Name { get; set; }

                /// <summary>
                /// Id类型.
                /// </summary>
                [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "id_type", Required = Required.Default)]
                public string IdType { get; set; }
            }
        }
    }
}
