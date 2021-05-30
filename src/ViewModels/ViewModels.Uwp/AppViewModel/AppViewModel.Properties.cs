// Copyright (c) Richasy. All rights reserved.

using System;

namespace Richasy.Bili.ViewModels.Uwp
{
    /// <summary>
    /// <see cref="AppViewModel"/>的属性集.
    /// </summary>
    public partial class AppViewModel
    {
        /// <summary>
        /// <see cref="AppViewModel"/>的单例.
        /// </summary>
        public static AppViewModel Instance { get; } = new Lazy<AppViewModel>(() => new AppViewModel()).Value;
    }
}
