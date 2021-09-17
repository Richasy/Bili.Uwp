// Copyright (c) Richasy. All rights reserved.

using Richasy.Bili.ViewModels.Uwp;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Richasy.Bili.App.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页.
    /// </summary>
    public sealed partial class SettingPage : Page
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SettingPage"/> class.
        /// </summary>
        public SettingPage()
        {
            this.InitializeComponent();
            ViewModel = SettingViewModel.Instance;
        }

        /// <summary>
        /// 视图模型.
        /// </summary>
        public SettingViewModel ViewModel { get; private set; }

        /// <inheritdoc/>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ViewModel.InitializeSettings();
        }
    }
}
