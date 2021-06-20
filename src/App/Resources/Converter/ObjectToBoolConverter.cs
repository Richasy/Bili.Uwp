// Copyright (c) Richasy. All rights reserved.

using System;
using Windows.UI.Xaml.Data;

namespace Richasy.Bili.App.Resources.Converter
{
    /// <summary>
    /// 对象到Boolean的转换器.
    /// </summary>
    /// <remarks>
    /// 当对象不为空时，返回<c>True</c>.
    /// </remarks>
    public class ObjectToBoolConverter : IValueConverter
    {
        /// <summary>
        /// 是否反转结果.
        /// </summary>
        /// <remarks>
        /// 反转后，当字符串为空时返回<c>True</c>，反之返回<c>False</c>.
        /// </remarks>
        public bool IsReverse { get; set; }

        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var result = false;
            if (value != null)
            {
                if (value is string str)
                {
                    result = !string.IsNullOrEmpty(str);
                }
                else
                {
                    result = true;
                }
            }

            if (IsReverse)
            {
                result = !result;
            }

            return result;
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
    }
}
