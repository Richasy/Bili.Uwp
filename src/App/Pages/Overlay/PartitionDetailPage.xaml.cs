// Copyright (c) Richasy. All rights reserved.

using System.ComponentModel;
using Richasy.Bili.App.Controls;
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
            this.Loaded += this.OnLoaded;
            this.Unloaded += this.OnUnloaded;
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
            var animate = animationService.PrepareToAnimate("PartitionBackAnimate", this.PartitionHeader);
            animate.Configuration = new DirectConnectedAnimationConfiguration();
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            ViewModel.PropertyChanged -= OnViewModelPropertyChanged;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            ViewModel.PropertyChanged += OnViewModelPropertyChanged;
            CheckCurrentSubPartitionAsync();
        }

        private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ViewModel.CurrentSelectedSubPartition))
            {
                CheckCurrentSubPartitionAsync();
            }
        }

        private void OnDetailNavigationViewItemInvoked(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewItemInvokedEventArgs args)
        {
            var vm = args.InvokedItem as SubPartitionViewModel;

            ViewModel.CurrentSelectedSubPartition = vm;
        }

        private async void CheckCurrentSubPartitionAsync()
        {
            var vm = ViewModel.CurrentSelectedSubPartition;
            if (vm != null)
            {
                VideoView.NewItemAdded -= OnVideoViewNewItemAddedAsync;
                VideoView.NewItemAdded += OnVideoViewNewItemAddedAsync;

                if (!vm.IsRequested)
                {
                    await vm.RequestDataAsync();
                }

                BannerView.Visibility = vm.BannerCollection != null && vm.BannerCollection.Count > 0 ? Visibility.Visible : Visibility.Collapsed;
                var isShowSort = vm.SortTypeCollection != null && vm.SortTypeCollection.Count > 0;
                if (isShowSort)
                {
                    VideoSortComboBox.Visibility = Visibility.Visible;
                    VideoSortComboBox.SelectedItem = vm.CurrentSortType;
                }
                else
                {
                    VideoSortComboBox.Visibility = Visibility.Collapsed;
                }

                if (!(DetailNavigationView.SelectedItem is SubPartitionViewModel selectedItem) || selectedItem != vm)
                {
                    DetailNavigationView.SelectedItem = vm;
                }
            }
        }

        private async void OnVideoViewRequestLoadMoreAsync(object sender, System.EventArgs e)
        {
            var currentSubPartition = ViewModel.CurrentSelectedSubPartition;
            if (currentSubPartition != null && !currentSubPartition.IsDeltaLoading && !currentSubPartition.IsInitializeLoading)
            {
                await currentSubPartition.RequestDataAsync();
            }
        }

        private async void OnVideoViewNewItemAddedAsync(object sender, Microsoft.UI.Xaml.Controls.ItemsRepeaterElementPreparedEventArgs e)
        {
            var currentSubPartition = ViewModel.CurrentSelectedSubPartition;
            if (currentSubPartition != null &&
                !currentSubPartition.IsDeltaLoading &&
                !currentSubPartition.IsInitializeLoading &&
                e.Index >= currentSubPartition.VideoCollection.Count - 1)
            {
                var videoItem = e.Element as VideoItem;
                var size = videoItem.GetHolderSize();
                var isNeedLoadMore = false;
                if (double.IsInfinity(size.Width))
                {
                    isNeedLoadMore = (e.Index + 1) * size.Height <= ContentScrollView.ViewportHeight;
                }
                else
                {
                    var rowCount = e.Index / (ContentScrollView.ViewportWidth / size.Width);
                    isNeedLoadMore = rowCount * size.Height <= ContentScrollView.ViewportHeight;
                }

                if (isNeedLoadMore)
                {
                    await ViewModel.CurrentSelectedSubPartition.RequestDataAsync();
                }
                else
                {
                    VideoView.NewItemAdded -= OnVideoViewNewItemAddedAsync;
                }
            }
        }
    }
}
