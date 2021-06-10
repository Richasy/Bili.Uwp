// Copyright (c) Richasy. All rights reserved.

using System.Threading.Tasks;
using Richasy.Bili.Locator.Uwp;

namespace Richasy.Bili.ViewModels.Uwp
{
    /// <summary>
    /// 用户视图模型.
    /// </summary>
    public partial class UserViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserViewModel"/> class.
        /// </summary>
        internal UserViewModel()
        {
            ServiceLocator.Instance.LoadService(out _authorizeProvider);
        }

        /// <summary>
        /// 登录账户.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        public async Task SignInAsync()
        {
            await _authorizeProvider.SignInAsync();
        }
    }
}
