// Copyright (c) Richasy. All rights reserved.

using System;
using Bili.DI.Container;
using Bili.Models.Enums.Workspace;
using Bili.Toolkit.Interfaces;
using Microsoft.UI.Xaml.Data;

namespace Bili.Workspace.Resources.Converter
{
    /// <summary>
    /// 播放器类型文本转换器.
    /// </summary>
    public sealed class PlayerTypeTextConverter : IValueConverter
    {
        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var result = string.Empty;
            var resToolkit = Locator.Instance.GetService<IResourceToolkit>();
            switch ((PlayerType)value)
            {
                case PlayerType.Bili:
                    result = resToolkit.GetLocaleString(Models.Enums.LanguageNames.Bili);
                    break;
                case PlayerType.Web:
                    result = resToolkit.GetLocaleString(Models.Enums.LanguageNames.Web);
                    break;
            }

            return result;
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
    }
}
