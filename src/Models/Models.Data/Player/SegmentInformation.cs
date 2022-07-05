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
        /// <param name="codecs">媒体编码.</param>
        public SegmentInformation(
            string id,
            string baseUrl,
            IEnumerable<string> backupUrls,
            string codecs)
        {
            Id = id;
            BaseUrl = baseUrl;
            BackupUrls = backupUrls;
            Codecs = codecs;
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
        /// 媒体编码.
        /// </summary>
        public string Codecs { get; }

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is SegmentInformation information && Id == information.Id;

        /// <inheritdoc/>
        public override int GetHashCode() => Id.GetHashCode();
    }
}
