// Copyright (c) Richasy. All rights reserved.

namespace Bili.Models.Data.Player
{
    /// <summary>
    /// 字幕信息.
    /// </summary>
    public sealed class SubtitleInformation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SubtitleInformation"/> class.
        /// </summary>
        public SubtitleInformation(double start, double end, string content)
        {
            StartPosition = start;
            EndPosition = end;
            Content = content;
        }

        /// <summary>
        /// 起始位置（秒）.
        /// </summary>
        public double StartPosition { get; }

        /// <summary>
        /// 结束位置（秒）.
        /// </summary>
        public double EndPosition { get; }

        /// <summary>
        /// 字幕内容.
        /// </summary>
        public string Content { get; }
    }
}
