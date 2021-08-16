// Copyright (c) Richasy. All rights reserved.

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
}
