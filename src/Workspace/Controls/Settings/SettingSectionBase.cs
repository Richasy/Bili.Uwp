// Copyright (c) Richasy. All rights reserved.

using Bili.DI.Container;
using Bili.ViewModels.Interfaces.Workspace;

namespace Bili.Workspace.Controls.Settings
{
    /// <summary>
    /// 设置区块基类.
    /// </summary>
    public class SettingSectionBase : ReactiveUserControl<ISettingsViewModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SettingSectionBase"/> class.
        /// </summary>
        public SettingSectionBase()
        {
            ViewModel = Locator.Instance.GetService<ISettingsViewModel>();
            IsTabStop = false;
        }
    }
}
