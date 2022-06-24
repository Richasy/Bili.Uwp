// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;

namespace Bili.Models.Data.Appearance
{
    /// <summary>
    /// 带表情的文本内容.
    /// </summary>
    public sealed class EmoteText
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EmoteText"/> class.
        /// </summary>
        /// <param name="text">完整文本.</param>
        /// <param name="emotes">表情索引.</param>
        public EmoteText(string text, Dictionary<string, Image> emotes)
        {
            Text = text;
            Emotes = emotes;
        }

        /// <summary>
        /// 完整文本.
        /// </summary>
        public string Text { get; }

        /// <summary>
        /// 表情索引.
        /// </summary>
        public Dictionary<string, Image> Emotes { get; }

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is EmoteText text && Text == text.Text;

        /// <inheritdoc/>
        public override int GetHashCode() => Text.GetHashCode();
    }
}
