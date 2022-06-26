// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using Bili.Adapter.Interfaces;
using Bili.Models.BiliBili;
using Bili.Models.Data.Live;
using Bili.Models.Data.Player;
using Bili.Models.Data.Video;
using Bili.Toolkit.Interfaces;

namespace Bili.Adapter
{
    /// <summary>
    /// 直播数据适配器.
    /// </summary>
    public sealed class LiveAdapter : ILiveAdapter
    {
        private readonly IUserAdapter _userAdapter;
        private readonly IImageAdapter _imageAdapter;
        private readonly ICommunityAdapter _communityAdapter;
        private readonly INumberToolkit _numberToolkit;
        private readonly ITextToolkit _textToolkit;

        /// <summary>
        /// Initializes a new instance of the <see cref="LiveAdapter"/> class.
        /// </summary>
        /// <param name="userAdapter">用户数据适配器.</param>
        /// <param name="imageAdapter">图片数据适配器.</param>
        /// <param name="communityAdapter">社区数据适配器.</param>
        /// <param name="numberToolkit">数字转换工具.</param>
        /// <param name="textToolkit">文本工具.</param>
        public LiveAdapter(
            IUserAdapter userAdapter,
            IImageAdapter imageAdapter,
            ICommunityAdapter communityAdapter,
            INumberToolkit numberToolkit,
            ITextToolkit textToolkit)
        {
            _userAdapter = userAdapter;
            _imageAdapter = imageAdapter;
            _communityAdapter = communityAdapter;
            _numberToolkit = numberToolkit;
            _textToolkit = textToolkit;
        }

        /// <inheritdoc/>
        public LiveInformation ConvertToLiveInformation(LiveFeedRoom room)
        {
            var title = _textToolkit.ConvertToTraditionalChineseIfNeeded(room.Title);
            var id = room.RoomId.ToString();
            var viewerCount = room.ViewerCount;
            var user = _userAdapter.ConvertToUserProfile(room.UserId, room.UserName, room.UserAvatar, Models.Enums.App.AvatarSize.Size48);
            var cover = _imageAdapter.ConvertToVideoCardCover(room.Cover);
            var subtitle = _textToolkit.ConvertToTraditionalChineseIfNeeded(room.DisplayAreaName);

            var identifier = new VideoIdentifier(id, title, -1, cover);
            return new LiveInformation(
                identifier,
                user,
                viewerCount,
                subtitle: subtitle);
        }

        /// <inheritdoc/>
        public LiveInformation ConvertToLiveInformation(LiveRoomCard card)
        {
            var title = _textToolkit.ConvertToTraditionalChineseIfNeeded(card.Title);
            var id = card.RoomId.ToString();
            var viewerCount = _numberToolkit.GetCountNumber(card.CoverRightContent.Text);
            var subtitle = card.CoverLeftContent.Text;
            var cover = _imageAdapter.ConvertToVideoCardCover(card.Cover);
            var identifier = new VideoIdentifier(id, title, -1, cover);
            return new LiveInformation(identifier, default, viewerCount, subtitle: subtitle);
        }

        /// <inheritdoc/>
        public LiveInformation ConvertToLiveInformation(LiveSearchItem item)
        {
            var title = _textToolkit.ConvertToTraditionalChineseIfNeeded(item.Title);
            var id = item.RoomId.ToString();
            var viewerCount = item.ViewerCount;
            var cover = _imageAdapter.ConvertToVideoCardCover(item.Cover);
            var subtitle = item.Name;

            var identifier = new VideoIdentifier(id, title, -1, cover);
            return new LiveInformation(identifier, null, viewerCount, subtitle: subtitle);
        }

        /// <inheritdoc/>
        public LivePlayerView ConvertToLivePlayerView(LiveRoomDetail detail)
        {
            var roomInfo = detail.RoomInformation;
            var title = _textToolkit.ConvertToTraditionalChineseIfNeeded(roomInfo.Title);
            var id = roomInfo.RoomId.ToString();
            var description = string.IsNullOrEmpty(roomInfo.Description)
                ? string.Empty
                : Regex.Replace(roomInfo.Description, @"<[^>]*>", string.Empty);

            if (!string.IsNullOrEmpty(description))
            {
                description = WebUtility.HtmlDecode(description).Trim();
            }

            if (string.IsNullOrEmpty(description))
            {
                description = "暂无直播间介绍";
            }

            description = _textToolkit.ConvertToTraditionalChineseIfNeeded(description);

            var viewerCount = roomInfo.ViewerCount;
            var cover = _imageAdapter.ConvertToImage(roomInfo.Cover ?? roomInfo.Keyframe);
            var userInfo = detail.AnchorInformation.UserBasicInformation;
            var userProfile = _userAdapter.ConvertToUserProfile(roomInfo.UserId, userInfo.UserName, userInfo.Avatar, Models.Enums.App.AvatarSize.Size48);
            var partition = $"{roomInfo.ParentAreaName} · {roomInfo.AreaName}";
            var subtitle = DateTimeOffset.FromUnixTimeSeconds(detail.RoomInformation.LiveStartTime).ToLocalTime().ToString("yyyy/MM/dd HH:mm");

            var identifier = new VideoIdentifier(id, title, -1, cover);
            var info = new LiveInformation(identifier, userProfile, viewerCount, subtitle: subtitle, description: description);
            return new LivePlayerView(info, partition);
        }

        /// <inheritdoc/>
        public LiveFeedView ConvertToLiveFeedView(LiveFeedResponse response)
        {
            var recommendRooms = response.CardList.Where(p => p.CardType.Contains("small_card"))
                .Where(p => p.CardData?.LiveCard != null)
                .Select(p => ConvertToLiveInformation(p.CardData.LiveCard))
                .ToList();
            var followRooms = response.CardList.Where(p => p.CardType.Contains("idol"))
                .SelectMany(p => p.CardData?.FollowList?.List)
                .Select(p => ConvertToLiveInformation(p))
                .ToList();
            var banners = response.CardList.Where(p => p.CardType.Contains("banner"))
                .SelectMany(p => p.CardData?.Banners?.List)
                .Select(p => _communityAdapter.ConvertToBannerIdentifier(p))
                .ToList();
            var partitions = response.CardList.Where(p => p.CardType.Contains("area"))
                .SelectMany(p => p.CardData?.HotAreas?.List)
                .Where(p => p.Id != 0)
                .Select(p => _communityAdapter.ConvertToPartition(p))
                .ToList();

            return new LiveFeedView(banners, partitions, followRooms, recommendRooms);
        }

        /// <inheritdoc/>
        public LivePartitionView ConvertToLivePartitionView(LiveAreaDetailResponse response)
        {
            var lives = response.List.Select(p => ConvertToLiveInformation(p)).ToList();
            var tags = response.Tags?.Count > 0
                ? response.Tags.Select(p => new LiveTag(p.Id.ToString(), p.Name, p.SortType)).ToList()
                : new List<LiveTag>() { new LiveTag(string.Empty, _textToolkit.ConvertToTraditionalChineseIfNeeded("全部"), string.Empty) };
            return new LivePartitionView(response.Count, lives, tags);
        }

        /// <inheritdoc/>
        public LiveMediaInformation ConvertToLiveMediaInformation(LiveAppPlayInformation information)
        {
            var id = information.RoomId.ToString();
            var playInfo = information.PlayUrlInfo.PlayUrl;
            var formats = new List<FormatInformation>();
            foreach (var item in playInfo.Descriptions)
            {
                var desc = _textToolkit.ConvertToTraditionalChineseIfNeeded(item.Description);
                formats.Add(new FormatInformation(item.Quality, desc, false));
            }

            var lines = new List<LivePlaylineInformation>();
            foreach (var stream in playInfo.StreamList)
            {
                foreach (var format in stream.FormatList)
                {
                    foreach (var codec in format.CodecList)
                    {
                        var name = codec.CodecName;
                        var urls = codec.Urls.Select(p => new LivePlayUrl(p.Host, codec.BaseUrl, p.Extra));
                        lines.Add(new LivePlaylineInformation(name, codec.CurrentQuality, codec.AcceptQualities, urls));
                    }
                }
            }

            return new LiveMediaInformation(id, formats, lines);
        }
    }
}
