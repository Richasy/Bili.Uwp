// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Bili.Lib.Interfaces;
using Bili.Models.App.Constants;
using Bili.Models.Data.Local;
using Bili.Models.Enums;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Account;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Dispatching;

namespace Bili.ViewModels.Desktop.Account
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
            IResourceToolkit resourceToolkit,
            INumberToolkit numberToolkit,
            IFileToolkit fileToolkit,
            IAuthorizeProvider authorizeProvider,
            IAccountProvider accountProvider)
        {
            _resourceToolkit = resourceToolkit;
            _numberToolkit = numberToolkit;
            _fileToolkit = fileToolkit;
            _authorizeProvider = authorizeProvider;
            _accountProvider = accountProvider;
            _dispatcherQueue = DispatcherQueue.GetForCurrentThread();

            TrySignInCommand = new AsyncRelayCommand<bool>(TrySignInAsync);
            SignOutCommand = new AsyncRelayCommand(SignOutAsync);
            LoadMyProfileCommand = new AsyncRelayCommand(GetMyProfileAsync);
            InitializeCommunityCommand = new AsyncRelayCommand(InitCommunityInformationAsync);
            InitializeUnreadCommand = new AsyncRelayCommand(InitUnreadAsync);
            AddFixedItemCommand = new AsyncRelayCommand<FixedItem>(AddFixedItemAsync);
            RemoveFixedItemCommand = new AsyncRelayCommand<string>(RemoveFixedItemAsync);

            FixedItemCollection = new ObservableCollection<FixedItem>();
            _authorizeProvider.StateChanged += OnAuthorizeStateChanged;
            State = _authorizeProvider.State;

            AttachExceptionHandlerToAsyncCommand(
                LogException,
                TrySignInCommand,
                SignOutCommand,
                LoadMyProfileCommand,
                InitializeCommunityCommand,
                InitializeUnreadCommand,
                AddFixedItemCommand,
                RemoveFixedItemCommand);

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

        /// <summary>
        /// 新增固定的条目.
        /// </summary>
        /// <param name="item">条目信息.</param>
        /// <returns><see cref="Task"/>.</returns>
        private async Task AddFixedItemAsync(FixedItem item)
        {
            if (!IsConnected || AccountInformation == null || FixedItemCollection.Contains(item))
            {
                return;
            }

            FixedItemCollection.Add(item);
            await _fileToolkit.WriteLocalDataAsync(
                string.Format(AppConstants.FixedContentFileName, Mid),
                FixedItemCollection.ToList(),
                AppConstants.FixedFolderName);
            IsShowFixedItem = true;
        }

        /// <summary>
        /// 移除固定的条目.
        /// </summary>
        /// <param name="itemId">条目Id.</param>
        /// <returns><see cref="Task"/>.</returns>
        private async Task RemoveFixedItemAsync(string itemId)
        {
            if (!IsConnected || AccountInformation == null || !FixedItemCollection.Any(p => p.Id == itemId))
            {
                return;
            }

            FixedItemCollection.Remove(FixedItemCollection.FirstOrDefault(p => p.Id == itemId));
            await _fileToolkit.WriteLocalDataAsync(
                string.Format(AppConstants.FixedContentFileName, Mid),
                FixedItemCollection.ToList(),
                AppConstants.FixedFolderName);
            IsShowFixedItem = FixedItemCollection.Count > 0;
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
            TipText = _resourceToolkit.GetLocaleString(LanguageNames.PleaseSignIn);
            IsVip = false;
            IsConnected = false;
            IsShowUnreadMessage = false;
            IsShowFixedItem = false;
            UnreadInformation = null;
        }

        private async Task InitializeFixedItemAsync()
        {
            if (IsConnected && AccountInformation != null)
            {
                var data = await _fileToolkit.ReadLocalDataAsync<List<FixedItem>>(
                    string.Format(AppConstants.FixedContentFileName, Mid),
                    "[]",
                    AppConstants.FixedFolderName);
                TryClear(FixedItemCollection);
                if (data.Count > 0)
                {
                    data.ForEach(p => FixedItemCollection.Add(p));
                    IsShowFixedItem = true;
                    return;
                }
            }

            IsShowFixedItem = false;
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
