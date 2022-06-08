// Copyright (c) Richasy. All rights reserved.

using Bili.ViewModels.Uwp.Home;
using ReactiveUI;
using Splat;

namespace Bili.App.Controls
{
    /// <summary>
    /// 设置区块基类.
    /// </summary>
    public class SettingSectionControl : ReactiveUserControl<SettingsPageViewModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SettingSectionControl"/> class.
        /// </summary>
        public SettingSectionControl()
            => ViewModel = Splat.Locator.Current.GetService<SettingsPageViewModel>();
    }
}
