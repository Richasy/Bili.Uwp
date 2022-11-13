// Copyright (c) Richasy. All rights reserved.

using System;
using Bili.DI.Container;
using Bili.Models.Enums.App;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Core;
using Windows.Storage;

namespace Bili.Uwp.App.Controls
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
            var resourceToolkit = Locator.Instance.GetService<IResourceToolkit>();

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

                Locator.Instance.GetService<ICallerViewModel>().ShowTip(resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.CacheCleared), InfoType.Success);
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
