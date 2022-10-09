// Copyright (c) Richasy. All rights reserved.

using System;
using Bili.DI.Container;
using Bili.Models.Enums;
using Bili.Toolkit.Interfaces;
using Windows.UI.Xaml.Data;

namespace Bili.App.Resources.Converter
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
            var resourceToolkit = Locator.Instance.GetService<IResourceToolkit>();
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
            else if (value is ArticleSortType ast)
            {
                switch (ast)
                {
                    case ArticleSortType.Default:
                        result = resourceToolkit.GetLocaleString(LanguageNames.SortByDefault);
                        break;
                    case ArticleSortType.Newest:
                        result = resourceToolkit.GetLocaleString(LanguageNames.SortByNewest);
                        break;
                    case ArticleSortType.Read:
                        result = resourceToolkit.GetLocaleString(LanguageNames.SortByRead);
                        break;
                    case ArticleSortType.Reply:
                        result = resourceToolkit.GetLocaleString(LanguageNames.SortByReply);
                        break;
                    case ArticleSortType.Like:
                        result = resourceToolkit.GetLocaleString(LanguageNames.SortByLike);
                        break;
                    case ArticleSortType.Favorite:
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
