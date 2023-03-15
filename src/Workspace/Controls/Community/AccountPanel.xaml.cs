// Copyright (c) Richasy. All rights reserved.

using System;
using System.Threading.Tasks;
using Bili.DI.Container;
using Bili.Models.Enums.Workspace;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Account;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls.Primitives;
using Windows.System;

namespace Bili.Workspace.Controls.Community
{
    /// <summary>
    /// 账户面板.
    /// </summary>
    public sealed partial class AccountPanel : AccountPanelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AccountPanel"/> class.
        /// </summary>
        public AccountPanel()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            ViewModel.InitializeCommunityCommand.Execute(default);
            ViewModel.InitializeUnreadCommand.Execute(default);
        }

        private void OnAvatarClick(object sender, EventArgs e)
            => FlyoutBase.ShowAttachedFlyout(Avatar);

        private async void OnDynamicButtonClickAsync(object sender, RoutedEventArgs e)
            => await NavigateAsync(string.Empty, $"https://space.bilibili.com/{ViewModel.Mid}/dynamic");

        private async void OnFollowButtonClickAsync(object sender, RoutedEventArgs e)
            => await NavigateAsync("richasy-bili://navigate?id=follow", $"https://space.bilibili.com/{ViewModel.Mid}/fans/follow");

        private async void OnFollowerButtonClickAsync(object sender, RoutedEventArgs e)
            => await NavigateAsync("richasy-bili://navigate?id=fans", $"https://space.bilibili.com/{ViewModel.Mid}/fans/fans");

        private async void OnMessageButtonClickAsync(object sender, RoutedEventArgs e)
            => await NavigateAsync("richasy-bili://navigate?id=message", $"https://message.bilibili.com/");

        private async void OnPersonalItemClickAsync(object sender, RoutedEventArgs e)
            => await NavigateAsync(string.Empty, $"https://space.bilibili.com/{ViewModel.Mid}");

        private async void OnFavoriteItemClickAsync(object sender, RoutedEventArgs e)
            => await NavigateAsync("richasy-bili://navigate?id=favorite", $"https://space.bilibili.com/{ViewModel.Mid}/favlist");

        private async void OnViewLaterClickAsync(object sender, RoutedEventArgs e)
            => await NavigateAsync("richasy-bili://navigate?id=viewLater", $"https://www.bilibili.com/watchlater");

        private async Task NavigateAsync(string biliUri, string webUri)
        {
            if (string.IsNullOrEmpty(biliUri))
            {
                await Launcher.LaunchUriAsync(new Uri(webUri));
                return;
            }

            var settingsToolkit = Locator.Instance.GetService<ISettingsToolkit>();
            var perferLaunch = settingsToolkit.ReadLocalSetting(Models.Enums.SettingNames.LaunchType, LaunchType.Web);
            var uri = perferLaunch == LaunchType.Web
                ? new Uri(webUri)
                : new Uri(biliUri);
            await Launcher.LaunchUriAsync(uri);
        }
    }

    /// <summary>
    /// <see cref="AccountPanel"/>的基类.
    /// </summary>
    public class AccountPanelBase : ReactiveUserControl<IAccountViewModel>
    {
    }
}
