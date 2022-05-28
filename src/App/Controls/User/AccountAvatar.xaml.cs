// Copyright (c) Richasy. All rights reserved.

using System;
using System.ComponentModel;
using Bili.App.Resources.Extension;
using Bili.Models.Enums;
using Bili.ViewModels.Uwp;
using Bili.ViewModels.Uwp.Account;
using Bili.ViewModels.Uwp.Core;
using ReactiveUI;
using Splat;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;

namespace Bili.App.Controls
{
    /// <summary>
    /// 账户管理中枢.
    /// </summary>
    public sealed partial class AccountAvatar : AccountAvatarBase
    {
        private readonly NavigationViewModel _navigationViewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountAvatar"/> class.
        /// </summary>
        public AccountAvatar()
        {
            InitializeComponent();
            ViewModel = Splat.Locator.Current.GetService<AccountViewModel>();
            _navigationViewModel = Splat.Locator.Current.GetService<NavigationViewModel>();
            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
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
            if (e.PropertyName == nameof(ViewModel.State))
            {
                CheckStatus();
            }
        }

        private void CheckStatus()
        {
            switch (ViewModel.State)
            {
                case AuthorizeState.SignedOut:
                case AuthorizeState.SignedIn:
                    VisualStateManager.GoToState(this, nameof(NormalState), false);
                    break;
                case AuthorizeState.Loading:
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
            if (pageId == PageIds.Favorite)
            {
                FavoriteViewModel.Instance.SetUser(ViewModel.Mid.Value, ViewModel.DisplayName);
            }

            _navigationViewModel.NavigateToSecondaryView(pageId);
            HideFlyout();
        }

        private async void OnSignOutButtonClickAsync(object sender, RoutedEventArgs e)
        {
            HideFlyout();
            await ViewModel.SignOutAsync();
        }

        private async void OnNavigateToMyHomePageButtonClickAsync(object sender, RoutedEventArgs e)
        {
            HideFlyout();
            await Launcher.LaunchUriAsync(new Uri($"https://space.bilibili.com/{ViewModel.Mid}/")).AsTask();
        }

        private void OnRequestCloseFlyout(object sender, EventArgs e)
            => HideFlyout();

        private void HideFlyout()
            => FlyoutBase.GetAttachedFlyout(UserAvatar).Hide();

        private async void OnFlyoutOpenedAsync(object sender, object e)
            => await ViewModel.InitCommunityInformationAsync();

        private async void OnUserAvatarClickAsync(object sender, EventArgs e)
        {
            if (ViewModel.State == AuthorizeState.SignedOut)
            {
                await ViewModel.TrySignInAsync();
            }
            else
            {
                FlyoutBase.ShowAttachedFlyout(UserAvatar);
            }
        }
    }

    /// <summary>
    /// <see cref="AccountAvatar"/> 的基类.
    /// </summary>
    public class AccountAvatarBase : ReactiveUserControl<AccountViewModel>
    {
    }
}
