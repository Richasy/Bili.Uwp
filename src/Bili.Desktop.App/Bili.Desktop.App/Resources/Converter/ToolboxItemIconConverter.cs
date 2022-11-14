// Copyright (c) Richasy. All rights reserved.

using System;
using Bili.Models.Enums;
using Microsoft.UI.Xaml.Data;

namespace Bili.Desktop.App.Resources.Converter
{
    internal class ToolboxItemIconConverter : IValueConverter
    {
        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var symbol = FluentIcons.Common.Symbol.ErrorCircle;
            if (value is ToolboxItemType type)
            {
                switch (type)
                {
                    case ToolboxItemType.AvBvConverter:
                        symbol = FluentIcons.Common.Symbol.TextClearFormatting;
                        break;
                    case ToolboxItemType.CoverDownloader:
                        symbol = FluentIcons.Common.Symbol.ImageSearch;
                        break;
                    default:
                        break;
                }
            }

            return symbol;
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
            => throw new NotImplementedException();
    }
}
