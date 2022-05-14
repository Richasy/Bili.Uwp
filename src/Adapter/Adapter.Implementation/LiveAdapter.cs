// Copyright (c) Richasy. All rights reserved.

using System;
using System.Net;
using System.Text.RegularExpressions;
using Bili.Adapter.Interfaces;
using Bili.Models.BiliBili;
using Bili.Models.Data.Live;
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
        private readonly INumberToolkit _numberToolkit;

        /// <summary>
        /// Initializes a new instance of the <see cref="LiveAdapter"/> class.
        /// </summary>
        /// <param name="userAdapter">用户数据适配器.</param>
        /// <param name="imageAdapter">图片数据适配器.</param>
        /// <param name="numberToolkit">数字转换工具.</param>
        public LiveAdapter(
            IUserAdapter userAdapter,
            IImageAdapter imageAdapter,
            INumberToolkit numberToolkit)
        {
            _userAdapter = userAdapter;
            _imageAdapter = imageAdapter;
            _numberToolkit = numberToolkit;
        }

        /// <inheritdoc/>
        public LiveInformation ConvertToLiveInformation(LiveFeedRoom room)
        {
            var title = room.Title;
            var id = room.RoomId.ToString();
            var viewerCount = room.ViewerCount;
            var user = _userAdapter.ConvertToUserProfile(room.UserId, room.UserName, room.UserAvatar, Models.Enums.App.AvatarSize.Size48);
            var cover = _imageAdapter.ConvertToVideoCardCover(room.Cover);
            var subtitle = room.DisplayAreaName;

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
            var title = card.Title;
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
            var title = item.Title;
            var id = item.RoomId.ToString();
            var viewerCount = item.ViewerCount;
            var cover = _imageAdapter.ConvertToVideoCardCover(item.Cover);
            var subtitle = item.Name;

            var identifier = new VideoIdentifier(id, title, -1, cover);
            return new LiveInformation(identifier, null, viewerCount, subtitle: subtitle);
        }

        /// <inheritdoc/>
        public LiveView ConvertToLiveView(LiveRoomDetail detail)
        {
            var roomInfo = detail.RoomInformation;
            var title = roomInfo.Title;
            var id = roomInfo.RoomId.ToString();
            var description = string.IsNullOrEmpty(roomInfo.Description)
                ? string.Empty
                : Regex.Replace(roomInfo.Description, @"<[^>]*>", string.Empty);

            if (!string.IsNullOrEmpty(description))
            {
                description = WebUtility.HtmlDecode(description);
            }

            var viewerCount = roomInfo.ViewerCount;
            var cover = _imageAdapter.ConvertToImage(roomInfo.Cover ?? roomInfo.Keyframe);
            var userInfo = detail.AnchorInformation.UserBasicInformation;
            var userProfile = _userAdapter.ConvertToUserProfile(roomInfo.UserId, userInfo.UserName, userInfo.Avatar, Models.Enums.App.AvatarSize.Size48);
            var partition = $"{roomInfo.ParentAreaName} · {roomInfo.AreaName}";
            var subtitle = DateTimeOffset.FromUnixTimeSeconds(detail.RoomInformation.LiveStartTime).ToLocalTime().ToString("yyyy/MM/dd HH:mm");

            var identifier = new VideoIdentifier(id, title, -1, cover);
            var info = new LiveInformation(identifier, userProfile, viewerCount, subtitle: subtitle, description: description);
            return new LiveView(info, partition);
        }
    }
}
