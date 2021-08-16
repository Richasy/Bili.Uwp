// Copyright (c) Richasy. All rights reserved.

using ReactiveUI.Fody.Helpers;

namespace Richasy.Bili.ViewModels.Uwp
{
    /// <summary>
    /// 剧集视图模型.
    /// </summary>
    public partial class SeasonViewModel
    {
        /// <summary>
        /// 剧集Id.
        /// </summary>
        public int SeasonId { get; set; }

        /// <summary>
        /// 视频Id.
        /// </summary>
        public int EpisodeId { get; set; }

        /// <summary>
        /// 视频标题.
        /// </summary>
        [Reactive]
        public string Title { get; set; }

        /// <summary>
        /// 副标题.
        /// </summary>
        [Reactive]
        public string Subtitle { get; set; }

        /// <summary>
        /// 剧集标签.
        /// </summary>
        [Reactive]
        public string Tags { get; set; }

        /// <summary>
        /// 封面地址.
        /// </summary>
        [Reactive]
        public string CoverUrl { get; set; }

        /// <summary>
        /// 附加文本.
        /// </summary>
        [Reactive]
        public string AdditionalText { get; set; }

        /// <summary>
        /// 徽章文本.
        /// </summary>
        [Reactive]
        public string BadgeText { get; set; }

        /// <summary>
        /// 是否显示徽章.
        /// </summary>
        [Reactive]
        public bool IsShowBadge { get; set; }

        /// <summary>
        /// 是否显示标签.
        /// </summary>
        [Reactive]
        public bool IsShowTags { get; set; }

        /// <summary>
        /// 是否显示附加文本.
        /// </summary>
        [Reactive]
        public bool IsShowAdditionalText { get; set; }

        /// <summary>
        /// 评分.
        /// </summary>
        [Reactive]
        public double Rating { get; set; }

        /// <summary>
        /// 是否显示评分.
        /// </summary>
        [Reactive]
        public bool IsShowRating { get; set; }

        /// <summary>
        /// 源数据.
        /// </summary>
        public object Source { get; set; }

        /// <summary>
        /// 源封面.
        /// </summary>
        public string SourceCoverUrl { get; set; }

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is SeasonViewModel model && EpisodeId == model.EpisodeId;

        /// <inheritdoc/>
        public override int GetHashCode() => -593024695 + EpisodeId.GetHashCode();
    }
}
