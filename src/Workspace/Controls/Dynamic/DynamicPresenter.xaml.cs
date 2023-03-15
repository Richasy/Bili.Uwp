// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using Bili.DI.Container;
using Bili.Models.Data.Article;
using Bili.Models.Data.Dynamic;
using Bili.Models.Data.Pgc;
using Bili.Models.Data.Video;
using Bili.ViewModels.Interfaces.Article;
using Bili.ViewModels.Interfaces.Community;
using Bili.ViewModels.Interfaces.Pgc;
using Bili.ViewModels.Interfaces.Video;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;

namespace Bili.Workspace.Controls.Dynamic
{
    /// <summary>
    /// 动态展示.
    /// </summary>
    public sealed partial class DynamicPresenter : UserControl
    {
        /// <summary>
        /// <see cref="Data"/> 的依赖属性.
        /// </summary>
        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register(nameof(Data), typeof(object), typeof(DynamicPresenter), new PropertyMetadata(default, new PropertyChangedCallback(DataChanged)));

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicPresenter"/> class.
        /// </summary>
        public DynamicPresenter() => InitializeComponent();

        /// <summary>
        /// 数据.
        /// </summary>
        public object Data
        {
            get { return (object)GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }

        private static void DataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as DynamicPresenter;
            var data = e.NewValue;

            if (data is VideoInformation videoInfo)
            {
                var videoVM = Locator.Instance.GetService<IVideoItemViewModel>();
                videoVM.InjectData(videoInfo);
                instance.MainPresenter.Content = videoVM;
                instance.MainPresenter.ContentTemplate = instance.VideoTemplate;
            }
            else if (data is EpisodeInformation episodeInfo)
            {
                var episodeVM = Locator.Instance.GetService<IEpisodeItemViewModel>();
                episodeVM.InjectData(episodeInfo);
                instance.MainPresenter.Content = episodeVM;
                instance.MainPresenter.ContentTemplate = instance.EpisodeTemplate;
            }
        }
    }
}
