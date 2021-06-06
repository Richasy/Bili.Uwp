// Copyright (c) Richasy. All rights reserved.

using System;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace Richasy.Bili.App.Controls
{
    /// <summary>
    /// 横幅条目.
    /// </summary>
    public sealed class BannerItem : Control
    {
        /// <summary>
        /// <see cref="Source"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register(nameof(Source), typeof(ImageSource), typeof(BannerItem), new PropertyMetadata(null));

        /// <summary>
        /// <see cref="Uri"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty UriProperty =
            DependencyProperty.Register(nameof(Uri), typeof(string), typeof(BannerItem), new PropertyMetadata(null));

        /// <summary>
        /// <see cref="Title"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(nameof(Title), typeof(string), typeof(BannerItem), new PropertyMetadata(null));

        /// <summary>
        /// Initializes a new instance of the <see cref="BannerItem"/> class.
        /// </summary>
        public BannerItem()
        {
            this.DefaultStyleKey = typeof(BannerItem);
        }

        /// <summary>
        /// 图片源.
        /// </summary>
        public ImageSource Source
        {
            get { return (ImageSource)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        /// <summary>
        /// 导航地址.
        /// </summary>
        public string Uri
        {
            get { return (string)GetValue(UriProperty); }
            set { SetValue(UriProperty, value); }
        }

        /// <summary>
        /// 标题.
        /// </summary>
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        /// <inheritdoc/>
        protected async override void OnTapped(TappedRoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(Uri))
            {
                await Launcher.LaunchUriAsync(new Uri(Uri));
            }
        }

        /// <inheritdoc/>
        protected override void OnPointerPressed(PointerRoutedEventArgs e)
        {
            this.CapturePointer(e.Pointer);
            VisualStateManager.GoToState(this, "PressedState", true);
        }

        /// <inheritdoc/>
        protected override void OnPointerReleased(PointerRoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, "NormalState", true);
            this.ReleasePointerCapture(e.Pointer);
        }
    }
}
