// Copyright (c) Richasy. All rights reserved.

using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Live;
using Splat;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Bili.App.Controls.Live
{
    /// <summary>
    /// 直播间视频卡片.
    /// </summary>
    public sealed class LiveItem : ReactiveControl<ILiveItemViewModel>, IRepeaterItem, IOrientationControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LiveItem"/> class.
        /// </summary>
        public LiveItem()
            => DefaultStyleKey = typeof(LiveItem);

        /// <inheritdoc/>
        public Size GetHolderSize() => new(210, 248);

        /// <inheritdoc/>
        public void ChangeLayout(Orientation orientation)
        {
            var resourceToolkit = Splat.Locator.Current.GetService<IResourceToolkit>();
            Style = orientation == Orientation.Horizontal
                ? resourceToolkit.GetResource<Style>("HorizontalLiveItemStyle")
                : resourceToolkit.GetResource<Style>("VerticalLiveItemStyle");
        }
    }
}
