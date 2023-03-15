// Copyright (c) Richasy. All rights reserved.

using System;
using Bili.DI.Container;
using Bili.Models.Enums.Workspace;
using Bili.Toolkit.Interfaces;
using Microsoft.UI.Xaml.Data;

namespace Bili.Workspace.Resources.Converter
{
    /// <summary>
    /// 启动位置文本转换器.
    /// </summary>
    public sealed class StartupPositionTextConverter : IValueConverter
    {
        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var result = string.Empty;
            var resToolkit = Locator.Instance.GetService<IResourceToolkit>();
            switch ((StartupPosition)value)
            {
                case StartupPosition.TopLeft:
                    result = resToolkit.GetLocaleString(Models.Enums.LanguageNames.TopLeft);
                    break;
                case StartupPosition.TopCenter:
                    result = resToolkit.GetLocaleString(Models.Enums.LanguageNames.TopCenter);
                    break;
                case StartupPosition.TopRight:
                    result = resToolkit.GetLocaleString(Models.Enums.LanguageNames.TopRight);
                    break;
                case StartupPosition.Center:
                    result = resToolkit.GetLocaleString(Models.Enums.LanguageNames.Center);
                    break;
                case StartupPosition.BottomLeft:
                    result = resToolkit.GetLocaleString(Models.Enums.LanguageNames.BottomLeft);
                    break;
                case StartupPosition.BottomCenter:
                    result = resToolkit.GetLocaleString(Models.Enums.LanguageNames.BottomCenter);
                    break;
                case StartupPosition.BottomRight:
                    result = resToolkit.GetLocaleString(Models.Enums.LanguageNames.BottomRight);
                    break;
            }

            return result;
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
    }
}
