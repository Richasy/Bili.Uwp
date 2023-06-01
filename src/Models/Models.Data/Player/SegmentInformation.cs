// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;

namespace Bili.Models.Data.Player
{
    /// <summary>
    /// 视频或音频分段信息.
    /// </summary>
    public sealed class SegmentInformation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SegmentInformation"/> class.
        /// </summary>
        /// <param name="id">分段 Id.</param>
        /// <param name="baseUrl">基链接.</param>
        /// <param name="backupUrls">备份链接.</param>
        /// <param name="bandwidth">媒体要求的带宽.</param>
        /// <param name="mimeType">媒体格式.</param>
        /// <param name="codecs">媒体编码.</param>
        /// <param name="width">媒体宽度.</param>
        /// <param name="height">媒体高度.</param>
        /// <param name="initialization">起始位置.</param>
        /// <param name="indexRange">索引范围.</param>
        public SegmentInformation(
            string id,
            string baseUrl,
            IEnumerable<string> backupUrls,
            int bandwidth,
            string mimeType,
            string codecs,
            int width,
            int height,
            string initialization,
            string indexRange,
            int startWithSap = 1)
        {
            Id = id;
            BaseUrl = baseUrl;
            BackupUrls = backupUrls;
            Bandwidth = bandwidth;
            MimeType = mimeType;
            Codecs = codecs;
            Width = width;
            Height = height;
            Initialization = initialization;
            IndexRange = indexRange;
            StartWithSap = startWithSap;
        }

        /// <summary>
        /// Dash Id.
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// 基链接.
        /// </summary>
        public string BaseUrl { get; set; }

        /// <summary>
        /// 备份链接.
        /// </summary>
        public IEnumerable<string> BackupUrls { get; }

        /// <summary>
        /// 媒体要求的带宽.
        /// </summary>
        public int Bandwidth { get; }

        /// <summary>
        /// 媒体格式.
        /// </summary>
        public string MimeType { get; }

        /// <summary>
        /// 媒体编码.
        /// </summary>
        public string Codecs { get; }

        /// <summary>
        /// 媒体宽度.
        /// </summary>
        public int Width { get; }

        /// <summary>
        /// 媒体高度.
        /// </summary>
        public int Height { get; }

        /// <summary>
        /// 起始位置.
        /// </summary>
        public string Initialization { get; }

        /// <summary>
        /// 索引范围.
        /// </summary>
        public string IndexRange { get; }

        /// <summary>
        /// None.
        /// </summary>
        public int StartWithSap { get; }

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is SegmentInformation information && Id == information.Id;

        /// <inheritdoc/>
        public override int GetHashCode() => Id.GetHashCode();
    }
}
