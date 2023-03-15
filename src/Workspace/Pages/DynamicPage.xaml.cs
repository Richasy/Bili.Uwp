// Copyright (c) Richasy. All rights reserved.

using System.ComponentModel;
using Bili.ViewModels.Interfaces;
using Bili.ViewModels.Interfaces.Community;

namespace Bili.Workspace.Pages
{
    /// <summary>
    /// 动态页面.
    /// </summary>
    public sealed partial class DynamicPage : DynamicPageBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicPage"/> class.
        /// </summary>
        public DynamicPage() => InitializeComponent();

        /// <inheritdoc/>
        protected override void OnPageLoaded()
            => ViewModel.InitializeCommand.Execute(default);
    }

    /// <summary>
    /// <see cref="DynamicPage"/>的基类.
    /// </summary>
    public class DynamicPageBase : PageBase<IDynamicVideoModuleViewModel>
    {
    }
}
