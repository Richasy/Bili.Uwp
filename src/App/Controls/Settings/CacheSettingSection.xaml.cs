// Copyright (c) Richasy. All rights reserved.

using System;
using Bili.Locator.Uwp;
using Bili.Models.Enums.App;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Uwp;
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
        public CacheSettingSection() => InitializeComponent();

        private async void OnClearButtonClickAsync(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var cacheFolder = ApplicationData.Current.LocalCacheFolder;
            LoadingRing.Visibility = Windows.UI.Xaml.Visibility.Visible;
            ClearButton.IsEnabled = false;
            var resourceToolkit = ServiceLocator.Instance.GetService<IResourceToolkit>();

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

                AppViewModel.Instance.ShowTip(resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.CacheCleared), InfoType.Success);
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
