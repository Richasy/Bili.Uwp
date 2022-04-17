// Copyright (c) Richasy. All rights reserved.

using System;
using Bilibili.App.View.V1;
using Richasy.Bili.ViewModels.Uwp;
using Windows.UI.Xaml.Data;

namespace Richasy.Bili.App.Resources.Converter
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
