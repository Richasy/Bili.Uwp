// Copyright (c) Richasy. All rights reserved.

using System;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;

namespace Bili.App.Resources.Converter
{
    internal sealed class ImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
            => value is string uri ? new BitmapImage(new Uri(uri)) : (object)null;

        public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
    }
}
