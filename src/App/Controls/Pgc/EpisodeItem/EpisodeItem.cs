// Copyright (c) Richasy. All rights reserved.

using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Uwp.Pgc;
using Splat;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Bili.App.Controls.Pgc
{
    /// <summary>
    /// 剧集单集条目视图.
    /// </summary>
    public sealed class EpisodeItem : ReactiveControl<EpisodeItemViewModel>, IRepeaterItem, IOrientationControl
    {
        /// <summary>
        /// <see cref="IsDynamic"/> 的依赖属性.
        /// </summary>
        public static readonly DependencyProperty IsDynamicProperty =
            DependencyProperty.Register(nameof(IsDynamic), typeof(bool), typeof(EpisodeItem), new PropertyMetadata(false));

        /// <summary>
        /// Initializes a new instance of the <see cref="EpisodeItem"/> class.
        /// </summary>
        public EpisodeItem()
            => DefaultStyleKey = typeof(EpisodeItem);

        /// <summary>
        /// 是否是动态剧集.
        /// </summary>
        public bool IsDynamic
        {
            get { return (bool)GetValue(IsDynamicProperty); }
            set { SetValue(IsDynamicProperty, value); }
        }

        /// <inheritdoc/>
        public Size GetHolderSize() => new Size(210, 248);

        /// <inheritdoc/>
        public void ChangeLayout(Orientation orientation)
        {
            var resourceToolkit = Splat.Locator.Current.GetService<IResourceToolkit>();
            Style = orientation == Orientation.Horizontal
                ? IsDynamic ? resourceToolkit.GetResource<Style>("HorizontalDynamicEpisodeItemStyle") : resourceToolkit.GetResource<Style>("HorizontalEpisodeItemStyle")
                : IsDynamic ? resourceToolkit.GetResource<Style>("VerticalDynamicEpisodeItemStyle") : resourceToolkit.GetResource<Style>("VerticalEpisodeItemStyle");
        }
    }
}
