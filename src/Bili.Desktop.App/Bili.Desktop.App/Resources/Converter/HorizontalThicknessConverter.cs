// Copyright (c) Richasy. All rights reserved.

using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;

namespace Bili.Desktop.App.Resources.Converter
{
    internal sealed class HorizontalThicknessConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var v = (double)value;
            return new Thickness(v, 0, v, 0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
    }
}
