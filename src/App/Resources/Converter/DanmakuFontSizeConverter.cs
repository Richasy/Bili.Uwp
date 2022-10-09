// Copyright (c) Richasy. All rights reserved.

using System;
using Bili.DI.Container;
using Bili.Models.Enums;
using Bili.Toolkit.Interfaces;
using Windows.UI.Xaml.Data;

namespace Bili.App.Resources.Converter
{
    /// <summary>
    /// 弹幕大小转换器.
    /// </summary>
    internal sealed class DanmakuFontSizeConverter : IValueConverter
    {
        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var size = (double)value;
            var name = LanguageNames.Standard;
            switch (size)
            {
                case 0.5:
                    name = LanguageNames.Minimum;
                    break;
                case 1:
                    name = LanguageNames.Small;
                    break;
                case 1.5:
                    name = LanguageNames.Standard;
                    break;
                case 2:
                    name = LanguageNames.Large;
                    break;
                case 2.5:
                    name = LanguageNames.Maximum;
                    break;
                default:
                    break;
            }

            return Locator.Instance.GetService<IResourceToolkit>().GetLocaleString(name);
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
    }
}
