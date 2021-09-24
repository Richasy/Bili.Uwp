// Copyright (c) GodLeaveMe. All rights reserved.

using System;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;

namespace Richasy.Bili.App.Resources.Converter
{
    /// <summary>
    /// 剧集/系列封面转换器.
    /// </summary>
    public class SeasonCoverConverter : IValueConverter
    {
        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is string coverUrl)
            {
                var url = coverUrl + "@180w_220h_1c_100q.jpg";
                return new BitmapImage(new Uri(url));
            }

            return value;
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
    }
}
