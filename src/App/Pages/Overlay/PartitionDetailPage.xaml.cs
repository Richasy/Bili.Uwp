// Copyright (c) Richasy. All rights reserved.

using System.ComponentModel;
using Richasy.Bili.Models.Enums;
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
                var animationService = ConnectedAnimationService.GetForCurrentView();
                var animate = animationService.GetAnimation("PartitionAnimate");
                if (animate != null)
                {
                    animate.TryStart(PartitionHeader, new UIElement[] { DetailNavigationView });
                }
            }
        }

        /// <inheritdoc/>
        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            // 这意味着是退回到主视图，而非切换到其它同级页面.
            if (e.SourcePageType.Name == nameof(Page))
            {
                var animationService = ConnectedAnimationService.GetForCurrentView();
                var animate = animationService.PrepareToAnimate("PartitionBackAnimate", this.RootContainer);
                animate.Configuration = new DirectConnectedAnimationConfiguration();
            }
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            ViewModel.PropertyChanged -= OnViewModelPropertyChanged;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            ViewModel.PropertyChanged += OnViewModelPropertyChanged;
            this.FindName(nameof(ContentGrid));
            CheckCurrentSubPartition();
        }

        private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ViewModel.CurrentSelectedSubPartition))
            {
                CheckCurrentSubPartition();
            }
        }

        private async void OnDetailNavigationViewItemInvokedAsync(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewItemInvokedEventArgs args)
        {
            var vm = args.InvokedItem as SubPartitionViewModel;
            await ViewModel.SelectSubPartitionAsync(vm);
            CheckError();
        }

        private void CheckCurrentSubPartition()
        {
            var vm = ViewModel.CurrentSelectedSubPartition;
            if (vm != null)
            {
                var isShowSort = vm.SortTypeCollection != null && vm.SortTypeCollection.Count > 0;
                if (isShowSort)
                {
                    VideoSortComboBox.Visibility = Visibility.Visible;
                    RefreshButton.Visibility = Visibility.Collapsed;
                    VideoSortComboBox.SelectedItem = vm.CurrentSortType;
                }
                else
                {
                    RefreshButton.Visibility = Visibility.Visible;
                    VideoSortComboBox.Visibility = Visibility.Collapsed;
                }

                if (!(DetailNavigationView.SelectedItem is SubPartitionViewModel selectedItem) || selectedItem != vm)
                {
                    DetailNavigationView.SelectedItem = vm;
                }
            }
        }

        private void CheckError()
        {
            if (ViewModel.CurrentSelectedSubPartition.IsError)
            {
                VideoSortComboBox.Visibility = Visibility.Collapsed;
                RefreshButton.Visibility = Visibility.Collapsed;
            }
        }

        private async void OnVideoViewRequestLoadMoreAsync(object sender, System.EventArgs e)
        {
            var currentSubPartition = ViewModel.CurrentSelectedSubPartition;
            if (currentSubPartition != null &&
                !currentSubPartition.IsDeltaLoading &&
                !currentSubPartition.IsInitializeLoading)
            {
                await currentSubPartition.RequestDataAsync();
            }
        }

        private void OnVideoSortComboBoxSlectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (VideoSortComboBox.SelectedItem != null)
            {
                var item = (VideoSortType)VideoSortComboBox.SelectedItem;
                ViewModel.CurrentSelectedSubPartition.CurrentSortType = item;
            }
        }

        private async void OnRefreshButtonClickAsync(object sender, RoutedEventArgs e)
        {
            if (ViewModel.CurrentSelectedSubPartition != null &&
                !ViewModel.CurrentSelectedSubPartition.IsInitializeLoading &&
                !ViewModel.CurrentSelectedSubPartition.IsDeltaLoading)
            {
                await ViewModel.CurrentSelectedSubPartition.InitializeRequestAsync();
                CheckCurrentSubPartition();
                CheckError();
            }
        }
    }
}
