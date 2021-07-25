// Copyright (c) Richasy. All rights reserved.

using System;
using Richasy.Bili.Locator.Uwp;
using Richasy.Bili.Models.Enums;
using Richasy.Bili.Toolkit.Interfaces;
using Windows.UI.Xaml.Data;

namespace Richasy.Bili.App.Resources.Converter
{
    /// <summary>
    /// 播放器显示模式到可读文本的转换器.
    /// </summary>
    public class PlayerDisplayModeConverter : IValueConverter
    {
        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var result = string.Empty;
            var resourceToolkit = ServiceLocator.Instance.GetService<IResourceToolkit>();
            if (value is PlayerDisplayMode mode)
            {
                switch (mode)
                {
                    case PlayerDisplayMode.Default:
                        result = resourceToolkit.GetLocaleString(LanguageNames.Default);
                        break;
                    case PlayerDisplayMode.Cinema:
                        result = resourceToolkit.GetLocaleString(LanguageNames.CinemaMode);
                        break;
                    case PlayerDisplayMode.FullWindow:
                        result = resourceToolkit.GetLocaleString(LanguageNames.FullWindowMode);
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
