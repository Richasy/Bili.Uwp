// Copyright (c) GodLeaveMe. All rights reserved.

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Richasy.Bili.App.Controls
{
    /// <summary>
    /// 共用ImageEx.
    /// </summary>
    public sealed partial class CommonImageEx : UserControl
    {
        /// <summary>
        /// <see cref="ImageUrl"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty ImageUrlProperty =
            DependencyProperty.Register(nameof(ImageUrl), typeof(string), typeof(CommonImageEx), new PropertyMetadata(null, new PropertyChangedCallback(OnImageUrlChanged)));

        /// <summary>
        /// <see cref="Stretch"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty StretchProperty =
            DependencyProperty.Register(nameof(Stretch), typeof(Stretch), typeof(CommonImageEx), new PropertyMetadata(Stretch.UniformToFill));

        /// <summary>
        /// <see cref="RetryCount"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty RetryCountProperty =
            DependencyProperty.Register(nameof(RetryCount), typeof(int), typeof(CommonImageEx), new PropertyMetadata(2));

        /// <summary>
        /// Initializes a new instance of the <see cref="CommonImageEx"/> class.
        /// </summary>
        public CommonImageEx()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 图片地址.
        /// </summary>
        public string ImageUrl
        {
            get { return (string)GetValue(ImageUrlProperty); }
            set { SetValue(ImageUrlProperty, value); }
        }

        /// <summary>
        /// 图片拉伸.
        /// </summary>
        public Stretch Stretch
        {
            get { return (Stretch)GetValue(StretchProperty); }
            set { SetValue(StretchProperty, value); }
        }

        /// <summary>
        /// 获取数据失败后的重试次数.
        /// </summary>
        public int RetryCount
        {
            get { return (int)GetValue(RetryCountProperty); }
            set { SetValue(RetryCountProperty, value); }
        }

        private static void OnImageUrlChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as CommonImageEx;
            instance.CoverImage.Source = e.NewValue;
        }
    }
}
