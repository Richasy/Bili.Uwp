// Copyright (c) Richasy. All rights reserved.

using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Uwp.Video;
using Splat;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Bili.App.Controls.Videos
{
    /// <summary>
    /// 用来显示视频条目的 UI 单元，可以通过样式展现不同的布局.
    /// </summary>
    public sealed partial class VideoItem : ReactiveControl<VideoItemViewModel>, IRepeaterItem, IOrientationControl
    {
        /// <summary>
        /// <see cref="IsDynamic"/> 的依赖属性.
        /// </summary>
        public static readonly DependencyProperty IsDynamicProperty =
            DependencyProperty.Register(nameof(IsDynamic), typeof(bool), typeof(VideoItem), new PropertyMetadata(false));

        /// <summary>
        /// Initializes a new instance of the <see cref="VideoItem"/> class.
        /// </summary>
        public VideoItem()
            => DefaultStyleKey = typeof(VideoItem);

        /// <summary>
        /// 是否是动态视频.
        /// </summary>
        public bool IsDynamic
        {
            get { return (bool)GetValue(IsDynamicProperty); }
            set { SetValue(IsDynamicProperty, value); }
        }

        /// <inheritdoc/>
        public Size GetHolderSize() => new (210, 248);

        /// <inheritdoc/>
        public void ChangeLayout(Orientation orientation)
        {
            var resourceToolkit = Splat.Locator.Current.GetService<IResourceToolkit>();
            Style = orientation == Orientation.Horizontal
                ? IsDynamic ? resourceToolkit.GetResource<Style>("HorizontalDynamicVideoItemStyle") : resourceToolkit.GetResource<Style>("HorizontalVideoItemStyle")
                : IsDynamic ? resourceToolkit.GetResource<Style>("VerticalDynamicVideoItemStyle") : resourceToolkit.GetResource<Style>("VerticalVideoItemStyle");
        }
    }
}
