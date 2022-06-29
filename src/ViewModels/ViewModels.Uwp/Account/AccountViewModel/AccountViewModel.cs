// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Bili.Lib.Interfaces;
using Bili.Models.App.Constants;
using Bili.Models.Data.Local;
using Bili.Models.Enums;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Uwp.Core;
using ReactiveUI;
using Windows.UI.Core;

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
            AppViewModel appViewModel,
            CoreDispatcher dispatcher)
        {
            _resourceToolkit = resourceToolkit;
            _numberToolkit = numberToolkit;
            _fileToolkit = fileToolkit;
            _authorizeProvider = authorizeProvider;
            _accountProvider = accountProvider;
            _appViewModel = appViewModel;
            _dispatcher = dispatcher;

            TrySignInCommand = ReactiveCommand.CreateFromTask<bool>(TrySignInAsync, outputScheduler: RxApp.MainThreadScheduler);
            SignOutCommand = ReactiveCommand.CreateFromTask(SignOutAsync, outputScheduler: RxApp.MainThreadScheduler);
            LoadMyProfileCommand = ReactiveCommand.CreateFromTask(GetMyProfileAsync, outputScheduler: RxApp.MainThreadScheduler);
            InitializeCommunityCommand = ReactiveCommand.CreateFromTask(InitCommunityInformationAsync, outputScheduler: RxApp.MainThreadScheduler);
            InitializeUnreadCommand = ReactiveCommand.CreateFromTask(InitUnreadAsync, outputScheduler: RxApp.MainThreadScheduler);
            AddFixedItemCommand = ReactiveCommand.CreateFromTask<FixedItem>(AddFixedItemAsync, outputScheduler: RxApp.MainThreadScheduler);
            RemoveFixedItemCommand = ReactiveCommand.CreateFromTask<string>(RemoveFixedItemAsync, outputScheduler: RxApp.MainThreadScheduler);

            _isSigning = TrySignInCommand.IsExecuting.ToProperty(this, x => x.IsSigning, scheduler: RxApp.MainThreadScheduler);

            FixedItemCollection = new ObservableCollection<FixedItem>();
            _authorizeProvider.StateChanged += OnAuthorizeStateChanged;
            State = _authorizeProvider.State;

            TrySignInCommand.ThrownExceptions
                .Merge(SignOutCommand.ThrownExceptions)
                .Merge(LoadMyProfileCommand.ThrownExceptions)
                .Merge(InitializeCommunityCommand.ThrownExceptions)
                .Merge(InitializeUnreadCommand.ThrownExceptions)
                .Merge(AddFixedItemCommand.ThrownExceptions)
                .Merge(RemoveFixedItemCommand.ThrownExceptions)
                .Subscribe(LogException);

            PropertyChanged += OnPropertyChanged;
            Reset();

            if (State == AuthorizeState.SignedIn)
            {
                LoadMyProfileCommand.Execute().Subscribe();
            }
        }

        /// <summary>
        /// 尝试登录.
        /// </summary>
        /// <param name="isSlientOnly">是否只进行静默登录.</param>
        /// <returns><see cref="Task"/>.</returns>
        private async Task TrySignInAsync(bool isSlientOnly = false)
        {
            if (State != AuthorizeState.SignedOut)
            {
                return;
            }

            await InternalSignInAsync(isSlientOnly);
        }

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
                if (_appViewModel.IsNetworkAvaliable
                    && await _authorizeProvider.IsTokenValidAsync())
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
            InitializeUnreadCommand.Execute().Subscribe();
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

            InitializeUnreadCommand.Execute().Subscribe();
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

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(State))
            {
                if (State == AuthorizeState.SignedIn)
                {
                    IsConnected = true;
                    LoadMyProfileCommand.Execute().Subscribe();
                }
            }
        }
    }
}
