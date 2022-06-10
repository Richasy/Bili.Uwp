// Copyright (c) Richasy. All rights reserved.

using Bili.ViewModels.Uwp.Video;
using ReactiveUI;
using Splat;

namespace Bili.App.Controls.Player
{
    /// <summary>
    /// 关联视频视图.
    /// </summary>
    public sealed partial class RelatedVideoView : RelatedVideoViewBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RelatedVideoView"/> class.
        /// </summary>
        public RelatedVideoView()
        {
            InitializeComponent();
            ViewModel = Splat.Locator.Current.GetService<VideoPlayerPageViewModel>();
            DataContext = ViewModel;
        }
    }

    /// <summary>
    /// <see cref="RelatedVideoView"/> 的基类.
    /// </summary>
    public class RelatedVideoViewBase : ReactiveUserControl<VideoPlayerPageViewModel>
    {
    }
}
