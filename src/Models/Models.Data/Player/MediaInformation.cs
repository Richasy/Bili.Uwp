// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;

namespace Bili.Models.Data.Player
{
    /// <summary>
    /// 媒体信息.
    /// </summary>
    public sealed class MediaInformation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MediaInformation"/> class.
        /// </summary>
        /// <param name="minBufferTime">最低缓冲时间.</param>
        /// <param name="videoSegments">不同清晰度的视频列表.</param>
        /// <param name="audioSegments">不同码率的音频列表.</param>
        /// <param name="formats">格式列表.</param>
        public MediaInformation(
            double minBufferTime,
            IEnumerable<SegmentInformation> videoSegments,
            IEnumerable<SegmentInformation> audioSegments,
            IEnumerable<FormatInformation> formats)
        {
            MinBufferTime = minBufferTime;
            VideoSegments = videoSegments;
            AudioSegments = audioSegments;
            Formats = formats;
        }

        /// <summary>
        /// 最低缓冲时间.
        /// </summary>
        public double MinBufferTime { get; }

        /// <summary>
        /// 不同清晰度的视频列表.
        /// </summary>
        public IEnumerable<SegmentInformation> VideoSegments { get; }

        /// <summary>
        /// 不同码率的音频列表.
        /// </summary>
        public IEnumerable<SegmentInformation> AudioSegments { get; }

        /// <summary>
        /// 格式列表.
        /// </summary>
        public IEnumerable<FormatInformation> Formats { get; }
    }
}
