// Copyright (c) Richasy. All rights reserved.

using Bili.DI.Container;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Live;
using Windows.Foundation;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Bili.Desktop.App.Controls.Live
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
            var resourceToolkit = Locator.Instance.GetService<IResourceToolkit>();
            Style = orientation == Orientation.Horizontal
                ? resourceToolkit.GetResource<Style>("HorizontalLiveItemStyle")
                : resourceToolkit.GetResource<Style>("VerticalLiveItemStyle");
        }
    }
}
