// Copyright (c) Richasy. All rights reserved.

namespace Bili.Models.Data.Player
{
    /// <summary>
    /// 弹幕信息.
    /// </summary>
    public sealed class DanmakuInformation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DanmakuInformation"/> class.
        /// </summary>
        public DanmakuInformation(
            string id,
            string content,
            int mode,
            double startPosition,
            uint color,
            int fontSize)
        {
            Id = id;
            Content = content;
            Mode = mode;
            StartPosition = startPosition;
            Color = color;
            FontSize = fontSize;
        }

        /// <summary>
        /// 标识符.
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// 内容.
        /// </summary>
        public string Content { get; }

        /// <summary>
        /// 弹幕模式.
        /// </summary>
        public int Mode { get; }

        /// <summary>
        /// 起始位置（秒）.
        /// </summary>
        public double StartPosition { get; }

        /// <summary>
        /// 弹幕颜色.
        /// </summary>
        public uint Color { get; }

        /// <summary>
        /// 文本字号.
        /// </summary>
        public int FontSize { get; }

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is DanmakuInformation information && Id == information.Id;

        /// <inheritdoc/>
        public override int GetHashCode() => Id.GetHashCode();
    }
}
