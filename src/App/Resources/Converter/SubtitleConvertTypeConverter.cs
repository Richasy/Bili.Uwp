// Copyright (c) Richasy. All rights reserved.

using System;
using Richasy.Bili.Locator.Uwp;
using Richasy.Bili.Models.Enums.App;
using Richasy.Bili.Toolkit.Interfaces;
using Windows.UI.Xaml.Data;

namespace Richasy.Bili.App.Resources.Converter
{
    internal sealed class SubtitleConvertTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var resourceToolkit = ServiceLocator.Instance.GetService<IResourceToolkit>();
            var result = string.Empty;
            if (value is SubtitleConvertType type)
            {
                switch (type)
                {
                    case SubtitleConvertType.None:
                        result = resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.NoConvert);
                        break;
                    case SubtitleConvertType.ToSimplifiedChinese:
                        result = resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.TC2SC);
                        break;
                    case SubtitleConvertType.ToTraditionalChinese:
                        result = resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.SC2TC);
                        break;
                    default:
                        break;
                }
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
    }
}
