// Copyright (c) Richasy. All rights reserved.

using ReactiveUI.Fody.Helpers;
using Richasy.Bili.Models.BiliBili;

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
        public int VideoId { get; set; }

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
        /// 源数据.
        /// </summary>
        public PgcModuleItem Source { get; set; }

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is SeasonViewModel model && VideoId == model.VideoId;

        /// <inheritdoc/>
        public override int GetHashCode() => -593024695 + VideoId.GetHashCode();
    }
}
