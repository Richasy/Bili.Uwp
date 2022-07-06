// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Bili.Adapter.Interfaces;
using Bili.Models.BiliBili;
using Bili.Models.Data.Player;
using Bili.Toolkit.Interfaces;

namespace Bili.Adapter
{
    /// <summary>
    /// 播放器数据适配器.
    /// </summary>
    public sealed class PlayerAdapter : IPlayerAdapter
    {
        private readonly ITextToolkit _textToolkit;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerAdapter"/> class.
        /// </summary>
        /// <param name="textToolkit">文本工具.</param>
        public PlayerAdapter(ITextToolkit textToolkit)
            => _textToolkit = textToolkit;

        /// <inheritdoc/>
        public FormatInformation ConvertToFormatInformation(VideoFormat format)
            => new FormatInformation(
                format.Quality,
                _textToolkit.ConvertToTraditionalChineseIfNeeded(format.Description),
                !string.IsNullOrEmpty(format.Superscript));

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
            if (dash == null)
            {
                return default;
            }

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
        public MediaInformation ConvertToMediaInformation(Bilibili.App.Playurl.V1.PlayViewReply reply)
        {
            var info = reply.VideoInfo;
            if (info == null)
            {
                return default;
            }

            var videoStreams = info.StreamList.ToList();
            var audioStreams = info.DashAudio != null
                ? info.DashAudio.ToList()
                : new List<Bilibili.App.Playurl.V1.DashItem>();
            var videos = new List<SegmentInformation>();
            var audios = new List<SegmentInformation>();
            var formats = new List<FormatInformation>();

            foreach (var video in videoStreams)
            {
                if (video.DashVideo == null)
                {
                    continue;
                }

                var seg = new SegmentInformation(
                    video.StreamInfo.Quality.ToString(),
                    video.DashVideo.BaseUrl,
                    video.DashVideo.BackupUrl.ToList(),
                    default,
                    default,
                    video.DashVideo.Codecid.ToString(),
                    default,
                    default,
                    default,
                    default);
                var format = new FormatInformation(
                    Convert.ToInt32(video.StreamInfo.Quality),
                    video.StreamInfo.Description,
                    video.StreamInfo.NeedVip);
                videos.Add(seg);
                formats.Add(format);
            }

            foreach (var audio in audioStreams)
            {
                var seg = new SegmentInformation(
                    audio.Id.ToString(),
                    audio.BaseUrl,
                    audio.BackupUrl.ToList(),
                    default,
                    default,
                    audio.Codecid.ToString(),
                    default,
                    default,
                    default,
                    default);

                audios.Add(seg);
            }

            return new MediaInformation(default, videos, audios, formats);
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
                _textToolkit.ConvertToTraditionalChineseIfNeeded(danmaku.Content),
                danmaku.Mode,
                danmaku.Progress / 1000.0,
                danmaku.Color,
                danmaku.Fontsize);

        /// <inheritdoc/>
        public InteractionInformation ConvertToInteractionInformation(InteractionChoice choice, IEnumerable<InteractionHiddenVariable> variables)
        {
            var id = choice.Id.ToString();
            var condition = choice.Condition ?? string.Empty;
            var partId = choice.PartId.ToString();
            var text = _textToolkit.ConvertToTraditionalChineseIfNeeded(choice.Option);
            var isValid = true;

            if (!string.IsNullOrEmpty(condition) && variables != null)
            {
                var v = variables.FirstOrDefault(p => condition.Contains(p.Id));
                var minString = Regex.Match(condition, ">=([0-9]{1,}[.][0-9]*)").Value.Replace(">=", string.Empty);
                var maxString = Regex.Match(condition, "<=([0-9]{1,}[.][0-9]*)").Value.Replace("<=", string.Empty);
                var min = string.IsNullOrEmpty(minString) ? 0 : Convert.ToDouble(minString);
                var max = string.IsNullOrEmpty(maxString) ? -1 : Convert.ToDouble(maxString);
                isValid = v != null && v.Value >= min && (max == -1 || v.Value <= max);
            }

            return new InteractionInformation(id, condition, partId, text, isValid);
        }
    }
}
