// Copyright (c) Richasy. All rights reserved.

using System;
using Bili.Models.App.Constants;
using Windows.Storage;
using Windows.System;

namespace Bili.Desktop.App.Controls
{
    /// <summary>
    /// 截图设置区块.
    /// </summary>
    public sealed partial class ScreenshotSettingSection : SettingSectionControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScreenshotSettingSection"/> class.
        /// </summary>
        public ScreenshotSettingSection()
            : base() => InitializeComponent();

        private async void OnOpenScreenshotFolderButtonClickAsync(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            var picFolder = await KnownFolders.PicturesLibrary.CreateFolderAsync(AppConstants.ScreenshotFolderName, CreationCollisionOption.OpenIfExists);
            await Launcher.LaunchFolderAsync(picFolder).AsTask();
        }
    }
}
