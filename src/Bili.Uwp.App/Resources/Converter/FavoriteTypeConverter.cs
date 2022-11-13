// Copyright (c) Richasy. All rights reserved.

using System;
using Bili.DI.Container;
using Bili.Models.Enums;
using Bili.Models.Enums.App;
using Bili.Toolkit.Interfaces;
using Windows.UI.Xaml.Data;

namespace Bili.Uwp.App.Resources.Converter
{
    /// <summary>
    /// 收藏夹类型转换器.
    /// </summary>
    public class FavoriteTypeConverter : IValueConverter
    {
        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var type = (FavoriteType)value;
            var result = string.Empty;
            var resourceToolkit = Locator.Instance.GetService<IResourceToolkit>();
            switch (type)
            {
                case FavoriteType.Video:
                    result = resourceToolkit.GetLocaleString(LanguageNames.Videos);
                    break;
                case FavoriteType.Anime:
                    result = resourceToolkit.GetLocaleString(LanguageNames.Anime);
                    break;
                case FavoriteType.Cinema:
                    result = resourceToolkit.GetLocaleString(LanguageNames.Cinema);
                    break;
                case FavoriteType.Article:
                    result = resourceToolkit.GetLocaleString(LanguageNames.SpecialColumn);
                    break;
                default:
                    break;
            }

            return result;
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
    }
}
