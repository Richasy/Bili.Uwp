// Copyright (c) Richasy. All rights reserved.

using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Bili.Uwp.App.Resources.Converter
{
    /// <summary>
    /// 数字转可见性，小于等于0则不显示.
    /// </summary>
    public class NumberToVisibilityConverter : IValueConverter
    {
        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var number = System.Convert.ToDouble(value);
            return number <= 0 ? Visibility.Collapsed : Visibility.Visible;
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
    }
}
