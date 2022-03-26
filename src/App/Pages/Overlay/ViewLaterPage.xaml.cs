// Copyright (c) Richasy. All rights reserved.

using System;
using System.Linq;
using Richasy.Bili.App.Controls.Dialogs;
using Richasy.Bili.Locator.Uwp;
using Richasy.Bili.Models.App.Constants;
using Richasy.Bili.Models.Enums;
using Richasy.Bili.Toolkit.Interfaces;
using Richasy.Bili.ViewModels.Uwp;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Richasy.Bili.App.Pages.Overlay
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页.
    /// </summary>
    public sealed partial class ViewLaterPage : AppPage
    {
        /// <summary>
        /// <see cref="ViewModel"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel), typeof(ViewLaterViewModel), typeof(ViewLaterPage), new PropertyMetadata(ViewLaterViewModel.Instance));

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewLaterPage"/> class.
        /// </summary>
        public ViewLaterPage()
        {
            InitializeComponent();
            Loaded += OnLoadedAsync;
        }

        /// <summary>
        /// 视图模型.
        /// </summary>
        public ViewLaterViewModel ViewModel
        {
            get { return (ViewLaterViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        private async void OnRefreshButtonClickAsync(object sender, RoutedEventArgs e)
        {
            await ViewModel.InitializeRequestAsync();
        }

        private async void OnClearButtonClickAsync(object sender, RoutedEventArgs e)
        {
            var isClear = false;
            if (ViewModel.VideoCollection.Count > 0)
            {
                // Show dialog.
                var msg = ServiceLocator.Instance.GetService<IResourceToolkit>().GetLocaleString(LanguageNames.ClearViewLaterWarning);
                var dialog = new ConfirmDialog(msg);
                var result = await dialog.ShowAsync().AsTask();
                if (result == ContentDialogResult.Primary)
                {
                    isClear = true;
                }
            }

            if (isClear)
            {
                await ViewModel.ClearAsync();
            }
        }

        private async void OnLoadedAsync(object sender, RoutedEventArgs e)
        {
            if (!ViewModel.IsRequested)
            {
                await ViewModel.InitializeRequestAsync();
            }
        }

        private async void OnVideoViewRequestLoadMoreAsync(object sender, System.EventArgs e)
        {
            await ViewModel.RequestDataAsync();
        }

        private async void OnRemoveItemClickAsync(object sender, RoutedEventArgs e)
        {
            var vm = (sender as FrameworkElement).DataContext as VideoViewModel;
            await ViewModel.RemoveAsync(vm);
        }

        private void OnPlayAllButtonClick(object sender, RoutedEventArgs e)
        {
            var videoList = ViewModel.VideoCollection.Where(p => p.VideoType == VideoType.Video).ToList();
            if (videoList.Count > 1)
            {
                PlayerViewModel.Instance.InitializeSection = AppConstants.ViewLaterSection;
                CoreViewModel.OpenPlayer(videoList);
            }
            else
            {
                var firstVideo = videoList.FirstOrDefault();
                if (firstVideo != null)
                {
                    CoreViewModel.OpenPlayer(firstVideo);
                }
            }
        }
    }
}
