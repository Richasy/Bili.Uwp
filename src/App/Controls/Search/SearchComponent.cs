// Copyright (c) Richasy. All rights reserved.

using Bili.ViewModels.Uwp.Core;
using Splat;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Bili.App.Controls
{
    /// <summary>
    /// 搜索组件.
    /// </summary>
    public class SearchComponent : UserControl
    {
        /// <summary>
        /// <see cref="ItemsSource"/> 的依赖属性.
        /// </summary>
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register(nameof(ItemsSource), typeof(object), typeof(SearchComponent), new PropertyMetadata(default));

        /// <summary>
        /// 核心视图模型.
        /// </summary>
        public AppViewModel CoreViewModel { get; } = Splat.Locator.Current.GetService<AppViewModel>();

        /// <summary>
        /// 数据源.
        /// </summary>
        public object ItemsSource
        {
            get { return (object)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }
    }
}
