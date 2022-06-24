// Copyright (c) Richasy. All rights reserved.

using System;
using Bili.Models.App.Constants;
using Bili.Models.Enums.App;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Uwp.Core;
using Splat;
using Windows.Storage;
using Windows.System;

namespace Bili.App.Controls
{
    /// <summary>
    /// 日志设置区块.
    /// </summary>
    public sealed partial class LoggerSettingSection : SettingSectionControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LoggerSettingSection"/> class.
        /// </summary>
        public LoggerSettingSection()
            : base()
            => InitializeComponent();

        private async void OnOpenLoggerFolderButtonClickAsync(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var folder = await ApplicationData.Current.LocalFolder.CreateFolderAsync(ControllerConstants.Names.LoggerFolder, CreationCollisionOption.OpenIfExists).AsTask();
            await Launcher.LaunchFolderAsync(folder);
        }

        private async void OnCleanLoggerButtonClickAsync(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var folder = await ApplicationData.Current.LocalFolder.CreateFolderAsync(ControllerConstants.Names.LoggerFolder, CreationCollisionOption.OpenIfExists).AsTask();
            var resourceToolkit = Locator.Current.GetService<IResourceToolkit>();
            try
            {
                await folder.DeleteAsync(StorageDeleteOption.PermanentDelete).AsTask();
                await ApplicationData.Current.LocalFolder.CreateFolderAsync(ControllerConstants.Names.LoggerFolder, CreationCollisionOption.OpenIfExists).AsTask();
            }
            catch (Exception)
            {
            }
            finally
            {
                Splat.Locator.Current.GetService<AppViewModel>().ShowTip(resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.LogEmptied), InfoType.Success);
            }
        }
    }
}
