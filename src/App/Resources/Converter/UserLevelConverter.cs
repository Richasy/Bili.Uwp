// Copyright (c) GodLeaveMe. All rights reserved.

using System;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;

namespace Richasy.Bili.App.Resources.Converter
{
    /// <summary>
    /// 用户等级转换器，将等级转化为对应的图片.
    /// </summary>
    public class UserLevelConverter : IValueConverter
    {
        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return new BitmapImage(new Uri($"ms-appx:///Assets/Level/level_{value}.png"));
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
    }
}
