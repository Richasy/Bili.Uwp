// Copyright (c) Richasy. All rights reserved.

using Bili.DI.Container;
using Bili.ViewModels.Interfaces.Core;
using Bili.ViewModels.Interfaces.Home;

namespace Bili.Desktop.App.Controls
{
    /// <summary>
    /// 设置区块基类.
    /// </summary>
    public class SettingSectionControl : ReactiveUserControl<ISettingsPageViewModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SettingSectionControl"/> class.
        /// </summary>
        public SettingSectionControl()
        {
            ViewModel = Locator.Instance.GetService<ISettingsPageViewModel>();
            IsTabStop = false;
        }

        /// <summary>
        /// 核心视图模型.
        /// </summary>
        public IAppViewModel CoreViewModel { get; } = Locator.Instance.GetService<IAppViewModel>();
    }
}
