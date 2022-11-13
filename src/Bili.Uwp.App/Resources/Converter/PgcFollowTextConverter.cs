// Copyright (c) Richasy. All rights reserved.

using System;
using Bili.DI.Container;
using Bili.Toolkit.Interfaces;
using Windows.UI.Xaml.Data;

namespace Bili.Uwp.App.Resources.Converter
{
    /// <summary>
    /// 追番/追剧文本转换.
    /// </summary>
    public class PgcFollowTextConverter : IValueConverter
    {
        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var result = string.Empty;
            if (value is bool isFollow)
            {
                var resourceToolkit = Locator.Instance.GetService<IResourceToolkit>();
                result = isFollow ?
                    resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.PgcFollowing) :
                    resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.PgcNotFollow);
            }

            return result;
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
    }
}
