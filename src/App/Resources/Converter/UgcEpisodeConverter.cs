// Copyright (c) Richasy. All rights reserved.

using System;
using Bili.ViewModels.Uwp;
using Bilibili.App.View.V1;
using Windows.UI.Xaml.Data;

namespace Bili.App.Resources.Converter
{
    internal sealed class UgcEpisodeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is Episode ugcEpisode)
            {
                return new VideoViewModel(ugcEpisode);
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
    }
}
