// Copyright (c) Richasy. All rights reserved.

using System;
using System.ComponentModel;
using Richasy.Bili.App.Resources.Extension;
using Richasy.Bili.ViewModels.Uwp;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace Richasy.Bili.App.Controls
{
    /// <summary>
    /// 账户管理中枢.
    /// </summary>
    public sealed partial class AccountAvatar : UserControl
    {
        /// <summary>
        /// <see cref="ViewModel"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel), typeof(AccountViewModel), typeof(AccountAvatar), new PropertyMetadata(AccountViewModel.Instance));

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountAvatar"/> class.
        /// </summary>
        public AccountAvatar()
        {
            this.InitializeComponent();
            this.Loaded += OnLoaded;
            this.Unloaded += OnUnloaded;
        }

        /// <summary>
        /// 账户视图模型.
        /// </summary>
        public AccountViewModel ViewModel
        {
            get { return (AccountViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            CheckStatus();
            ViewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            ViewModel.PropertyChanged -= OnViewModelPropertyChanged;
        }

        private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ViewModel.Status))
            {
                CheckStatus();
            }
        }

        private void CheckStatus()
        {
            switch (ViewModel.Status)
            {
                case AccountViewModelStatus.Logout:
                case AccountViewModelStatus.Login:
                    VisualStateManager.GoToState(this, nameof(NormalState), false);
                    break;
                case AccountViewModelStatus.Logging:
                    VisualStateManager.GoToState(this, nameof(LoadingState), false);
                    break;
                default:
                    break;
            }
        }

        private void OnNavigateButtonClick(object sender, RoutedEventArgs e)
        {
            var btn = sender as FrameworkElement;
            var pageId = NavigationExtension.GetPageId(btn);
            if (pageId == Models.Enums.PageIds.Favorite)
            {
                FavoriteViewModel.Instance.SetUser(ViewModel.Mid.Value, ViewModel.DisplayName);
            }

            AppViewModel.Instance.SetOverlayContentId(pageId);
            HideFlyout();
        }

        private async void OnSignOutButtonClickAsync(object sender, RoutedEventArgs e)
        {
            HideFlyout();
            await AccountViewModel.Instance.SignOutAsync();
        }

        private async void OnNavigateToMyHomePageButtonClickAsync(object sender, RoutedEventArgs e)
        {
            HideFlyout();
            await Launcher.LaunchUriAsync(new Uri($"https://space.bilibili.com/{AccountViewModel.Instance.Mid}/")).AsTask();
        }

        private void OnRequestCloseFlyout(object sender, EventArgs e)
        {
            HideFlyout();
        }

        private void HideFlyout()
        {
            FlyoutBase.GetAttachedFlyout(UserAvatar).Hide();
        }

        private async void OnFlyoutOpenedAsync(object sender, object e)
        {
            await ViewModel.InitCommunityInformationAsync();
        }

        private async void OnUserAvatarClickAsync(object sender, EventArgs e)
        {
            if (ViewModel.Status == AccountViewModelStatus.Logout)
            {
                await ViewModel.TrySignInAsync();
            }
            else
            {
                FlyoutBase.ShowAttachedFlyout(UserAvatar);
            }
        }
    }
}
