// Copyright (c) GodLeaveMe. All rights reserved.

using Newtonsoft.Json;

namespace Richasy.Bili.Models.BiliBili
{
    /// <summary>
    /// 用户关系.
    /// </summary>
    public class UserRelation
    {
        /// <summary>
        /// 用户关系状态，1:未关注 2:已关注 3:被关注 4:互相关注.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "status", Required = Required.Default)]
        public int Status { get; set; }
    }

    /// <summary>
    /// 用户关系响应结果.
    /// </summary>
    public class UserRelationResponse
    {
        /// <summary>
        /// 用户Id.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "mid", Required = Required.Default)]
        public int UserId { get; set; }

        /// <summary>
        /// 关注时间，未关注则为0.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "mtime", Required = Required.Default)]
        public long FollowTime { get; set; }

        /// <summary>
        /// 关注类型,0-未关注，2-已关注，6-已互关，128-拉黑.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "attribute", Required = Required.Default)]
        public int Type { get; set; }

        /// <summary>
        /// 是否为特别关注，0-否，1-是.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "special", Required = Required.Default)]
        public int IsSpecialFollow { get; set; }

        /// <summary>
        /// 是否关注.
        /// </summary>
        /// <returns>关注结果.</returns>
        public bool IsFollow()
        {
            return Type == 2 || Type == 6;
        }
    }
}
