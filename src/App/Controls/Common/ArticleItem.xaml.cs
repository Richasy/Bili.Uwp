// Copyright (c) Richasy. All rights reserved.

using Bili.ViewModels.Uwp;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Bili.App.Controls
{
    /// <summary>
    /// 文章条目.
    /// </summary>
    public sealed partial class ArticleItem : UserControl, IRepeaterItem, IDynamicLayoutItem
    {
        /// <summary>
        /// <see cref="ViewModel"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel), typeof(ArticleViewModel), typeof(ArticleItem), new PropertyMetadata(null));

        /// <summary>
        /// <see cref="Orientation"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register(nameof(Orientation), typeof(Orientation), typeof(ArticleItem), new PropertyMetadata(default(Orientation), new PropertyChangedCallback(OnOrientationChanged)));

        /// <summary>
        /// <see cref="CardPanelStyle"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty CardPanelStyleProperty =
            DependencyProperty.Register(nameof(CardPanelStyle), typeof(Style), typeof(ArticleItem), new PropertyMetadata(null));

        /// <summary>
        /// <see cref="DescriptionVisibility"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty DescriptionVisibilityProperty =
            DependencyProperty.Register(nameof(DescriptionVisibility), typeof(Visibility), typeof(ArticleItem), new PropertyMetadata(Visibility.Visible));

        /// <summary>
        /// Initializes a new instance of the <see cref="ArticleItem"/> class.
        /// </summary>
        public ArticleItem()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        /// <summary>
        /// 视图模型.
        /// </summary>
        public ArticleViewModel ViewModel
        {
            get { return (ArticleViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        /// <summary>
        /// 文章的布局方式.
        /// </summary>
        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        /// <summary>
        /// 内部容器的样式.
        /// </summary>
        public Style CardPanelStyle
        {
            get { return (Style)GetValue(CardPanelStyleProperty); }
            set { SetValue(CardPanelStyleProperty, value); }
        }

        /// <summary>
        /// 描述的可见性.
        /// </summary>
        public Visibility DescriptionVisibility
        {
            get { return (Visibility)GetValue(DescriptionVisibilityProperty); }
            set { SetValue(DescriptionVisibilityProperty, value); }
        }

        /// <inheritdoc/>
        public Size GetHolderSize()
        {
            if (Orientation == Orientation.Horizontal)
            {
                return new Size(double.PositiveInfinity, 180);
            }
            else
            {
                return new Size(210, 248);
            }
        }

        private static void OnOrientationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as ArticleItem;
            instance.CheckOrientation();
        }

        private void OnContainerClickAsync(object sender, RoutedEventArgs e)
        {
        }

        private void OnLoaded(object sender, RoutedEventArgs e) => CheckOrientation();

        private void CheckOrientation()
        {
            switch (Orientation)
            {
                case Orientation.Vertical:
                    VisualStateManager.GoToState(this, nameof(VerticalState), false);
                    break;
                case Orientation.Horizontal:
                    VisualStateManager.GoToState(this, nameof(HorizontalState), false);
                    break;
                default:
                    break;
            }
        }
    }
}
