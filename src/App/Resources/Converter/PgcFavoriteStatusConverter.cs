// Copyright (c) Richasy. All rights reserved.

using System;
using Bili.Locator.Uwp;
using Bili.Toolkit.Interfaces;
using Windows.UI.Xaml.Data;

namespace Bili.App.Resources.Converter
{
    /// <summary>
    /// PGC收藏状态转换器.
    /// </summary>
    internal sealed class PgcFavoriteStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is int status)
            {
                var resourceToolkit = ServiceLocator.Instance.GetService<IResourceToolkit>();
                switch (status)
                {
                    case 1:
                        return resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.WantWatch);
                    case 2:
                        return resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.Watching);
                    case 3:
                        return resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.Watched);
                    default:
                        break;
                }
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
    }
}
