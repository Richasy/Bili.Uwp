// Copyright (c) Richasy. All rights reserved.

using Newtonsoft.Json;

namespace Richasy.Bili.Models.BiliBili
{
    /// <summary>
    /// 分区类型.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class Partition
    {
        /// <summary>
        /// 分区的标识符.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "tid", Required = Required.Default)]
        public int Tid { get; set; }

        /// <summary>
        /// 分区名称.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "name", Required = Required.Default)]
        public string Name { get; set; }

        /// <summary>
        /// 分区图标.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "logo", Required = Required.Default)]
        public string Logo { get; set; }

        /// <summary>
        /// 分区指向的链接.
        /// </summary>
        /// <remarks>
        /// 该链接指向的是移动端的跳转链接，这里仅用作分析，不支持跳转.
        /// </remarks>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "uri", Required = Required.Default)]
        public string Uri { get; set; }

        /// <summary>
        /// 判断该分区是否需要显示.
        /// </summary>
        /// <remarks>
        /// 部分分区是以H5页面的形式呈现，部分分区是广告，此处仅显示以视频为主的常规分区.
        /// </remarks>
        /// <returns>分区是否需要显示.</returns>
        public bool IsNeedToShow()
        {
            var needToShow = !string.IsNullOrEmpty(Uri) &&
                Uri.StartsWith("bilibili") &&
                (Uri.Contains("pgc/partition_page") || Uri.Contains("region/"));
            return needToShow;
        }

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is Partition partition && Tid == partition.Tid;

        /// <inheritdoc/>
        public override int GetHashCode() => -2122499778 + Tid.GetHashCode();
    }
}
