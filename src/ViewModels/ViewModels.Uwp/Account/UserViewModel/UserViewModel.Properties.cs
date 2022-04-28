// Copyright (c) Richasy. All rights reserved.

using System.Collections.ObjectModel;
using Bili.Models.BiliBili;
using Bili.Toolkit.Interfaces;
using ReactiveUI.Fody.Helpers;

namespace Bili.ViewModels.Uwp
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
        private int _searchPageNumber;

        private bool _isVideoLoadCompleted;
        private bool _isFollowRequesting;
        private bool _isSearchLoadCompleted;

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
        /// 是否显示无视频提示.
        /// </summary>
        [Reactive]
        public bool IsShowVideoEmpty { get; set; }

        /// <summary>
        /// 是否显示无搜索结果提示.
        /// </summary>
        [Reactive]
        public bool IsShowSearchEmpty { get; set; }

        /// <summary>
        /// 附加文本.
        /// </summary>
        [Reactive]
        public string AdditionalText { get; set; }

        /// <summary>
        /// 搜索关键词.
        /// </summary>
        [Reactive]
        public string SearchKeyword { get; set; }

        /// <summary>
        /// 用户Id.
        /// </summary>
        [Reactive]
        public int Id { get; set; }

        /// <summary>
        /// 投稿视频集合.
        /// </summary>
        public ObservableCollection<VideoViewModel> VideoCollection { get; }

        /// <summary>
        /// 搜索结果集合.
        /// </summary>
        public ObservableCollection<VideoViewModel> SearchCollection { get; }

        /// <summary>
        /// 用户是否已被固定，<c>true</c> 意味着显示固定按钮，<c>false</c> 意味着显示取消固定按钮.
        /// </summary>
        [Reactive]
        public bool IsPublisherFixed { get; set; }

        /// <summary>
        /// 是否可以显示固定用户相关的操作（意味着用户是否已经登录）.
        /// </summary>
        [Reactive]
        public bool CanFixPublisher { get; set; }

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is UserViewModel model && Id == model.Id;

        /// <inheritdoc/>
        public override int GetHashCode() => 2108858624 + Id.GetHashCode();
    }
}
