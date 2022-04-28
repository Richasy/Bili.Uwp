// Copyright (c) Richasy. All rights reserved.

using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Bili.App.Resources.Converter
{
    /// <summary>
    /// <see cref="bool"/>到<see cref="Visibility"/>的转换.
    /// </summary>
    public class BoolToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// 是否反转值.
        /// </summary>
        public bool IsReverse { get; set; }

        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var vis = Visibility.Visible;
            if (value is bool v)
            {
                if (IsReverse)
                {
                    vis = v ? Visibility.Collapsed : Visibility.Visible;
                }
                else
                {
                    vis = v ? Visibility.Visible : Visibility.Collapsed;
                }
            }

            return vis;
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
    }
}
