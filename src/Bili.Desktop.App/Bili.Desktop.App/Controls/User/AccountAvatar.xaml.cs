// Copyright (c) Richasy. All rights reserved.

using System;
using System.ComponentModel;
using Bili.Desktop.App.Resources.Extension;
using Bili.DI.Container;
using Bili.Models.Enums;
using Bili.ViewModels.Interfaces.Account;
using Bili.ViewModels.Interfaces.Core;
using Windows.System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls.Primitives;

namespace Bili.Desktop.App.Controls
{
    /// <summary>
    /// 账户管理中枢.
    /// </summary>
    public sealed partial class AccountAvatar : AccountAvatarBase
    {
        private readonly INavigationViewModel _navigationViewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountAvatar"/> class.
        /// </summary>
        public AccountAvatar()
        {
            InitializeComponent();
            ViewModel = Locator.Instance.GetService<IAccountViewModel>();
            _navigationViewModel = Locator.Instance.GetService<INavigationViewModel>();
            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            CheckStatus();
            ViewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
            => ViewModel.PropertyChanged -= OnViewModelPropertyChanged;

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
            _navigationViewModel.NavigateToSecondaryView(pageId);
            HideFlyout();
        }

        private void OnSignOutButtonClick(object sender, RoutedEventArgs e)
            => HideFlyout();

        private async void OnNavigateToMyHomePageButtonClickAsync(object sender, RoutedEventArgs e)
        {
            HideFlyout();
            await Launcher.LaunchUriAsync(new Uri($"https://space.bilibili.com/{ViewModel.Mid}/")).AsTask();
        }

        private void OnRequestCloseFlyout(object sender, EventArgs e)
            => HideFlyout();

        private void HideFlyout()
            => FlyoutBase.GetAttachedFlyout(UserAvatar).Hide();

        private void OnFlyoutOpened(object sender, object e)
            => ViewModel.InitializeCommunityCommand.ExecuteAsync(null);

        private void OnUserAvatarClick(object sender, EventArgs e)
        {
            if (ViewModel.State == AuthorizeState.SignedOut)
            {
                ViewModel.TrySignInCommand.ExecuteAsync(false);
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
    public class AccountAvatarBase : ReactiveUserControl<IAccountViewModel>
    {
    }
}
