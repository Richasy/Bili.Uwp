// Copyright (c) Richasy. All rights reserved.

using System;
using System.Threading.Tasks;
using Bili.Lib.Interfaces;
using Bili.Models.Enums;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Account;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Dispatching;

namespace Bili.ViewModels.Workspace.Account
{
    /// <summary>
    /// 用户视图模型.
    /// </summary>
    public sealed partial class AccountViewModel : ViewModelBase, IAccountViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AccountViewModel"/> class.
        /// </summary>
        public AccountViewModel(
            INumberToolkit numberToolkit,
            IAuthorizeProvider authorizeProvider,
            IAccountProvider accountProvider)
        {
            _numberToolkit = numberToolkit;
            _authorizeProvider = authorizeProvider;
            _accountProvider = accountProvider;
            _dispatcher = DispatcherQueue.GetForCurrentThread();

            TrySignInCommand = new AsyncRelayCommand<bool>(TrySignInAsync);
            SignOutCommand = new AsyncRelayCommand(SignOutAsync);
            LoadMyProfileCommand = new AsyncRelayCommand(GetMyProfileAsync);
            InitializeCommunityCommand = new AsyncRelayCommand(InitCommunityInformationAsync);
            InitializeUnreadCommand = new AsyncRelayCommand(InitUnreadAsync);

            _authorizeProvider.StateChanged += OnAuthorizeStateChanged;
            State = _authorizeProvider.State;

            AttachExceptionHandlerToAsyncCommand(
                LogException,
                TrySignInCommand,
                SignOutCommand,
                LoadMyProfileCommand,
                InitializeCommunityCommand,
                InitializeUnreadCommand);

            Reset();

            if (State == AuthorizeState.SignedIn)
            {
                HandleLogged();
            }
        }

        /// <summary>
        /// 尝试登录.
        /// </summary>
        /// <param name="isSlientOnly">是否只进行静默登录.</param>
        /// <returns><see cref="Task"/>.</returns>
        private async Task<bool> TrySignInAsync(bool isSlientOnly = false)
            => State == AuthorizeState.SignedOut && await InternalSignInAsync(isSlientOnly);

        /// <summary>
        /// 登出.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        private async Task SignOutAsync()
        {
            _isRequestLogout = true;
            await _authorizeProvider.SignOutAsync();
        }

        /// <summary>
        /// 获取我的账户资料.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        private async Task GetMyProfileAsync()
        {
            try
            {
                if (await _authorizeProvider.IsTokenValidAsync())
                {
                    AccountInformation = await _accountProvider.GetMyInformationAsync();
                    IsConnected = true;
                }
            }
            catch (Exception)
            {
                State = AuthorizeState.SignedOut;
                throw;
            }

            InitializeAccountInformation();
        }

        /// <summary>
        /// 初始化用户社交信息.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        private async Task InitCommunityInformationAsync()
        {
            var data = await _accountProvider.GetMyCommunityInformationAsync();
            DynamicCount = _numberToolkit.GetCountText(data.DynamicCount);
            FollowCount = _numberToolkit.GetCountText(data.FollowCount);
            FollowerCount = _numberToolkit.GetCountText(data.FansCount);
            await InitUnreadAsync();
        }

        /// <summary>
        /// 加载未读消息数据.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        private async Task InitUnreadAsync()
        {
            UnreadInformation = await _accountProvider.GetUnreadMessageAsync();
            IsShowUnreadMessage = UnreadInformation.Total > 0;
        }

        private void InitializeAccountInformation()
        {
            if (AccountInformation == null)
            {
                IsConnected = false;
                return;
            }

            Avatar = AccountInformation.User.Avatar.GetSourceUri().ToString();
            DisplayName = AccountInformation.User.Name;
            Level = AccountInformation.Level;
            TipText = $"{AccountInformation.User.Name} Lv.{AccountInformation.Level}";
            IsVip = AccountInformation.IsVip;

            InitializeUnreadCommand.ExecuteAsync(null);
        }

        private void Reset()
        {
            AccountInformation = null;
            _isRequestLogout = false;
            Avatar = string.Empty;
            DisplayName = string.Empty;
            Level = 0;
            IsVip = false;
            IsConnected = false;
            IsShowUnreadMessage = false;
            UnreadInformation = null;
        }

        partial void OnStateChanged(AuthorizeState value)
        {
            if (value == AuthorizeState.SignedIn)
            {
                IsConnected = true;
                LoadMyProfileCommand.ExecuteAsync(null);
            }
        }
    }
}
