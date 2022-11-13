// Copyright (c) Richasy. All rights reserved.

using Bili.DI.Container;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Video;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Bili.Uwp.App.Controls.Videos
{
    /// <summary>
    /// 用来显示视频条目的 UI 单元，可以通过样式展现不同的布局.
    /// </summary>
    public sealed partial class VideoItem : ReactiveControl<IVideoItemViewModel>, IRepeaterItem, IOrientationControl
    {
        /// <summary>
        /// <see cref="IsDynamic"/> 的依赖属性.
        /// </summary>
        public static readonly DependencyProperty IsDynamicProperty =
            DependencyProperty.Register(nameof(IsDynamic), typeof(bool), typeof(VideoItem), new PropertyMetadata(false));

        /// <summary>
        /// <see cref="IsCustom"/> 的依赖属性.
        /// </summary>
        public static readonly DependencyProperty IsCustomProperty =
            DependencyProperty.Register(nameof(IsCustom), typeof(bool), typeof(VideoItem), new PropertyMetadata(false));

        /// <summary>
        /// Initializes a new instance of the <see cref="VideoItem"/> class.
        /// </summary>
        public VideoItem() => DefaultStyleKey = typeof(VideoItem);

        /// <summary>
        /// 是否是动态视频.
        /// </summary>
        public bool IsDynamic
        {
            get { return (bool)GetValue(IsDynamicProperty); }
            set { SetValue(IsDynamicProperty, value); }
        }

        /// <summary>
        /// 是否为自定义样式，设置该值后，卡片将不能根据 <see cref="Orientation"/> 自动切换样式.
        /// </summary>
        public bool IsCustom
        {
            get { return (bool)GetValue(IsCustomProperty); }
            set { SetValue(IsCustomProperty, value); }
        }

        /// <inheritdoc/>
        public Size GetHolderSize() => new Size(210, 248);

        /// <inheritdoc/>
        public void ChangeLayout(Orientation orientation)
        {
            if (IsCustom)
            {
                return;
            }

            var resourceToolkit = Locator.Instance.GetService<IResourceToolkit>();
            Style = orientation == Orientation.Horizontal
                ? IsDynamic ? resourceToolkit.GetResource<Style>("HorizontalDynamicVideoItemStyle") : resourceToolkit.GetResource<Style>("HorizontalVideoItemStyle")
                : IsDynamic ? resourceToolkit.GetResource<Style>("VerticalDynamicVideoItemStyle") : resourceToolkit.GetResource<Style>("VerticalVideoItemStyle");
        }
    }
}
