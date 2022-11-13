// Copyright (c) Richasy. All rights reserved.

using Bili.ViewModels.Interfaces.Account;

namespace Bili.Uwp.App.Controls.User
{
    /// <summary>
    /// 用户条目.
    /// </summary>
    public sealed class UserItem : ReactiveControl<IUserItemViewModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserItem"/> class.
        /// </summary>
        public UserItem() => DefaultStyleKey = typeof(UserItem);
    }
}
