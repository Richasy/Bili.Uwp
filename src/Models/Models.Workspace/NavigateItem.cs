// Copyright (c) Richasy. All rights reserved.

using Bili.Models.Enums.Workspace;

namespace Models.Workspace
{
    /// <summary>
    /// 导航条目.
    /// </summary>
    public sealed class NavigateItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NavigateItem"/> class.
        /// </summary>
        /// <param name="target">导航目标.</param>
        /// <param name="title">标题.</param>
        /// <param name="icon">图标.</param>
        public NavigateItem(NavigateTarget target, string title, FluentSymbol icon)
        {
            Target = target;
            Title = title;
            Icon = icon;
        }

        /// <summary>
        /// 导航目标.
        /// </summary>
        public NavigateTarget Target { get; }

        /// <summary>
        /// 标题.
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// 图标.
        /// </summary>
        public FluentSymbol Icon { get; }

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is NavigateItem item && Target == item.Target;

        /// <inheritdoc/>
        public override int GetHashCode() => Target.GetHashCode();
    }
}
