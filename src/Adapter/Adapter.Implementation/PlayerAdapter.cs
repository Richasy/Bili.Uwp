// Copyright (c) Richasy. All rights reserved.

using System.Linq;
using Bili.Adapter.Interfaces;
using Bili.Models.BiliBili;
using Bili.Models.Data.Player;

namespace Bili.Adapter
{
    /// <summary>
    /// 播放器数据适配器.
    /// </summary>
    public sealed class PlayerAdapter : IPlayerAdapter
    {
        /// <inheritdoc/>
        public FormatInformation ConvertToFormatInformation(VideoFormat format)
            => new FormatInformation(format.Quality, format.Description, !string.IsNullOrEmpty(format.Superscript));

        /// <inheritdoc/>
        public SegmentInformation ConvertToSegmentInformation(DashItem item)
        {
            return new SegmentInformation(
                item.Id.ToString(),
                item.BaseUrl,
                item.BackupUrl,
                item.BandWidth,
                item.MimeType,
                item.Codecs,
                item.Width,
                item.Height,
                item.SegmentBase.Initialization,
                item.SegmentBase.IndexRange);
        }

        /// <inheritdoc/>
        public MediaInformation ConvertToMediaInformation(PlayerInformation information)
        {
            var dash = information.VideoInformation;
            var minBuffer = dash.MinBufferTime;
            var videos = dash.Video?.Count > 0
                ? dash.Video.Select(p => ConvertToSegmentInformation(p))
                : null;
            var audios = dash.Audio?.Count > 0
                ? dash.Audio.Select(p => ConvertToSegmentInformation(p))
                : null;
            var formats = information.SupportFormats.Select(p => ConvertToFormatInformation(p)).ToList();
            return new MediaInformation(minBuffer, videos, audios, formats);
        }

        /// <inheritdoc/>
        public SubtitleMeta ConvertToSubtitleMeta(SubtitleIndexItem item)
            => new SubtitleMeta(item.Id.ToString(), item.DisplayLanguage, item.Url);

        /// <inheritdoc/>
        public SubtitleInformation ConvertToSubtitleInformation(SubtitleItem item)
            => new SubtitleInformation(item.From, item.To, item.Content);

        /// <inheritdoc/>
        public DanmakuInformation ConvertToDanmakuInformation(Bilibili.Community.Service.Dm.V1.DanmakuElem danmaku)
            => new DanmakuInformation(
                danmaku.Id.ToString(),
                danmaku.Content,
                danmaku.Mode,
                danmaku.Progress / 1000.0,
                danmaku.Color,
                danmaku.Fontsize);
    }
}
