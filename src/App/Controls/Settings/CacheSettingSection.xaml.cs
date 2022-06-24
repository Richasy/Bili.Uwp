// Copyright (c) Richasy. All rights reserved.

using System;
using Bili.Models.Enums.App;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Uwp.Core;
using Splat;
using Windows.Storage;

namespace Bili.App.Controls
{
    /// <summary>
    /// 缓存设置.
    /// </summary>
    public sealed partial class CacheSettingSection : SettingSectionControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CacheSettingSection"/> class.
        /// </summary>
        public CacheSettingSection()
            : base()
            => InitializeComponent();

        private async void OnClearButtonClickAsync(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var cacheFolder = ApplicationData.Current.LocalCacheFolder;
            LoadingRing.Visibility = Windows.UI.Xaml.Visibility.Visible;
            ClearButton.IsEnabled = false;
            var resourceToolkit = Locator.Current.GetService<IResourceToolkit>();

            try
            {
                var children = await cacheFolder.GetItemsAsync();
                foreach (var child in children)
                {
                    if (child is StorageFile file)
                    {
                        await file.DeleteAsync();
                    }
                    else if (child is StorageFolder folder)
                    {
                        await folder.DeleteAsync();
                    }
                }

                Splat.Locator.Current.GetService<AppViewModel>().ShowTip(resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.CacheCleared), InfoType.Success);
            }
            catch
            {
            }
            finally
            {
                LoadingRing.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                ClearButton.IsEnabled = true;
            }
        }
    }
}
