// Copyright (c) Richasy. All rights reserved.

using System;
using Bili.DI.Container;
using Bili.Models.App.Constants;
using Bili.Toolkit.Interfaces;
using Microsoft.UI.Xaml.Data;

namespace Bili.Desktop.App.Resources.Converter
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
            var resourceToolkit = Locator.Instance.GetService<IResourceToolkit>();
            var result = themeStr switch
            {
                AppConstants.ThemeLight => resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.Light),
                AppConstants.ThemeDark => resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.Dark),
                _ => resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.FollowSystem),
            };
            return result;
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
    }
}
