// Copyright (c) Richasy. All rights reserved.

using Bili.ViewModels.Uwp.Community;
using Windows.Foundation;

namespace Bili.App.Controls.Dynamic
{
    /// <summary>
    /// 动态条目.
    /// </summary>
    public sealed class DynamicItem : ReactiveControl<DynamicItemViewModel>, IRepeaterItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicItem"/> class.
        /// </summary>
        public DynamicItem() => DefaultStyleKey = typeof(DynamicItem);

        /// <inheritdoc/>
        public Size GetHolderSize() => new Size(300, 200);
    }
}
