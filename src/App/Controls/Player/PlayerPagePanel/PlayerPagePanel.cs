// Copyright (c) Richasy. All rights reserved.

using System;
using Bili.Models.App.Other;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;

namespace Bili.App.Controls.Player
{
    /// <summary>
    /// 播放页面面板.
    /// </summary>
    [ContentProperty(Name = "Player")]
    public sealed class PlayerPagePanel : Control
    {
        /// <summary>
        /// <see cref="Player"/> 的依赖属性.
        /// </summary>
        public static readonly DependencyProperty PlayerProperty =
            DependencyProperty.Register(nameof(Player), typeof(object), typeof(PlayerPagePanel), new PropertyMetadata(default));

        /// <summary>
        /// <see cref="Dashboard"/> 的依赖属性.
        /// </summary>
        public static readonly DependencyProperty DashboardProperty =
            DependencyProperty.Register(nameof(Dashboard), typeof(object), typeof(PlayerPagePanel), new PropertyMetadata(default));

        /// <summary>
        /// <see cref="SectionHeaderItemsSource"/> 的依赖属性.
        /// </summary>
        public static readonly DependencyProperty SectionHeaderItemsSourceProperty =
            DependencyProperty.Register(nameof(SectionHeaderItemsSource), typeof(object), typeof(PlayerPagePanel), new PropertyMetadata(default));

        /// <summary>
        /// <see cref="SectionContent"/> 的依赖属性.
        /// </summary>
        public static readonly DependencyProperty SectionContentProperty =
            DependencyProperty.Register(nameof(SectionContent), typeof(object), typeof(PlayerPagePanel), new PropertyMetadata(default));

        /// <summary>
        /// <see cref="SectionHeaderSelectedItem"/> 的依赖属性.
        /// </summary>
        public static readonly DependencyProperty SectionHeaderSelectedItemProperty =
            DependencyProperty.Register(nameof(SectionHeaderSelectedItem), typeof(object), typeof(PlayerPagePanel), new PropertyMetadata(default));

        /// <summary>
        /// <see cref="Descriptor"/> 的依赖属性.
        /// </summary>
        public static readonly DependencyProperty DescriptorProperty =
            DependencyProperty.Register(nameof(Descriptor), typeof(object), typeof(PlayerPagePanel), new PropertyMetadata(default));

        private SplitView _splitView;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerPagePanel"/> class.
        /// </summary>
        public PlayerPagePanel()
            => DefaultStyleKey = typeof(PlayerPagePanel);

        /// <summary>
        /// 区块标头被点击.
        /// </summary>
        public event EventHandler<PlayerSectionHeader> SectionHeaderItemInvoked;

        /// <summary>
        /// 播放器组件.
        /// </summary>
        public object Player
        {
            get { return (object)GetValue(PlayerProperty); }
            set { SetValue(PlayerProperty, value); }
        }

        /// <summary>
        /// 信息面板组件.
        /// </summary>
        public object Dashboard
        {
            get { return (object)GetValue(DashboardProperty); }
            set { SetValue(DashboardProperty, value); }
        }

        /// <summary>
        /// 描述组件.
        /// </summary>
        public object Descriptor
        {
            get { return (object)GetValue(DescriptorProperty); }
            set { SetValue(DescriptorProperty, value); }
        }

        /// <summary>
        /// 侧面板的头部数据源.
        /// </summary>
        public object SectionHeaderItemsSource
        {
            get { return (object)GetValue(SectionHeaderItemsSourceProperty); }
            set { SetValue(SectionHeaderItemsSourceProperty, value); }
        }

        /// <summary>
        /// 侧面板头部选中条目.
        /// </summary>
        public object SectionHeaderSelectedItem
        {
            get { return (object)GetValue(SectionHeaderSelectedItemProperty); }
            set { SetValue(SectionHeaderSelectedItemProperty, value); }
        }

        /// <summary>
        /// 侧面板的内容.
        /// </summary>
        public object SectionContent
        {
            get { return (object)GetValue(SectionContentProperty); }
            set { SetValue(SectionContentProperty, value); }
        }

        /// <inheritdoc/>
        protected override void OnApplyTemplate()
        {
            _splitView = GetTemplateChild("RootView") as SplitView;
            var expandButton = GetTemplateChild("ExpandButton") as Button;
            var sectionView = GetTemplateChild("SectionNavigationView") as Microsoft.UI.Xaml.Controls.NavigationView;

            expandButton.Click += OnExpandButtonClick;
            sectionView.ItemInvoked += OnSectionViewItemInvoked;
        }

        private void OnExpandButtonClick(object sender, RoutedEventArgs e)
            => _splitView.IsPaneOpen = true;

        private void OnSectionViewItemInvoked(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewItemInvokedEventArgs args)
        {
            var item = args.InvokedItem as PlayerSectionHeader;
            SectionHeaderItemInvoked?.Invoke(this, item);
        }
    }
}
