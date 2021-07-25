// Copyright (c) Richasy. All rights reserved.

using System;
using Richasy.Bili.Locator.Uwp;
using Richasy.Bili.Models.Enums;
using Richasy.Bili.Toolkit.Interfaces;
using Windows.UI.Xaml.Data;

namespace Richasy.Bili.App.Resources.Converter
{
    /// <summary>
    /// 媒体传输控件控制模式转换器.
    /// </summary>
    public class MTCControlModeConverter : IValueConverter
    {
        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var result = string.Empty;
            var resourceToolkit = ServiceLocator.Instance.GetService<IResourceToolkit>();
            if (value is MTCControlMode mode)
            {
                switch (mode)
                {
                    case MTCControlMode.Automatic:
                        result = resourceToolkit.GetLocaleString(LanguageNames.Automatic);
                        break;
                    case MTCControlMode.Manual:
                        result = resourceToolkit.GetLocaleString(LanguageNames.Manual);
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
