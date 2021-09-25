// Copyright (c) Richasy. All rights reserved.

using System;
using Richasy.Bili.Locator.Uwp;
using Richasy.Bili.Models.Enums;
using Richasy.Bili.Toolkit.Interfaces;
using Windows.UI.Xaml.Data;

namespace Richasy.Bili.App.Resources.Converter
{
    /// <summary>
    /// 双击行为的文本转换器.
    /// </summary>
    public class DoubleClickTextConverter : IValueConverter
    {
        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var result = string.Empty;
            var resourceToolkit = ServiceLocator.Instance.GetService<IResourceToolkit>();
            if (value is DoubleClickBehavior behavior)
            {
                switch (behavior)
                {
                    case DoubleClickBehavior.PlayPause:
                        result = resourceToolkit.GetLocaleString(LanguageNames.PlayPause);
                        break;
                    case DoubleClickBehavior.FullScreen:
                        result = resourceToolkit.GetLocaleString(LanguageNames.FullScreenMode);
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
