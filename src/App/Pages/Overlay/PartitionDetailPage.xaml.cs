// Copyright (c) Richasy. All rights reserved.

using Richasy.Bili.App.Resources.Extension;
using Richasy.Bili.ViewModels.Uwp;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace Richasy.Bili.App.Pages.Overlay
{
    /// <summary>
    /// 分区详情页面.
    /// </summary>
    public sealed partial class PartitionDetailPage : Page
    {
        /// <summary>
        /// Dependency property of <see cref="ViewModel"/>.
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel), typeof(PartitionViewModel), typeof(PartitionDetailPage), new PropertyMetadata(null));

        /// <summary>
        /// Initializes a new instance of the <see cref="PartitionDetailPage"/> class.
        /// </summary>
        public PartitionDetailPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 分区视图模型.
        /// </summary>
        public PartitionViewModel ViewModel
        {
            get { return (PartitionViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        /// <inheritdoc/>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter != null && e.Parameter is PartitionViewModel data)
            {
                this.ViewModel = data;
                this.DataContext = data;
                var animationService = ConnectedAnimationService.GetForCurrentView();
                animationService.TryStartAnimation("PartitionLogoAnimate", PartitionLogo);
                animationService.TryStartAnimation("PartitionNameAnimate", PartitionName);
            }
        }

        /// <inheritdoc/>
        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            var animationService = ConnectedAnimationService.GetForCurrentView();
            animationService.PrepareToAnimate("PartitionBackAnimate", this.PartitionHeader);
        }
    }
}
