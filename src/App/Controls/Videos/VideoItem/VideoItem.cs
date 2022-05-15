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
        /// Initializes a new instance of the <see cref="VideoItem"/> class.
        /// </summary>
        public VideoItem()
            => DefaultStyleKey = typeof(VideoItem);

        /// <inheritdoc/>
        public Size GetHolderSize() => new (210, 248);

        /// <inheritdoc/>
        public void ChangeLayout(Orientation orientation)
        {
            var resourceToolkit = Splat.Locator.Current.GetService<IResourceToolkit>();
            Style = orientation == Orientation.Horizontal
                ? resourceToolkit.GetResource<Style>("HorizontalVideoItemStyle")
                : resourceToolkit.GetResource<Style>("VerticalVideoItemStyle");
        }
    }
}
