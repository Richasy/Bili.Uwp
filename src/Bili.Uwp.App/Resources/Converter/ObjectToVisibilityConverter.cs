// Copyright (c) Richasy. All rights reserved.

using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Bili.Uwp.App.Resources.Converter
{
    /// <summary>
    /// 对象到可见性的转换器. 当对象为空时，返回Collapsed.
    /// </summary>
    public class ObjectToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// 是否反转结果.
        /// </summary>
        public bool IsReverse { get; set; }

        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var isShow = true;
            if (value == null)
            {
                isShow = false;
            }
            else if (value is string str)
            {
                isShow = !string.IsNullOrEmpty(str);
            }

            if (IsReverse)
            {
                isShow = !isShow;
            }

            return isShow ? Visibility.Visible : Visibility.Collapsed;
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
    }
}
