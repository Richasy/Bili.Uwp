// Copyright (c) Richasy. All rights reserved.

using System;
using Bili.DI.Container;
using Bili.Models.Enums.Workspace;
using Bili.Toolkit.Interfaces;
using Microsoft.UI.Xaml.Data;

namespace Bili.Workspace.Resources.Converter
{
    /// <summary>
    /// 启动类型文本转换器.
    /// </summary>
    public sealed class LaunchTypeTextConverter : IValueConverter
    {
        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var result = string.Empty;
            var resToolkit = Locator.Instance.GetService<IResourceToolkit>();
            switch ((LaunchType)value)
            {
                case LaunchType.Bili:
                    result = resToolkit.GetLocaleString(Models.Enums.LanguageNames.Bili);
                    break;
                case LaunchType.Web:
                    result = resToolkit.GetLocaleString(Models.Enums.LanguageNames.Web);
                    break;
            }

            return result;
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
    }
}
