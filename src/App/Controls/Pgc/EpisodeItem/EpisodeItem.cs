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
        /// Initializes a new instance of the <see cref="EpisodeItem"/> class.
        /// </summary>
        public EpisodeItem()
            => DefaultStyleKey = typeof(EpisodeItem);

        /// <inheritdoc/>
        public Size GetHolderSize() => new Size(210, 248);

        /// <inheritdoc/>
        public void ChangeLayout(Orientation orientation)
        {
            var resourceToolkit = Splat.Locator.Current.GetService<IResourceToolkit>();
            Style = orientation == Orientation.Horizontal
                ? resourceToolkit.GetResource<Style>("HorizontalEpisodeItemStyle")
                : resourceToolkit.GetResource<Style>("VerticalEpisodeItemStyle");
        }
    }
}
