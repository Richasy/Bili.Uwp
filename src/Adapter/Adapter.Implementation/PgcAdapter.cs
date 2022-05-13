// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using Bili.Adapter.Interfaces;
using Bili.Models.App.Constants;
using Bili.Models.BiliBili;
using Bili.Models.Data.Pgc;
using Bili.Models.Data.Video;
using Bili.Models.Enums;

namespace Bili.Adapter
{
    /// <summary>
    /// PGC 内容适配器.
    /// </summary>
    public sealed class PgcAdapter : IPgcAdapter
    {
        private readonly IImageAdapter _imageAdapter;
        private readonly IUserAdapter _userAdapter;
        private readonly ICommunityAdapter _communityAdapter;

        /// <summary>
        /// Initializes a new instance of the <see cref="PgcAdapter"/> class.
        /// </summary>
        /// <param name="imageAdapter">图片适配器.</param>
        /// <param name="communityAdapter">社区信息适配器.</param>
        /// <param name="userAdapter">用户适配器.</param>
        public PgcAdapter(
            IImageAdapter imageAdapter,
            ICommunityAdapter communityAdapter,
            IUserAdapter userAdapter)
        {
            _imageAdapter = imageAdapter;
            _communityAdapter = communityAdapter;
            _userAdapter = userAdapter;
        }

        /// <inheritdoc/>
        public EpisodeInformation ConvertToEpisodeInformation(PgcEpisodeDetail episode)
        {
            var epid = episode.Report.EpisodeId;
            var title = episode.Report.EpisodeTitle;
            var duration = episode.Duration;
            var cover = _imageAdapter.ConvertToVideoCardCover(episode.Cover);
            var seasonId = episode.Report.SeasonId;
            var aid = episode.Aid.ToString();
            var index = episode.Index;
            var isPv = episode.IsPV == 1;
            var isVip = episode.BadgeText == "会员";
            var publishTime = DateTimeOffset.FromUnixTimeSeconds(episode.PublishTime).ToLocalTime().DateTime;
            var communityInfo = _communityAdapter.ConvertToVideoCommunityInformation(episode.Stat);

            var identifier = new VideoIdentifier(epid, title, duration, cover);
            return new EpisodeInformation(
                identifier,
                seasonId,
                aid,
                default,
                index,
                isVip,
                isPv,
                publishTime,
                communityInfo);
        }

        /// <inheritdoc/>
        public EpisodeInformation ConvertToEpisodeInformation(RecommendCard card)
        {
            if (card.CardGoto != ServiceConstants.Bangumi)
            {
                throw new ArgumentException($"推荐卡片的 CardGoTo 属性应该是 {ServiceConstants.Bangumi}，这里是 {card.Goto}，不符合要求，请检查分配条件", nameof(card));
            }

            var epid = card.Parameter;
            var title = card.Title;
            var subtitle = card.Description;
            var communityInfo = _communityAdapter.ConvertToVideoCommunityInformation(card);
            var cover = _imageAdapter.ConvertToVideoCardCover(card.Cover);

            var identifier = new VideoIdentifier(epid, title, -1, cover);
            return new EpisodeInformation(identifier, subtitle: subtitle, communityInformation: communityInfo);
        }

        /// <inheritdoc/>
        public SeasonInformation ConvertToSeasonInformation(PgcDisplayInformation display)
        {
            var ssid = display.SeasonId.ToString();
            var title = display.Title;
            var cover = _imageAdapter.ConvertToImage(display.Cover, 240, 320);
            var subtitle = display.Subtitle;
            var description = display.Evaluate;
            var highlight = display.BadgeText;
            var progress = display.PublishInformation.DisplayProgress;
            var publishDate = display.PublishInformation.DisplayPublishTime;
            var originName = display.OriginName;
            var alias = display.Alias;
            var typeName = display.TypeName;
            var tags = display.TypeDescription;
            var ratingCount = display.Rating != null
                ? display.Rating.Count
                : -1;
            var labors = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(display.Actor?.Information))
            {
                labors.Add(display.Actor.Title, display.Actor.Information);
            }

            if (!string.IsNullOrEmpty(display.Staff?.Information))
            {
                labors.Add(display.Staff.Title, display.Staff.Information);
            }

            var celebrities = display.Celebrity?.Select(p => _userAdapter.ConvertToRoleProfile(p));
            var communityInfo = _communityAdapter.ConvertToVideoCommunityInformation(display.InformationStat);
            if (display.Rating != null)
            {
                communityInfo.Score = display.Rating.Score;
            }

            var type = display.TypeName switch
            {
                "国创" => PgcType.Domestic,
                "电影" => PgcType.Movie,
                "电视剧" => PgcType.TV,
                "纪录片" => PgcType.Documentary,
                _ => PgcType.Bangumi,
            };

            var identifier = new VideoIdentifier(ssid, title, -1, cover);
            return new SeasonInformation(
                identifier,
                subtitle,
                highlight,
                tags,
                ratingCount,
                originName,
                alias,
                publishDate,
                progress,
                description,
                type,
                labors,
                communityInfo,
                celebrities);
        }
    }
}
