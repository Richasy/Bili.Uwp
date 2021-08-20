// Copyright (c) Richasy. All rights reserved.

using System.Collections.ObjectModel;
using ReactiveUI.Fody.Helpers;
using Richasy.Bili.Models.BiliBili;
using Richasy.Bili.Toolkit.Interfaces;

namespace Richasy.Bili.ViewModels.Uwp
{
    /// <summary>
    /// 用户视图模型.
    /// </summary>
    public partial class UserViewModel
    {
        private readonly INumberToolkit _numberToolkit;
        private readonly IResourceToolkit _resourceToolkit;

        private UserSpaceInformation _detail;
        private string _videoOffsetId;

        private bool _isVideoLoadCompleted;
        private bool _isFollowRequesting;

        /// <summary>
        /// 头像.
        /// </summary>
        [Reactive]
        public string Avatar { get; set; }

        /// <summary>
        /// 用户名.
        /// </summary>
        [Reactive]
        public string Name { get; set; }

        /// <summary>
        /// 是否已关注.
        /// </summary>
        [Reactive]
        public bool IsFollow { get; set; }

        /// <summary>
        /// 个性签名/说明文本.
        /// </summary>
        [Reactive]
        public string Sign { get; set; }

        /// <summary>
        /// 粉丝数.
        /// </summary>
        [Reactive]
        public string FollowerCount { get; set; }

        /// <summary>
        /// 获赞数.
        /// </summary>
        [Reactive]
        public string LikeCount { get; set; }

        /// <summary>
        /// 关注数.
        /// </summary>
        [Reactive]
        public string FollowCount { get; set; }

        /// <summary>
        /// 等级.
        /// </summary>
        [Reactive]
        public int Level { get; set; }

        /// <summary>
        /// 是否显示关注按钮.
        /// </summary>
        [Reactive]
        public bool IsShowFollowButton { get; set; }

        /// <summary>
        /// 用户Id.
        /// </summary>
        [Reactive]
        public int Id { get; set; }

        /// <summary>
        /// 投稿视频集合.
        /// </summary>
        [Reactive]
        public ObservableCollection<VideoViewModel> VideoCollection { get; set; }

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is UserViewModel model && Id == model.Id;

        /// <inheritdoc/>
        public override int GetHashCode() => 2108858624 + Id.GetHashCode();
    }
}
