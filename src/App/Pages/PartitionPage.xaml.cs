// Copyright (c) Richasy. All rights reserved.

using Richasy.Bili.App.Resources.Extension;
using Richasy.Bili.ViewModels.Uwp;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace Richasy.Bili.App.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页.
    /// </summary>
    public sealed partial class PartitionPage : Page, IConnectedAnimationPage
    {
        /// <summary>
        /// <see cref="ViewModel"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(PartitionModuleViewModel), typeof(PartitionPage), new PropertyMetadata(PartitionModuleViewModel.Instance));

        /// <summary>
        /// Initializes a new instance of the <see cref="PartitionPage"/> class.
        /// </summary>
        public PartitionPage()
        {
            this.InitializeComponent();
            this.Loaded += this.OnLoaded;
        }

        /// <summary>
        /// 分区视图模型.
        /// </summary>
        public PartitionModuleViewModel ViewModel
        {
            get { return (PartitionModuleViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        /// <inheritdoc/>
        public void TryStartConnectedAnimation()
        {
            if (this.PartitionView != null && this.ViewModel.CurrentPartition != null)
            {
                var element = this.PartitionView.GetOrCreateElement(this.ViewModel.PartitionCollection.IndexOf(this.ViewModel.CurrentPartition));
                if (element != null)
                {
                    var animateService = ConnectedAnimationService.GetForCurrentView();
                    animateService.TryStartAnimation("PartitionBackAnimate", element);
                }
            }
        }

        private async void OnItemsRepeaterLoadedAsync(object sender, RoutedEventArgs e)
        {
            if (ViewModel.PartitionCollection.Count == 0)
            {
                await ViewModel.InitializeAllPartitionAsync();
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            this.FindName("PartitionView");
        }

        private async void OnPartitionItemClickAsync(object sender, PartitionViewModel e)
        {
            AppViewModel.Instance.SetOverlayContentId(Models.Enums.PageIds.PartitionDetail, e);
            await this.ViewModel.SelectPartitionAsync(e);
        }
    }
}
