// Copyright (c) Richasy. All rights reserved.

using System;
using Richasy.Bili.Locator.Uwp;
using Richasy.Bili.Models.Enums;
using Richasy.Bili.Toolkit.Interfaces;
using Windows.UI.Xaml.Data;

namespace Richasy.Bili.App.Resources.Converter
{
    /// <summary>
    /// 视频排序方式可读文本转换器.
    /// </summary>
    public class SortTypeTextConverter : IValueConverter
    {
        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var result = string.Empty;
            var resourceToolkit = ServiceLocator.Instance.GetService<IResourceToolkit>();
            if (value is VideoSortType vst)
            {
                switch (vst)
                {
                    case VideoSortType.Default:
                        result = resourceToolkit.GetLocaleString(LanguageNames.SortByDefault);
                        break;
                    case VideoSortType.Newest:
                        result = resourceToolkit.GetLocaleString(LanguageNames.SortByNewest);
                        break;
                    case VideoSortType.Play:
                        result = resourceToolkit.GetLocaleString(LanguageNames.SortByPlay);
                        break;
                    case VideoSortType.Reply:
                        result = resourceToolkit.GetLocaleString(LanguageNames.SortByReply);
                        break;
                    case VideoSortType.Danmaku:
                        result = resourceToolkit.GetLocaleString(LanguageNames.SortByDanmaku);
                        break;
                    case VideoSortType.Favorite:
                        result = resourceToolkit.GetLocaleString(LanguageNames.SortByFavorite);
                        break;
                    default:
                        break;
                }
            }

            return result;
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
    }
}
