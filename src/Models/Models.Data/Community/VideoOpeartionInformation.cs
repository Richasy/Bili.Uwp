// Copyright (c) Richasy. All rights reserved.

namespace Bili.Models.Data.Community
{
    /// <summary>
    /// 用户对视频的操作信息，即一些交互，比如点赞与否，投币与否等.
    /// </summary>
    public sealed class VideoOpeartionInformation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VideoOpeartionInformation"/> class.
        /// </summary>
        public VideoOpeartionInformation()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VideoOpeartionInformation"/> class.
        /// </summary>
        /// <param name="id">视频 Id.</param>
        /// <param name="isLiked">是否点赞.</param>
        /// <param name="isCoined">是否投币.</param>
        /// <param name="isFavorited">是否收藏.</param>
        /// <param name="isFollowing">是否关注.</param>
        public VideoOpeartionInformation(
            string id,
            bool isLiked,
            bool isCoined,
            bool isFavorited,
            bool isFollowing)
        {
            Id = id;
            IsLiked = isLiked;
            IsCoined = isCoined;
            IsFollowing = isFollowing;
            IsFavorited = isFavorited;
        }

        /// <summary>
        /// 挂载的视频 Id.
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// 是否已点赞.
        /// </summary>
        public bool IsLiked { get; }

        /// <summary>
        /// 是否已投币.
        /// </summary>
        public bool IsCoined { get; }

        /// <summary>
        /// 是否已收藏.
        /// </summary>
        public bool IsFavorited { get; }

        /// <summary>
        /// 是否正在追番/追剧.
        /// </summary>
        public bool IsFollowing { get; }

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is VideoOpeartionInformation information && Id == information.Id;

        /// <inheritdoc/>
        public override int GetHashCode() => Id.GetHashCode();
    }
}
