// Copyright (c) GodLeaveMe. All rights reserved.

using Richasy.Bili.Models.Enums;
using Windows.UI.Xaml;

namespace Richasy.Bili.App.Resources.Extension
{
    /// <summary>
    /// 导航扩展.
    /// </summary>
    public static class NavigationExtension
    {
        /// <summary>
        /// PageId的依赖附加属性.
        /// </summary>
        public static readonly DependencyProperty PageIdProperty =
            DependencyProperty.RegisterAttached("PageId", typeof(PageIds), typeof(NavigationExtension), new PropertyMetadata(PageIds.None));

        /// <summary>
        /// 获取附加的页面ID.
        /// </summary>
        /// <param name="obj">依赖对象.</param>
        /// <returns>附加的ID值.</returns>
        public static PageIds GetPageId(DependencyObject obj)
        {
            return (PageIds)obj.GetValue(PageIdProperty);
        }

        /// <summary>
        /// 设置附加的页面ID.
        /// </summary>
        /// <param name="obj">依赖对象.</param>
        /// <param name="id">需要附加的ID值.</param>
        public static void SetPageId(DependencyObject obj, PageIds id)
        {
            obj.SetValue(PageIdProperty, id);
        }
    }
}
