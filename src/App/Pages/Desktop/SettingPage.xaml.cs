// Copyright (c) Richasy. All rights reserved.

using Bili.ViewModels.Uwp.Core;
using Bili.ViewModels.Uwp.Home;

namespace Bili.App.Pages.Desktop
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页.
    /// </summary>
    public sealed partial class SettingsPage : SettingsPageBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsPage"/> class.
        /// </summary>
        public SettingsPage() => InitializeComponent();
    }

    /// <summary>
    /// <see cref="SettingsPage"/> 的基类.
    /// </summary>
    public class SettingsPageBase : AppPage<SettingsPageViewModel>
    {
    }
}
