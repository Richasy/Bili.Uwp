// Copyright (c) Richasy. All rights reserved.

using Bili.ViewModels.Uwp.Community;

namespace Bili.App.Pages.Desktop
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页.
    /// </summary>
    public sealed partial class PartitionPage : PartitionPageBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PartitionPage"/> class.
        /// </summary>
        public PartitionPage()
            : base() => InitializeComponent();
    }

    /// <summary>
    /// <see cref="PartitionPage"/> 的基类.
    /// </summary>
    public class PartitionPageBase : AppPage<PartitionPageViewModel>
    {
    }
}
