// Copyright (c) Richasy. All rights reserved.

using Bili.ViewModels.Interfaces.Video;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Bili.App.Resources.Selector
{
    internal sealed class RecommendDataTemplateSelector : DataTemplateSelector
    {
        /// <summary>
        /// 视频卡片模板.
        /// </summary>
        public DataTemplate VideoTemplate { get; set; }

        /// <summary>
        /// 剧集卡片模板.
        /// </summary>
        public DataTemplate EpisodeTemplate { get; set; }

        /// <inheritdoc/>
        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            return item is IVideoItemViewModel
                ? VideoTemplate
                : EpisodeTemplate;
        }
    }
}
