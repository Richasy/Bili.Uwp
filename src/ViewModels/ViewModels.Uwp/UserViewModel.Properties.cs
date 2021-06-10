// Copyright (c) Richasy. All rights reserved.

using System;
using Richasy.Bili.Lib.Interfaces;

namespace Richasy.Bili.ViewModels.Uwp
{
    /// <summary>
    /// 用户试图模型属性集.
    /// </summary>
    public partial class UserViewModel
    {
        private readonly IAuthorizeProvider _authorizeProvider;

        /// <summary>
        /// <see cref="UserViewModel"/>的实例.
        /// </summary>
        public static UserViewModel Instance { get; } = new Lazy<UserViewModel>(() => new UserViewModel()).Value;
    }
}
