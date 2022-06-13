// Copyright (c) Richasy. All rights reserved.

using System;
using Bili.Toolkit.Interfaces;
using Splat;
using Windows.UI.Xaml.Data;

namespace Bili.App.Resources.Converter
{
    /// <summary>
    /// 固定内容转换器.
    /// </summary>
    public sealed class FixedContentConverter : IValueConverter
    {
        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var isFixed = System.Convert.ToBoolean(value);
            var resourceToolkit = Splat.Locator.Current.GetService<IResourceToolkit>();
            return isFixed
                ? resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.UnfixContent)
                : resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.FixContent);
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
    }
}
