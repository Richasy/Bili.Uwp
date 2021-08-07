// Copyright (c) Richasy. All rights reserved.

using System;
using Richasy.Bili.ViewModels.Uwp;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Richasy.Bili.App.Controls
{
    /// <summary>
    /// PGC条目.
    /// </summary>
    public sealed partial class PgcItem : UserControl
    {
        /// <summary>
        /// <see cref="ViewModel"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel), typeof(SeasonViewModel), typeof(PgcItem), new PropertyMetadata(null));

        /// <summary>
        /// <see cref="CardStyle"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty CardStyleProperty =
            DependencyProperty.Register(nameof(CardStyle), typeof(Style), typeof(PgcItem), new PropertyMetadata(null));

        /// <summary>
        /// <see cref="CoverWidth"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty CoverWidthProperty =
            DependencyProperty.Register(nameof(CoverWidth), typeof(double), typeof(PgcItem), new PropertyMetadata(120d));

        /// <summary>
        /// <see cref="CoverHeight"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty CoverHeightProperty =
            DependencyProperty.Register(nameof(CoverHeight), typeof(double), typeof(PgcItem), new PropertyMetadata(160d));

        /// <summary>
        /// <see cref="TitleRowHeight"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty TitleRowHeightProperty =
            DependencyProperty.Register(nameof(TitleRowHeight), typeof(double), typeof(PgcItem), new PropertyMetadata(48d));

        /// <summary>
        /// <see cref="TagVisibility"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty TagVisibilityProperty =
            DependencyProperty.Register(nameof(TagVisibility), typeof(Visibility), typeof(PgcItem), new PropertyMetadata(Visibility.Visible));

        /// <summary>
        /// Initializes a new instance of the <see cref="PgcItem"/> class.
        /// </summary>
        public PgcItem()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 视图模型.
        /// </summary>
        public SeasonViewModel ViewModel
        {
            get { return (SeasonViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        /// <summary>
        /// 封面宽度.
        /// </summary>
        public double CoverWidth
        {
            get { return (double)GetValue(CoverWidthProperty); }
            set { SetValue(CoverWidthProperty, value); }
        }

        /// <summary>
        /// 封面高度.
        /// </summary>
        public double CoverHeight
        {
            get { return (double)GetValue(CoverHeightProperty); }
            set { SetValue(CoverHeightProperty, value); }
        }

        /// <summary>
        /// 标题行高度.
        /// </summary>
        public double TitleRowHeight
        {
            get { return (double)GetValue(TitleRowHeightProperty); }
            set { SetValue(TitleRowHeightProperty, value); }
        }

        /// <summary>
        /// 标签的可见性.
        /// </summary>
        public Visibility TagVisibility
        {
            get { return (Visibility)GetValue(TagVisibilityProperty); }
            set { SetValue(TagVisibilityProperty, value); }
        }

        /// <summary>
        /// 卡片样式.
        /// </summary>
        public Style CardStyle
        {
            get { return (Style)GetValue(CardStyleProperty); }
            set { SetValue(CardStyleProperty, value); }
        }

        private void OnRootCardClick(object sender, RoutedEventArgs e)
        {
            AppViewModel.Instance.OpenPlayer(ViewModel);
        }
    }
}
