// Copyright (c) Richasy. All rights reserved.

namespace Models.Workspace
{
    /// <summary>
    /// 快速专题导航.
    /// </summary>
    public class QuickTopic
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QuickTopic"/> class.
        /// </summary>
        public QuickTopic(string title, string biliUrl, string webUrl, string icon)
        {
            Title = title;
            BiliUrl = biliUrl;
            WebUrl = webUrl;
            Icon = icon;
        }

        /// <summary>
        /// 专题标题.
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// 哔哩链接.
        /// </summary>
        public string BiliUrl { get; }

        /// <summary>
        /// 网页链接.
        /// </summary>
        public string WebUrl { get; }

        /// <summary>
        /// 图标.
        /// </summary>
        public string Icon { get; }

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is QuickTopic topic && Title == topic.Title;

        /// <inheritdoc/>
        public override int GetHashCode() => Title.GetHashCode();
    }
}
