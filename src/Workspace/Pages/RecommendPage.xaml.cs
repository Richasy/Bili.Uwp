// Copyright (c) Richasy. All rights reserved.

using System.ComponentModel;
using Bili.ViewModels.Interfaces.Home;

namespace Bili.Workspace.Pages
{
    /// <summary>
    /// 推荐视频页面.
    /// </summary>
    public sealed partial class RecommendPage : RecommendPageBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RecommendPage"/> class.
        /// </summary>
        public RecommendPage()
        {
            InitializeComponent();
            ViewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        /// <inheritdoc/>
        protected override void OnPageLoaded()
            => ViewModel.InitializeCommand.Execute(default);

        private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ViewModel.IsReloading))
            {
                ContentScrollViewer.ChangeView(0, 0, 1);
            }
        }

        private void OnErrorPanelButtonClick(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
            => ViewModel.ReloadCommand.Execute(default);

        private void OnVideoViewRequestLoadMore(object sender, System.EventArgs e)
            => ViewModel.IncrementalCommand.Execute(default);
    }

    /// <summary>
    /// <see cref="RecommendPage"/>的基类.
    /// </summary>
    public class RecommendPageBase : PageBase<IRecommendPageViewModel>
    {
    }
}
