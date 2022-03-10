// Copyright (c) Richasy. All rights reserved.

using System;
using Richasy.Bili.Models.App.Constants;
using Windows.Storage;
using Windows.System;

namespace Richasy.Bili.App.Controls
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
            => InitializeComponent();

        private async void OnOpenScreenshotFolderButtonClickAsync(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var picFolder = await KnownFolders.PicturesLibrary.CreateFolderAsync(AppConstants.ScreenshotFolderName, CreationCollisionOption.OpenIfExists);
            await Launcher.LaunchFolderAsync(picFolder).AsTask();
        }
    }
}
