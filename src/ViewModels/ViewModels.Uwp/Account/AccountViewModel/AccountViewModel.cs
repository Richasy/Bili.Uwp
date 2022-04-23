// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Richasy.Bili.Controller.Uwp;
using Richasy.Bili.Locator.Uwp;
using Richasy.Bili.Models.App;
using Richasy.Bili.Models.App.Constants;
using Richasy.Bili.Models.BiliBili;
using Richasy.Bili.Models.Enums;

namespace Richasy.Bili.ViewModels.Uwp
{
    /// <summary>
    /// 用户视图模型.
    /// </summary>
    public partial class AccountViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AccountViewModel"/> class.
        /// </summary>
        internal AccountViewModel()
        {
            FixedItemCollection = new ObservableCollection<FixedItem>();
            _controller = BiliController.Instance;
            _controller.Logged += OnLoggedAsync;
            _controller.LoggedFailed += OnLoggedFailedAsync;
            _controller.LoggedOut += OnLoggedOut;
            _controller.AccountChanged += OnAccountChangedAsync;
            Status = AccountViewModelStatus.Logout;
            ServiceLocator.Instance.LoadService(out _resourceToolkit)
                                   .LoadService(out _numberToolkit)
                                   .LoadService(out _fileToolkit);
            Reset();
        }

        /// <summary>
        /// 尝试登录.
        /// </summary>
        /// <param name="isSlientOnly">是否只进行静默登录.</param>
        /// <returns><see cref="Task"/>.</returns>
        public async Task<bool> TrySignInAsync(bool isSlientOnly = false)
        {
            if (Status != AccountViewModelStatus.Logout)
            {
                return Status == AccountViewModelStatus.Login;
            }

            Status = AccountViewModelStatus.Logging;
            return await _controller.TrySignInAsync(isSlientOnly);
        }

        /// <summary>
        /// 登出.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        public async Task SignOutAsync()
            => await _controller.SignOutAsync();

        /// <summary>
        /// 获取我的账户资料.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        public async Task GetMyProfileAsync()
        {
            try
            {
                await _controller.RequestMyProfileAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Status = AccountViewModelStatus.Logout;
            }
        }

        /// <summary>
        /// 初始化用户社交信息.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        public async Task InitCommunityInformationAsync()
        {
            try
            {
                var data = await _controller.GetMyDataAsync();
                DynamicCount = _numberToolkit.GetCountText(data.DynamicCount);
                FollowCount = _numberToolkit.GetCountText(data.FollowCount);
                FollowerCount = _numberToolkit.GetCountText(data.FollowerCount);

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
                var unread = await _controller.GetUnreadMessageAsync();
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
            if (!IsConnected || _myInfo == null || FixedItemCollection.Contains(item))
            {
                return;
            }

            FixedItemCollection.Add(item);
            await _fileToolkit.WriteLocalDataAsync(
                string.Format(AppConstants.FixedContentFileName, _myInfo.Mid),
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
            if (!IsConnected || _myInfo == null || !FixedItemCollection.Any(p => p.Id == itemId))
            {
                return;
            }

            FixedItemCollection.Remove(FixedItemCollection.FirstOrDefault(p => p.Id == itemId));
            await _fileToolkit.WriteLocalDataAsync(
                string.Format(AppConstants.FixedContentFileName, _myInfo.Mid),
                FixedItemCollection.ToList(),
                AppConstants.FixedFolderName);
            IsShowFixedItem = FixedItemCollection.Count > 0;
        }

        private void OnLoggedOut(object sender, EventArgs e)
        {
            Status = AccountViewModelStatus.Logout;
            Reset();
        }

        private async void OnLoggedFailedAsync(object sender, Exception e)
        {
            Debug.WriteLine($"Login failed: {e.Message}");

            // 它仅在用户未登录时触发.
            if (Status != AccountViewModelStatus.Login)
            {
                Reset();
                Status = AccountViewModelStatus.Logout;
                await _controller.SignOutAsync();
            }
        }

        private async void OnLoggedAsync(object sender, EventArgs e)
        {
            if (Status != AccountViewModelStatus.Login)
            {
                IsConnected = true;
                await GetMyProfileAsync();
                await InitializeFixedItemAsync();
                Status = AccountViewModelStatus.Login;
            }
        }

        private async void OnAccountChangedAsync(object sender, MyInfo e)
        {
            if (e != null)
            {
                _myInfo = e;
                Avatar = e.Avatar;
                DisplayName = e.Name;
                Level = e.Level;
                TipText = $"{e.Name} Lv.{e.Level}";
                IsVip = e.VIP.Status == 1;

                await InitUnreadAsync();
            }
        }

        private void Reset()
        {
            _myInfo = null;
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
            if (IsConnected && _myInfo != null)
            {
                var data = await _fileToolkit.ReadLocalDataAsync<List<FixedItem>>(
                    string.Format(AppConstants.FixedContentFileName, _myInfo.Mid),
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
