// Copyright (c) Richasy. All rights reserved.

using System;
using System.Linq;
using System.Threading.Tasks;
using Bili.App.Controls;
using Bili.ViewModels.Uwp;
using Windows.UI.Xaml;

namespace Bili.App.Pages
{
    /// <summary>
    /// 首页.
    /// </summary>
    public sealed partial class RecommendPage : AppPage, IRefreshPage
    {
        /// <summary>
        /// <see cref="ViewModel"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel), typeof(RecommendViewModel), typeof(RecommendPage), new PropertyMetadata(RecommendViewModel.Instance));

        /// <summary>
        /// Initializes a new instance of the <see cref="RecommendPage"/> class.
        /// </summary>
        public RecommendPage()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        /// <summary>
        /// 视频推荐视图模型.
        /// </summary>
        public RecommendViewModel ViewModel
        {
            get { return (RecommendViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        /// <inheritdoc/>
        public async Task RefreshAsync()
        {
            ViewModel.InitializeCommand.Execute().Subscribe();
            await Task.CompletedTask;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (!ViewModel.VideoCollection.Any())
            {
                FindName(nameof(VideoView));
                ViewModel.InitializeCommand.Execute().Subscribe();
            }
        }

        private void OnVideoViewRequestLoadMore(object sender, EventArgs e)
            => ViewModel.IncrementalCommand.Execute().Subscribe();

        private void OnRefreshButtonClick(object sender, RoutedEventArgs e)
            => ViewModel.InitializeCommand.Execute().Subscribe();
    }
}
