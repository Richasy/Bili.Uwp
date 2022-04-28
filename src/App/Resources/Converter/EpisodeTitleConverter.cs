// Copyright (c) Richasy. All rights reserved.

using System;
using Bili.Models.BiliBili;
using Windows.UI.Xaml.Data;

namespace Bili.App.Resources.Converter
{
    /// <summary>
    /// 剧集标题转换器.
    /// </summary>
    public class EpisodeTitleConverter : IValueConverter
    {
        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var result = "--";

            if (value is PgcEpisodeDetail episode)
            {
                if (!string.IsNullOrEmpty(episode.LongTitle))
                {
                    result = episode.LongTitle;
                }
                else if (!string.IsNullOrEmpty(episode.Title))
                {
                    result = episode.Title;
                }
                else
                {
                    result = episode.Index.ToString();
                }
            }

            return result;
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
    }
}
