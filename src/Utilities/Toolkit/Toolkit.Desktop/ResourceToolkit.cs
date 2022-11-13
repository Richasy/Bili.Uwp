// Copyright (c) Richasy. All rights reserved.

using Bili.Models.App.Constants;
using Bili.Models.Enums;
using Bili.Toolkit.Interfaces;
using Microsoft.UI.Xaml;
using Windows.ApplicationModel.Resources.Core;
using Windows.UI.ViewManagement;

namespace Bili.Toolkit.Desktop
{
    /// <summary>
    /// App resource toolkit.
    /// </summary>
    public class ResourceToolkit : IResourceToolkit
    {
        private readonly ISettingsToolkit _settingsToolkit;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceToolkit"/> class.
        /// </summary>
        /// <param name="settingsToolkit">Settings toolkit.</param>
        public ResourceToolkit(ISettingsToolkit settingsToolkit)
            => _settingsToolkit = settingsToolkit;

        /// <inheritdoc/>
        public string GetLocaleString(LanguageNames languageName)
            => GetLocaleString(languageName.ToString());

        /// <inheritdoc/>
        public T GetResource<T>(string resourceName)
        {
            var isHC = new AccessibilitySettings().HighContrast;
            ResourceDictionary themeDict;
            var theme = _settingsToolkit.ReadLocalSetting(SettingNames.AppTheme, AppConstants.ThemeDefault);
            var themeStr = theme == AppConstants.ThemeDefault
                ? Application.Current.RequestedTheme.ToString()
                : theme.ToString();
            themeDict = isHC
                ? (ResourceDictionary)Application.Current.Resources.ThemeDictionaries["HighContrast"]
                : (ResourceDictionary)Application.Current.Resources.ThemeDictionaries[themeStr];

            return (T)themeDict[resourceName];
        }

        private static string GetLocaleString(string languageName)
            => ResourceManager.Current.MainResourceMap[$"Resources/{languageName}"].Candidates[0].ValueAsString;
    }
}
