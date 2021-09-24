// Copyright (c) GodLeaveMe. All rights reserved.

using Newtonsoft.Json;

namespace Richasy.Bili.Models.BiliBili
{
    /// <summary>
    /// 直播套接字消息基类.
    /// </summary>
    public class LiveMessage
    {
    }

    /// <summary>
    /// 直播弹幕消息.
    /// </summary>
    public class LiveDanmakuMessage : LiveMessage
    {
        /// <summary>
        /// 文本.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// 用户名.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 用户等级.
        /// </summary>
        public string Level { get; set; }

        /// <summary>
        /// 等级颜色.
        /// </summary>
        public string LevelColor { get; set; }

        /// <summary>
        /// 用户头衔.
        /// </summary>
        public string UserTitle { get; set; }

        /// <summary>
        /// 会员文本.
        /// </summary>
        public string VipText { get; set; }

        /// <summary>
        /// 勋章名.
        /// </summary>
        public string MedalName { get; set; }

        /// <summary>
        /// 勋章等级.
        /// </summary>
        public string MedalLevel { get; set; }

        /// <summary>
        /// 勋章颜色.
        /// </summary>
        public string MedalColor { get; set; }

        /// <summary>
        /// 是否为管理员.
        /// </summary>
        public bool IsAdmin { get; set; }

        /// <summary>
        /// 是否为老爷.
        /// </summary>
        public bool IsVip { get; set; }

        /// <summary>
        /// 是否为年费老爷.
        /// </summary>
        public bool IsBigVip { get; set; }

        /// <summary>
        /// 是否有徽章.
        /// </summary>
        public bool HasMedal { get; set; }

        /// <summary>
        /// 是否有头衔.
        /// </summary>
        public bool HasTitle { get; set; }

        /// <summary>
        /// 用户名颜色.
        /// </summary>
        public string UserNameColor { get; set; }

        /// <summary>
        /// 内容颜色.
        /// </summary>
        public string ContentColor { get; set; }
    }

    /// <summary>
    /// 直播礼物消息.
    /// </summary>
    public class LiveGiftMessage : LiveMessage
    {
        /// <summary>
        /// 用户名.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "uname", Required = Required.Default)]
        public string UserName { get; set; }

        /// <summary>
        /// 礼物名.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "gift_name", Required = Required.Default)]
        public string GiftName { get; set; }

        /// <summary>
        /// 动作.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "action", Required = Required.Default)]
        public string Action { get; set; }

        /// <summary>
        /// 个数.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "total_num", Required = Required.Default)]
        public string TotalNumber { get; set; }

        /// <summary>
        /// 礼物Id.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "gift_id", Required = Required.Default)]
        public int GiftId { get; set; }

        /// <summary>
        /// 用户Id.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "uid", Required = Required.Default)]
        public string UserId { get; set; }

        /// <summary>
        /// 动图.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "gif", Required = Required.Default)]
        public string Gif { get; set; }
    }

    /// <summary>
    /// 直播欢迎消息.
    /// </summary>
    public class LiveWelcomeMessage : LiveMessage
    {
        /// <summary>
        /// 动图.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "uname", Required = Required.Default)]
        public string UserName { get; set; }

        /// <summary>
        /// 用户Id.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "uid", Required = Required.Default)]
        public string UserId { get; set; }

        /// <summary>
        /// 是否为老爷.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "vip", Required = Required.Default)]
        public int Vip { get; set; }
    }
}
