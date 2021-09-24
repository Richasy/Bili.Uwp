// Copyright (c) GodLeaveMe. All rights reserved.

using System;
using Richasy.Bili.Locator.Uwp;
using Richasy.Bili.Models.Enums.App;
using Richasy.Bili.Toolkit.Interfaces;
using Windows.UI.Xaml.Data;

namespace Richasy.Bili.App.Resources.Converter
{
    /// <summary>
    /// 弹幕样式转换器.
    /// </summary>
    public class DanmakuStyleConverter : IValueConverter
    {
        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var result = string.Empty;
            var resToolkit = ServiceLocator.Instance.GetService<IResourceToolkit>();
            switch ((DanmakuStyle)value)
            {
                case DanmakuStyle.Stroke:
                    result = resToolkit.GetLocaleString(Models.Enums.LanguageNames.Stroke);
                    break;
                case DanmakuStyle.NoStroke:
                    result = resToolkit.GetLocaleString(Models.Enums.LanguageNames.NoStroke);
                    break;
                case DanmakuStyle.Shadow:
                    result = resToolkit.GetLocaleString(Models.Enums.LanguageNames.Shadow);
                    break;
            }

            return result;
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
    }
}
