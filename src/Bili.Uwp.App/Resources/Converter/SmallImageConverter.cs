// Copyright (c) Richasy. All rights reserved.

using System;
using Windows.UI.Xaml.Data;

namespace Bili.Uwp.App.Resources.Converter
{
    internal class SmallImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is string url)
            {
                return url + "@150w_150h_1c_100q.jpg";
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
    }
}
