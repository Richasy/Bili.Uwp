// Copyright (c) Richasy. All rights reserved.

using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Richasy.Bili.ViewModels.Uwp;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Richasy.Bili.App.Controls
{
    /// <summary>
    /// 应用标题栏.
    /// </summary>
    public sealed partial class AppTitleBar : UserControl
    {
        /// <summary>
        /// <see cref="ViewModel"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel), typeof(AppViewModel), typeof(AppTitleBar), new PropertyMetadata(AppViewModel.Instance));

        /// <summary>
        /// Initializes a new instance of the <see cref="AppTitleBar"/> class.
        /// </summary>
        public AppTitleBar()
        {
            InitializeComponent();
            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
            SizeChanged += OnSizeChanged;
        }

        /// <summary>
        /// 视图模型.
        /// </summary>
        public AppViewModel ViewModel
        {
            get { return (AppViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        /// <summary>
        /// 尝试回退.
        /// </summary>
        /// <returns>是否调用了返回命令.</returns>
        public async Task<bool> TryBackAsync()
        {
            if (BackButton.Visibility != Visibility.Visible)
            {
                return false;
            }

            if (ViewModel.IsOpenPlayer)
            {
                if (await PlayerViewModel.Instance.CheckBackAsync())
                {
                    ViewModel.IsOpenPlayer = false;
                    CheckDevice();
                    return true;
                }
            }
            else if (ViewModel.IsShowOverlay)
            {
                ViewModel.SetMainContentId(ViewModel.CurrentMainContentId);
                return true;
            }

            return false;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Window.Current.SetTitleBar(TitleBarHost);
            CheckBackButtonVisibility();
            CheckDevice();
            ViewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        private void OnUnloaded(object sender, RoutedEventArgs e) => ViewModel.PropertyChanged -= OnViewModelPropertyChanged;

        private void OnSizeChanged(object sender, SizeChangedEventArgs e) => CheckDevice();

        private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ViewModel.IsOpenPlayer)
                || e.PropertyName == nameof(ViewModel.IsShowOverlay)
                || e.PropertyName == nameof(ViewModel.CanShowHomeButton)
                || e.PropertyName == nameof(ViewModel.IsXbox))
            {
                CheckBackButtonVisibility();
                CheckDevice();
            }
        }

        private void CheckDevice()
        {
            var width = Window.Current.Bounds.Width;
            if (ViewModel.IsXbox)
            {
                MenuButton.Visibility = ViewModel.IsOpenPlayer ? Visibility.Collapsed : Visibility.Visible;
                AppNameBlock.Visibility = Visibility.Visible;
                RightPaddingColumn.Width = new GridLength(24);
            }
            else
            {
                RightPaddingColumn.Width = new GridLength(172);
                MenuButton.Visibility = width >= ViewModel.MediumWindowThresholdWidth || ViewModel.IsOpenPlayer ? Visibility.Collapsed : Visibility.Visible;
                AppNameBlock.Visibility = width >= ViewModel.MediumWindowThresholdWidth ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private void OnMenuButtonClick(object sender, RoutedEventArgs e)
            => ViewModel.IsNavigatePaneOpen = !ViewModel.IsNavigatePaneOpen;

        private async void OnBackButtonClickAsync(object sender, RoutedEventArgs e)
            => await TryBackAsync();

        private void CheckBackButtonVisibility()
        {
            BackButton.Visibility = (ViewModel.IsShowOverlay || ViewModel.IsOpenPlayer) ?
                Visibility.Visible : Visibility.Collapsed;
            HomeButton.Visibility = (ViewModel.IsOpenPlayer && ViewModel.CanShowHomeButton) ?
                Visibility.Visible : Visibility.Collapsed;
        }

        private async void OnHomeButtonClickAsync(object sender, RoutedEventArgs e)
        {
            await PlayerViewModel.Instance.BackToHomeAsync();
            ViewModel.IsOpenPlayer = false;
        }
    }
}
