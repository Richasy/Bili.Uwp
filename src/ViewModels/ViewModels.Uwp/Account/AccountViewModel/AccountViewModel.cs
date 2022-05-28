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
    public partial class AccountViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AccountViewModel"/> class.
        /// </summary>
        internal AccountViewModel(
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
                    _accountInformation = await _accountProvider.GetMyInformationAsync();
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
                var unread = await _accountProvider.GetUnreadMessageAsync();
                MessageModuleViewModel.Instance.InitializeUnreadCount(unread);
                UnreadMessageCount = unread.At + unread.Like + unread.Reply;
                IsShowUnreadMessage = UnreadMessageCount != 0;
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 新增固定的条目.
        /// </summary>
        /// <param name="item">条目信息.</param>
        /// <returns><see cref="Task"/>.</returns>
        public async Task AddFixedItemAsync(FixedItem item)
        {
            if (!IsConnected || _accountInformation == null || FixedItemCollection.Contains(item))
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
            if (!IsConnected || _accountInformation == null || !FixedItemCollection.Any(p => p.Id == itemId))
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
            if (_accountInformation == null)
            {
                return;
            }

            Avatar = _accountInformation.User.Avatar.Uri;
            DisplayName = _accountInformation.User.Name;
            Level = _accountInformation.Level;
            TipText = $"{_accountInformation.User.Name} Lv.{_accountInformation.Level}";
            IsVip = _accountInformation.IsVip;

            await InitUnreadAsync();
        }

        private void Reset()
        {
            _accountInformation = null;
            _isRequestLogout = false;
            Avatar = string.Empty;
            DisplayName = string.Empty;
            Level = 0;
            TipText = _resourceToolkit.GetLocaleString(LanguageNames.PleaseSignIn);
            IsVip = false;
            IsConnected = false;
            IsShowUnreadMessage = false;
            IsShowFixedItem = false;
            UnreadMessageCount = 0;
        }

        private async Task InitializeFixedItemAsync()
        {
            if (IsConnected && _accountInformation != null)
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
