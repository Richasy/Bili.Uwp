// Copyright (c) Richasy. All rights reserved.

namespace Bili.Models.Data.Player
{
    /// <summary>
    /// 字幕元数据信息.
    /// </summary>
    public sealed class SubtitleMeta
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SubtitleMeta"/> class.
        /// </summary>
        public SubtitleMeta(string id, string name, string url)
        {
            Id = id;
            LanguageName = name;
            Url = url;
        }

        /// <summary>
        /// 标识符.
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// 显示的语言名称.
        /// </summary>
        public string LanguageName { get; }

        /// <summary>
        /// 字幕地址.
        /// </summary>
        public string Url { get; }

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is SubtitleMeta information && Id == information.Id;

        /// <inheritdoc/>
        public override int GetHashCode() => Id.GetHashCode();
    }
}
