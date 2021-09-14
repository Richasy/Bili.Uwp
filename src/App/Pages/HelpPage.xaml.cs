// Copyright (c) Richasy. All rights reserved.

using System;
using Windows.System;
using Windows.UI.Xaml.Controls;

namespace Richasy.Bili.App.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页.
    /// </summary>
    public sealed partial class HelpPage : Page
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HelpPage"/> class.
        /// </summary>
        public HelpPage()
        {
            this.InitializeComponent();
        }

        private async void OnAskIssueButtonClickAsync(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri("https://github.com/Richasy/Bili.Uwp/issues/new")).AsTask();
        }

        private async void OnProjectHomePageClickAsync(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri("https://github.com/Richasy/Bili.Uwp/")).AsTask();
        }

        private async void OnBiliHomePageClickAsync(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri("https://space.bilibili.com/5992670")).AsTask();
        }
    }
}
