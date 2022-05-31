// Copyright (c) Richasy. All rights reserved.

using System;
using Bili.Models.Enums.Community;
using Bili.Toolkit.Interfaces;
using Splat;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Bili.App.Resources.Converter
{
    /// <summary>
    /// 关系按钮样式转换器.
    /// </summary>
    public sealed class RelationButtonStyleConverter : IValueConverter
    {
        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is UserRelationStatus status)
            {
                var resourceToolkit = Splat.Locator.Current.GetService<IResourceToolkit>();
                return status == UserRelationStatus.Unfollow || status == UserRelationStatus.BeFollowed
                    ? resourceToolkit.GetResource<Style>("AccentButtonStyle")
                    : (object)resourceToolkit.GetResource<Style>("DefaultButtonStyle");
            }

            return null;
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
    }
}
