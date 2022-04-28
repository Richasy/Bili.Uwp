// Copyright (c) Richasy. All rights reserved.

using System;
using Bili.Locator.Uwp;
using Bili.Models.Enums.App;
using Bili.Toolkit.Interfaces;
using Windows.UI.Xaml.Data;

namespace Bili.App.Resources.Converter
{
    /// <summary>
    /// 弹幕位置文本转换.
    /// </summary>
    public class DanmakuLocationConverter : IValueConverter
    {
        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var result = string.Empty;
            var resToolkit = ServiceLocator.Instance.GetService<IResourceToolkit>();
            switch ((DanmakuLocation)value)
            {
                case DanmakuLocation.Scroll:
                    result = resToolkit.GetLocaleString(Models.Enums.LanguageNames.ScrollDanmaku);
                    break;
                case DanmakuLocation.Top:
                    result = resToolkit.GetLocaleString(Models.Enums.LanguageNames.TopDanmaku);
                    break;
                case DanmakuLocation.Bottom:
                    result = resToolkit.GetLocaleString(Models.Enums.LanguageNames.BottomDanmaku);
                    break;
            }

            return result;
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
    }
}
