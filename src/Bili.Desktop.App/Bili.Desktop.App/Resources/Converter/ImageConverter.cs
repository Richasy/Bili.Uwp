// Copyright (c) Richasy. All rights reserved.

using System;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media.Imaging;

namespace Bili.Desktop.App.Resources.Converter
{
    internal sealed class ImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
            => value is string uri ? new BitmapImage(new Uri(uri)) : (object)null;

        public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
    }
}
