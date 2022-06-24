// Copyright (c) Richasy. All rights reserved.

using Bili.ViewModels.Uwp.Account;

namespace Bili.App.Controls.User
{
    /// <summary>
    /// 用户条目.
    /// </summary>
    public sealed class UserItem : ReactiveControl<UserItemViewModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserItem"/> class.
        /// </summary>
        public UserItem() => DefaultStyleKey = typeof(UserItem);
    }
}
