// Copyright (c) Richasy. All rights reserved.

using System;
using Microsoft.UI.Xaml.Data;

namespace Bili.Desktop.App.Resources.Converter
{
    internal sealed class ErrorOpacityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
            => value is bool isError
                ? isError ? 0d : 1d
                : 1d;

        public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
    }
}
