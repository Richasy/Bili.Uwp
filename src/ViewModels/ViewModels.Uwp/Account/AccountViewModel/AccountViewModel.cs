// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Bili.Lib.Interfaces;
using Bili.Models.App;
using Bili.Models.App.Constants;
using Bili.Models.Enums;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Uwp.Core;
using ReactiveUI;

namespace Bili.ViewModels.Uwp.Account
{
    /// <summary>
    /// 用户视图模型.
    /// </summary>
    public sealed partial class AccountViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AccountViewModel"/> class.
        /// </summary>
        public AccountViewModel(
            IResourceToolkit resourceToolkit,
            INumberToolkit numberToolkit,
            IFileToolkit fileToolkit,
            IAuthorizeProvider authorizeProvider,
            IAccountProvider accountProvider,
            AppViewModel appViewModel)
        {
            _resourceToolkit = resourceToolkit;
            _numberToolkit = numberToolkit;
            _fileToolkit = fileToolkit;
            _authorizeProvider = authorizeProvider;
            _accountProvider = accountProvider;
            _appViewModel = appViewModel;

            FixedItemCollection = new ObservableCollection<FixedItem>();
            _authorizeProvider.StateChanged += OnAuthorizeStateChangedAsync;
            State = _authorizeProvider.State;

            this.WhenAnyValue(x => x.State)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(async state =>
                {
                    if (state == AuthorizeState.SignedIn)
                    {
                        IsConnected = true;
                        await GetMyProfileAsync();
                    }
                });

            Reset();
        }

        /// <summary>
        /// 尝试登录.
        /// </summary>
        /// <param name="isSlientOnly">是否只进行静默登录.</param>
        /// <returns><see cref="Task"/>.</returns>
        public async Task<bool> TrySignInAsync(bool isSlientOnly = false)
        {
            if (State != AuthorizeState.SignedOut)
            {
                return State == AuthorizeState.SignedIn;
            }

            State = AuthorizeState.Loading;
            return await InternalSignInAsync(isSlientOnly);
        }

        /// <summary>
        /// 登出.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        public async Task SignOutAsync()
        {
            _isRequestLogout = true;
            await _authorizeProvider.SignOutAsync();
        }

        /// <summary>
        /// 获取我的账户资料.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        public async Task GetMyProfileAsync()
        {
            try
            {
                if (_appViewModel.IsNetworkAvaliable
                    && await _authorizeProvider.IsTokenValidAsync())
                {
                    AccountInformation = await _accountProvider.GetMyInformationAsync();
                    IsConnected = true;
                }
            }
            catch (Exception ex)
            {
                LogException(ex);
                State = AuthorizeState.SignedOut;
            }

            await InitializeAccountInformationAsync();
        }

        /// <summary>
        /// 初始化用户社交信息.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        public async Task InitCommunityInformationAsync()
        {
            try
            {
                var data = await _accountProvider.GetMyCommunityInformationAsync();
                DynamicCount = _numberToolkit.GetCountText(data.DynamicCount);
                FollowCount = _numberToolkit.GetCountText(data.FollowCount);
                FollowerCount = _numberToolkit.GetCountText(data.FansCount);

                await InitUnreadAsync();
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 加载未读消息数据.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        public async Task InitUnreadAsync()
        {
            try
            {
                UnreadInformation = await _accountProvider.GetUnreadMessageAsync();
                IsShowUnreadMessage = UnreadInformation.Total > 0;
            }
            catch (Exception ex)
            {
                LogException(ex);
            }
        }

        /// <summary>
        /// 新增固定的条目.
        /// </summary>
        /// <param name="item">条目信息.</param>
        /// <returns><see cref="Task"/>.</returns>
        public async Task AddFixedItemAsync(FixedItem item)
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
        public async Task RemoveFixedItemAsync(string itemId)
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

        private async Task InitializeAccountInformationAsync()
        {
            if (AccountInformation == null)
            {
                IsConnected = false;
                return;
            }

            Avatar = AccountInformation.User.Avatar.Uri;
            DisplayName = AccountInformation.User.Name;
            Level = AccountInformation.Level;
            TipText = $"{AccountInformation.User.Name} Lv.{AccountInformation.Level}";
            IsVip = AccountInformation.IsVip;

            await InitUnreadAsync();
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
                FixedItemCollection.Clear();
                if (data.Count > 0)
                {
                    data.ForEach(p => FixedItemCollection.Add(p));
                    IsShowFixedItem = true;
                    return;
                }
            }

            IsShowFixedItem = false;
        }
    }
}
