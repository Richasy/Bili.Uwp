// Copyright (c) Richasy. All rights reserved.

using System;
using Bili.Adapter.Interfaces;
using Bili.Models.App.Constants;
using Bili.Models.BiliBili;
using Bili.Models.Data.Player;

namespace Bili.Adapter
{
    /// <summary>
    /// 视频信息适配器.
    /// </summary>
    public sealed class VideoAdapter : IVideoAdapter
    {
        private readonly ICommunityAdapter _communityAdapter;
        private readonly IUserAdapter _userAdapter;
        private readonly IImageAdapter _imageAdapter;

        /// <summary>
        /// Initializes a new instance of the <see cref="VideoAdapter"/> class.
        /// </summary>
        /// <param name="communityAdapter">社区数据适配器.</param>
        /// <param name="userAdapter">用户数据适配器.</param>
        /// <param name="imageAdapter">图片适配器.</param>
        public VideoAdapter(ICommunityAdapter communityAdapter, IUserAdapter userAdapter, IImageAdapter imageAdapter)
        {
            _communityAdapter = communityAdapter;
            _userAdapter = userAdapter;
            _imageAdapter = imageAdapter;
        }

        /// <inheritdoc/>
        public VideoInformation ConvertToVideoInformation(RecommendCard videoCard)
        {
            if (videoCard.CardGoto != ServiceConstants.Av)
            {
                throw new ArgumentException($"推荐卡片的 CardGoTo 属性应该是 {ServiceConstants.Av}，这里是 {videoCard.Goto}，不符合要求，请检查分配条件", nameof(videoCard));
            }

            var communityInfo = _communityAdapter.ConvertToVideoCommunityInformation(videoCard);
            var publisher = _userAdapter.ConvertToPublisherProfile(videoCard.Mask.Avatar);
            var title = videoCard.Title;
            var id = videoCard.Parameter;
            var duration = (videoCard.PlayerArgs?.Duration).HasValue
                    ? videoCard.PlayerArgs.Duration
                    : 0;
            var subtitle = videoCard.Description;
            var cover = _imageAdapter.ConvertToImage(videoCard.Cover, AppConstants.VideoCardCoverWidth, AppConstants.VideoCardCoverHeight);

            var identifier = new VideoIdentifier(id, title, duration, cover);
            return new VideoInformation(
                identifier,
                publisher,
                subtitle: subtitle,
                communityInformation: communityInfo);
        }
    }
}
