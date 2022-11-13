// Copyright (c) Richasy. All rights reserved.

using System;
using System.Linq;
using Bili.DI.Container;
using Bili.ViewModels.Interfaces.Account;
using Microsoft.UI.Xaml;

namespace Bili.ViewModels.Desktop.Live
{
    /// <summary>
    /// 直播播放页面视图模型.
    /// </summary>
    public sealed partial class LivePlayerPageViewModel
    {
        private void InitializeTimers()
        {
            if (_heartBeatTimer == null)
            {
                _heartBeatTimer = new DispatcherTimer();
                _heartBeatTimer.Interval = TimeSpan.FromSeconds(25);
                _heartBeatTimer.Tick += OnHeartBeatTimerTickAsync;
            }
        }

        private void InitializePublisher()
        {
            var profile = View.Information.User;
            var vm = Locator.Instance.GetService<IUserItemViewModel>();
            vm.SetProfile(profile);
            User = vm;
            User.InitializeRelationCommand.ExecuteAsync(null);
        }

        private void InitializeOverview()
            => WatchingCountText = _numberToolkit.GetCountText(View.Information.ViewerCount);

        private void InitializeInterop()
        {
            var fixedItems = _accountViewModel.FixedItemCollection;
            IsLiveFixed = fixedItems.Any(p => p.Type == Models.Enums.App.FixedType.Live && p.Id == View.Information.Identifier.Id);
        }
    }
}
