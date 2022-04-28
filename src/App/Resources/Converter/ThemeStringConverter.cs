// Copyright (c) Richasy. All rights reserved.

using System;
using Bili.Locator.Uwp;
using Bili.Models.App.Constants;
using Bili.Toolkit.Interfaces;
using Windows.UI.Xaml.Data;

namespace Bili.App.Resources.Converter
{
    /// <summary>
    /// 主题文本转换器.
    /// </summary>
    public class ThemeStringConverter : IValueConverter
    {
        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var themeStr = value.ToString();
            var resourceToolkit = ServiceLocator.Instance.GetService<IResourceToolkit>();
            var result = string.Empty;
            switch (themeStr)
            {
                case AppConstants.ThemeLight:
                    result = resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.Light);
                    break;
                case AppConstants.ThemeDark:
                    result = resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.Dark);
                    break;
                default:
                    result = resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.FollowSystem);
                    break;
            }

            return result;
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
    }
}
