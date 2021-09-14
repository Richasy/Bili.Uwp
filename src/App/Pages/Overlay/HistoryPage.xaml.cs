// Copyright (c) Richasy. All rights reserved.

using System;
using Richasy.Bili.App.Controls.Dialogs;
using Richasy.Bili.Locator.Uwp;
using Richasy.Bili.Models.Enums;
using Richasy.Bili.Toolkit.Interfaces;
using Richasy.Bili.ViewModels.Uwp;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Richasy.Bili.App.Pages.Overlay
{
    /// <summary>
    /// 历史记录页面.
    /// </summary>
    public sealed partial class HistoryPage : Page
    {
        /// <summary>
        /// <see cref="ViewModel"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel), typeof(HistoryViewModel), typeof(HistoryPage), new PropertyMetadata(HistoryViewModel.Instance));

        /// <summary>
        /// Initializes a new instance of the <see cref="HistoryPage"/> class.
        /// </summary>
        public HistoryPage()
        {
            this.InitializeComponent();
            this.Loaded += OnLoadedAsync;
        }

        /// <summary>
        /// 视图模型.
        /// </summary>
        public HistoryViewModel ViewModel
        {
            get { return (HistoryViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        private async void OnLoadedAsync(object sender, RoutedEventArgs e)
        {
            if (!ViewModel.IsRequested)
            {
                await ViewModel.InitializeRequestAsync();
            }
        }

        private async void OnRefreshButtonClickAsync(object sender, RoutedEventArgs e)
        {
            await ViewModel.InitializeRequestAsync();
        }

        private async void OnVideoViewRequestLoadMoreAsync(object sender, System.EventArgs e)
        {
            await ViewModel.RequestDataAsync();
        }

        private async void OnDeleteItemClickAsync(object sender, RoutedEventArgs e)
        {
            var context = (sender as FrameworkElement).DataContext as VideoViewModel;
            await ViewModel.DeleteItemAsync(context);
        }

        private async void OnClearButtonClickAsync(object sender, RoutedEventArgs e)
        {
            var isClear = false;
            if (ViewModel.VideoCollection.Count > 0)
            {
                // Show dialog.
                var msg = ServiceLocator.Instance.GetService<IResourceToolkit>().GetLocaleString(LanguageNames.ClearHistoryWarning);
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
    }
}
