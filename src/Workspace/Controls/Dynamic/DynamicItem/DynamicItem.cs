// Copyright (c) Richasy. All rights reserved.

using Bili.ViewModels.Interfaces.Community;

namespace Bili.Workspace.Controls.Dynamic
{
    /// <summary>
    /// 动态条目.
    /// </summary>
    public sealed class DynamicItem : ReactiveControl<IDynamicItemViewModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicItem"/> class.
        /// </summary>
        public DynamicItem() => DefaultStyleKey = typeof(DynamicItem);
    }
}
