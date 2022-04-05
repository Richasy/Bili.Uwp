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
            FixedPublisherCollection = new ObservableCollection<FixedPublisher>();
            FixedPgcCollection = new ObservableCollection<FixedPgc>();
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
        /// 新增固定的UP主.
        /// </summary>
        /// <param name="publisher">UP主信息.</param>
        /// <returns><see cref="Task"/>.</returns>
        public async Task AddFixedPublisherAsync(FixedPublisher publisher)
        {
            if (!IsConnected || _myInfo == null || FixedPublisherCollection.Contains(publisher))
            {
                return;
            }

            FixedPublisherCollection.Add(publisher);
            await _fileToolkit.WriteLocalDataAsync(
                string.Format(AppConstants.FixedContentFileName, _myInfo.Mid),
                FixedPublisherCollection.ToList(),
                AppConstants.FixedPublisherFolderName);
            IsShowFixedPublisher = true;
        }

        /// <summary>
        /// 移除固定的UP主.
        /// </summary>
        /// <param name="userId">用户Id.</param>
        /// <returns><see cref="Task"/>.</returns>
        public async Task RemoveFixedPublisherAsync(string userId)
        {
            if (!IsConnected || _myInfo == null || !FixedPublisherCollection.Any(p => p.UserId == userId))
            {
                return;
            }

            FixedPublisherCollection.Remove(FixedPublisherCollection.FirstOrDefault(p => p.UserId == userId));
            await _fileToolkit.WriteLocalDataAsync(
                string.Format(AppConstants.FixedContentFileName, _myInfo.Mid),
                FixedPublisherCollection.ToList(),
                AppConstants.FixedPublisherFolderName);
            IsShowFixedPublisher = FixedPublisherCollection.Count > 0;
        }

        /// <summary>
        /// 新增固定的UP主.
        /// </summary>
        /// <param name="pgc">剧集信息.</param>
        /// <returns><see cref="Task"/>.</returns>
        public async Task AddFixedPgcAsync(FixedPgc pgc)
        {
            if (!IsConnected || _myInfo == null || FixedPgcCollection.Contains(pgc))
            {
                return;
            }

            FixedPgcCollection.Add(pgc);
            await _fileToolkit.WriteLocalDataAsync(
                string.Format(AppConstants.FixedContentFileName, _myInfo.Mid),
                FixedPgcCollection.ToList(),
                AppConstants.FixedPgcFolderName);
            IsShowFixedPgc = true;
        }

        /// <summary>
        /// 移除固定的剧集.
        /// </summary>
        /// <param name="seasonId">剧集Id.</param>
        /// <returns><see cref="Task"/>.</returns>
        public async Task RemoveFixedPgcAsync(int seasonId)
        {
            if (!IsConnected || _myInfo == null || !FixedPgcCollection.Any(p => p.SeasonId == seasonId))
            {
                return;
            }

            FixedPgcCollection.Remove(FixedPgcCollection.FirstOrDefault(p => p.SeasonId == seasonId));
            await _fileToolkit.WriteLocalDataAsync(
                string.Format(AppConstants.FixedContentFileName, _myInfo.Mid),
                FixedPgcCollection.ToList(),
                AppConstants.FixedPgcFolderName);
            IsShowFixedPgc = FixedPgcCollection.Count > 0;
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
                await InitializeFixedPublisherAsync();
                await InitializeFixedPgcAsync();
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
            IsShowFixedPublisher = false;
            FixedPublisherCollection.Clear();
            UnreadMessageCount = 0;
        }

        private async Task InitializeFixedPublisherAsync()
        {
            if (IsConnected && _myInfo != null)
            {
                var data = await _fileToolkit.ReadLocalDataAsync<List<FixedPublisher>>(
                    string.Format(AppConstants.FixedContentFileName, _myInfo.Mid),
                    "[]",
                    AppConstants.FixedPublisherFolderName);
                FixedPublisherCollection.Clear();
                if (data.Count > 0)
                {
                    data.ForEach(p => FixedPublisherCollection.Add(p));
                    IsShowFixedPublisher = true;
                    return;
                }
            }

            IsShowFixedPublisher = false;
        }

        private async Task InitializeFixedPgcAsync()
        {
            if (IsConnected && _myInfo != null)
            {
                var data = await _fileToolkit.ReadLocalDataAsync<List<FixedPgc>>(
                    string.Format(AppConstants.FixedContentFileName, _myInfo.Mid),
                    "[]",
                    AppConstants.FixedPgcFolderName);
                FixedPgcCollection.Clear();
                if (data.Count > 0)
                {
                    data.ForEach(p => FixedPgcCollection.Add(p));
                    IsShowFixedPgc = true;
                    return;
                }
            }

            IsShowFixedPgc = false;
        }
    }
}
