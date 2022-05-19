// Copyright (c) Richasy. All rights reserved.

using System;
using System.Linq;
using Bili.App.Controls.Dialogs;
using Bili.Locator.Uwp;
using Bili.Models.App.Constants;
using Bili.Models.Enums;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces;
using Bili.ViewModels.Uwp;
using Splat;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Bili.App.Pages.Desktop.Overlay
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

        private readonly INavigationViewModel _navigationViewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewLaterPage"/> class.
        /// </summary>
        public ViewLaterPage()
        {
            InitializeComponent();
            _navigationViewModel = Splat.Locator.Current.GetService<INavigationViewModel>();
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
                _navigationViewModel.NavigateToPlayView(videoList);
            }
            else
            {
                var firstVideo = videoList.FirstOrDefault();
                if (firstVideo != null)
                {
                    _navigationViewModel.NavigateToPlayView(firstVideo);
                }
            }
        }
    }
}
