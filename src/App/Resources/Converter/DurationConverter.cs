// Copyright (c) Richasy. All rights reserved.

using System;
using Bili.Toolkit.Interfaces;
using Splat;
using Windows.UI.Xaml.Data;

namespace Bili.App.Resources.Converter
{
    /// <summary>
    /// 时长转换为可读文本.
    /// </summary>
    public class DurationConverter : IValueConverter
    {
        /// <summary>
        /// 是否为毫秒记录.
        /// </summary>
        public bool IsMilliseconds { get; set; }

        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var numToolkit = Locator.Current.GetService<INumberToolkit>();
            if (value is int time)
            {
                if (IsMilliseconds)
                {
                    return numToolkit.GetDurationText(TimeSpan.FromMilliseconds(time));
                }
                else
                {
                    return numToolkit.GetDurationText(TimeSpan.FromSeconds(time));
                }
            }

            return value.ToString();
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
    }
}
