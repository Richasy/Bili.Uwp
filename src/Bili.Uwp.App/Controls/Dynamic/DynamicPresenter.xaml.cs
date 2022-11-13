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
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Bili.Uwp.App.Controls.Dynamic
{
    /// <summary>
    /// 动态展示.
    /// </summary>
    public sealed partial class DynamicPresenter : UserControl, IOrientationControl
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

        /// <inheritdoc/>
        public void ChangeLayout(Orientation orientation)
        {
            if (VisualTreeHelper.GetChildrenCount(MainPresenter) > 0)
            {
                var child = VisualTreeHelper.GetChild(MainPresenter, 0);
                if (child is IOrientationControl item)
                {
                    item.ChangeLayout(orientation);
                }
            }
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
            else if (data is DynamicInformation dynamicInfo)
            {
                var dynamicVM = Locator.Instance.GetService<IDynamicItemViewModel>();
                dynamicVM.InjectData(dynamicInfo);
                instance.MainPresenter.Content = dynamicVM;
                instance.MainPresenter.ContentTemplate = instance.ForwardTemplate;
            }
            else if (data is List<Models.Data.Appearance.Image> images)
            {
                instance.MainPresenter.Content = images;
                instance.MainPresenter.ContentTemplate = instance.ImageTemplate;
            }
            else if (data is ArticleInformation article)
            {
                var articleVM = Locator.Instance.GetService<IArticleItemViewModel>();
                articleVM.InjectData(article);
                instance.MainPresenter.Content = articleVM;
                instance.MainPresenter.ContentTemplate = instance.ArticleTemplate;
            }
            else
            {
                instance.MainPresenter.Content = null;
                instance.MainPresenter.ContentTemplate = instance.NoneTemplate;
            }
        }
    }
}
