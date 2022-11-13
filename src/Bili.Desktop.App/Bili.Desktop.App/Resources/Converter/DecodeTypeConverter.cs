// Copyright (c) Richasy. All rights reserved.

using System;
using Bili.DI.Container;
using Bili.Models.Enums;
using Bili.Models.Enums.App;
using Bili.Toolkit.Interfaces;
using Microsoft.UI.Xaml.Data;

namespace Bili.Desktop.App.Resources.Converter
{
    internal sealed class DecodeTypeConverter : IValueConverter
    {
        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var result = string.Empty;
            var resourceToolkit = Locator.Instance.GetService<IResourceToolkit>();
            if (value is DecodeType decode)
            {
                switch (decode)
                {
                    case DecodeType.Automatic:
                        result = resourceToolkit.GetLocaleString(LanguageNames.Automatic);
                        break;
                    case DecodeType.HardwareDecode:
                        result = resourceToolkit.GetLocaleString(LanguageNames.HardwareDecode);
                        break;
                    case DecodeType.SoftwareDecode:
                        result = resourceToolkit.GetLocaleString(LanguageNames.SoftwareDecode);
                        break;
                    default:
                        break;
                }
            }

            return result;
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
    }
}
