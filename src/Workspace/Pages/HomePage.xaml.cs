// Copyright (c) Richasy. All rights reserved.

using Bili.ViewModels.Interfaces.Workspace;

namespace Bili.Workspace.Pages
{
    /// <summary>
    /// 首页.
    /// </summary>
    public sealed partial class HomePage : HomePageBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HomePage"/> class.
        /// </summary>
        public HomePage() => InitializeComponent();

        /// <inheritdoc/>
        protected override void OnPageLoaded()
        {
            if (ViewModel.VideoPartitions.Count == 0)
            {
                ViewModel.InitializeVideoPartitionsCommand.Execute(default);
            }
        }
    }

    /// <summary>
    /// <see cref="HomePage"/>的基类.
    /// </summary>
    public class HomePageBase : PageBase<IHomePageViewModel>
    {
    }
}
