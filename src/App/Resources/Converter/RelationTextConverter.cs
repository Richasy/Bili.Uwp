// Copyright (c) Richasy. All rights reserved.

using System;
using Bili.DI.Container;
using Bili.Models.Enums.Community;
using Bili.Toolkit.Interfaces;
using Windows.UI.Xaml.Data;

namespace Bili.App.Resources.Converter
{
    /// <summary>
    /// 用户关系文本转换.
    /// </summary>
    public sealed class RelationTextConverter : IValueConverter
    {
        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is UserRelationStatus status)
            {
                var resourceToolkit = Locator.Instance.GetService<IResourceToolkit>();
                return status switch
                {
                    UserRelationStatus.Unfollow => resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.Follow),
                    UserRelationStatus.Following => resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.Followed),
                    UserRelationStatus.BeFollowed => resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.Follow),
                    UserRelationStatus.Friends => resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.FollowEachOther),
                    _ => "--",
                };
            }

            return "--";
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
            => throw new NotImplementedException();
    }
}
