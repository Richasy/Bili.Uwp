// Copyright (c) Richasy. All rights reserved.

using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Richasy.Bili.Controller.Uwp;
using Richasy.Bili.Locator.Uwp;
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
            _controller = BiliController.Instance;
            _controller.Logged += OnLoggedAsync;
            _controller.LoggedFailed += OnLoggedFailed;
            _controller.LoggedOut += OnLoggedOut;
            _controller.AccountChanged += OnAccountChangedAsync;
            Status = AccountViewModelStatus.Logout;
            ServiceLocator.Instance.LoadService(out _resourceToolkit)
                                   .LoadService(out _numberToolkit);
            Reset();
        }

        /// <summary>
        /// 尝试登录.
        /// </summary>
        /// <param name="isSlientOnly">是否只进行静默登录.</param>
        /// <returns><see cref="Task"/>.</returns>
        public async Task TrySignInAsync(bool isSlientOnly = false)
        {
            this.Status = AccountViewModelStatus.Logging;
            await _controller.TrySignInAsync(isSlientOnly);
        }

        /// <summary>
        /// 登出.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        public async Task SignOutAsync()
        {
            await _controller.SignOutAsync();
        }

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
            var data = await _controller.GetMyDataAsync();
            DynamicCount = _numberToolkit.GetCountText(data.DynamicCount);
            FollowCount = _numberToolkit.GetCountText(data.FollowCount);
            FollowerCount = _numberToolkit.GetCountText(data.FollowerCount);

            await InitUnreadAsync();
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

        private void OnLoggedOut(object sender, EventArgs e)
        {
            this.Status = AccountViewModelStatus.Logout;
            Reset();
        }

        private void OnLoggedFailed(object sender, Exception e)
        {
            Debug.WriteLine($"Login failed: {e.Message}");

            // 它仅在用户未登录时触发.
            if (this.Status != AccountViewModelStatus.Login)
            {
                Reset();
                this.Status = AccountViewModelStatus.Logout;
            }
        }

        private async void OnLoggedAsync(object sender, EventArgs e)
        {
            if (this.Status != AccountViewModelStatus.Login)
            {
                IsConnected = true;
                await GetMyProfileAsync();
                this.Status = AccountViewModelStatus.Login;
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
            UnreadMessageCount = 0;
        }
    }
}
