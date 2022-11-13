// Copyright (c) Richasy. All rights reserved.

using System;
using Bili.Desktop.App.Resources.Extension;
using Windows.UI;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;

namespace Bili.Desktop.App.Resources.Converter
{
    /// <summary>
    /// 文本哈希到颜色的转换.
    /// </summary>
    public class ColorConverter : IValueConverter
    {
        /// <summary>
        /// 是否转换为笔刷.
        /// </summary>
        public bool IsBrush { get; set; }

        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var color = Colors.Transparent;
            if (value is string hexColor)
            {
                if (int.TryParse(hexColor, out var test))
                {
                    color = hexColor.ToColor();
                }
                else
                {
                    color = Microsoft.Toolkit.Uwp.Helpers.ColorHelper.ToColor(hexColor);
                }
            }

            return IsBrush ? new SolidColorBrush(color) : (object)color;
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
    }
}
