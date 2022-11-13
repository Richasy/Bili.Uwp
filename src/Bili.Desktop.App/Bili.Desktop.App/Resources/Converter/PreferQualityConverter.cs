// Copyright (c) Richasy. All rights reserved.

using System;
using Bili.DI.Container;
using Bili.Models.Enums.Player;
using Bili.Toolkit.Interfaces;
using Microsoft.UI.Xaml.Data;

namespace Bili.Desktop.App.Resources.Converter
{
    /// <summary>
    /// 偏好画质的可读文本转换器.
    /// </summary>
    internal sealed class PreferQualityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if(value is PreferQuality quality)
            {
                var resToolkit = Locator.Instance.GetService<IResourceToolkit>();
                return quality switch
                {
                    PreferQuality.HDFirst => resToolkit.GetLocaleString(Models.Enums.LanguageNames.HDFirst),
                    PreferQuality.HighQuality => resToolkit.GetLocaleString(Models.Enums.LanguageNames.PreferHighQuality),
                    _ => resToolkit.GetLocaleString(Models.Enums.LanguageNames.Automatic),
                };
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
    }
}
