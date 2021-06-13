// Copyright (c) Richasy. All rights reserved.

using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Richasy.Bili.Controller.Uwp;
using Richasy.Bili.Models.BiliBili;

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
            _controller.AccountChanged += OnAccountChanged;
            Status = AccountViewModelStatus.Logout;
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
        /// 获取我的账户资料.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        public async Task GetMyProfileAsync()
        {
            try
            {
                await _controller.GetMyProfileAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Status = AccountViewModelStatus.Logout;
            }
        }

        private void OnLoggedOut(object sender, EventArgs e)
        {
            this.Status = AccountViewModelStatus.Logout;
        }

        private void OnLoggedFailed(object sender, Exception e)
        {
            Debug.WriteLine($"Login failed: {e.Message}");

            // 它仅在用户未登录时触发.
            if (this.Status != AccountViewModelStatus.Login)
            {
                this.Status = AccountViewModelStatus.Logout;
            }
        }

        private async void OnLoggedAsync(object sender, EventArgs e)
        {
            if (this.Status != AccountViewModelStatus.Login)
            {
                this.Status = AccountViewModelStatus.Login;
                await GetMyProfileAsync();
            }
        }

        private void OnAccountChanged(object sender, MyInfo e)
        {
            if (e != null)
            {
                Avatar = e.Avatar;
                DisplayName = e.Name;
            }
        }
    }
}
