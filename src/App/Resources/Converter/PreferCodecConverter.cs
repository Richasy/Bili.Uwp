// Copyright (c) GodLeaveMe. All rights reserved.

using System;
using Richasy.Bili.Locator.Uwp;
using Richasy.Bili.Models.Enums;
using Richasy.Bili.Toolkit.Interfaces;
using Windows.UI.Xaml.Data;

namespace Richasy.Bili.App.Resources.Converter
{
    /// <summary>
    /// 偏好解码模式到可读文本转换器.
    /// </summary>
    public class PreferCodecConverter : IValueConverter
    {
        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var result = string.Empty;
            var resourceToolkit = ServiceLocator.Instance.GetService<IResourceToolkit>();
            if (value is PreferCodec codec)
            {
                switch (codec)
                {
                    case PreferCodec.H265:
                        result = resourceToolkit.GetLocaleString(LanguageNames.H265);
                        break;
                    case PreferCodec.H264:
                        result = resourceToolkit.GetLocaleString(LanguageNames.H264);
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
