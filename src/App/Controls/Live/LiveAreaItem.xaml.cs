// Copyright (c) Richasy. All rights reserved.

using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace Bili.App.Controls
{
    /// <summary>
    /// 直播分区条目.
    /// </summary>
    public sealed partial class LiveAreaItem : UserControl
    {
        /// <summary>
        /// <see cref="Cover"/> 的依赖属性.
        /// </summary>
        public static readonly DependencyProperty CoverProperty =
            DependencyProperty.Register(nameof(Cover), typeof(string), typeof(LiveAreaItem), new PropertyMetadata(default));

        /// <summary>
        /// <see cref="Title"/> 的依赖属性.
        /// </summary>
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(nameof(Title), typeof(string), typeof(LiveAreaItem), new PropertyMetadata(default));

        /// <summary>
        /// Initializes a new instance of the <see cref="LiveAreaItem"/> class.
        /// </summary>
        public LiveAreaItem() => InitializeComponent();

        /// <summary>
        /// 在条目被点击时发生.
        /// </summary>
        public event EventHandler ItemClick;

        /// <summary>
        /// 封面.
        /// </summary>
        public string Cover
        {
            get { return (string)GetValue(CoverProperty); }
            set { SetValue(CoverProperty, value); }
        }

        /// <summary>
        /// 标题.
        /// </summary>
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        private void OnAreaClick(object sender, RoutedEventArgs e)
        {
            var animationService = ConnectedAnimationService.GetForCurrentView();
            animationService.PrepareToAnimate("LiveAreaAnimate", ContentContainer);
            ItemClick?.Invoke(this, EventArgs.Empty);
        }
    }
}
