// Copyright (c) Richasy. All rights reserved.

using System.Collections.ObjectModel;
using ReactiveUI.Fody.Helpers;

namespace Bili.ViewModels.Uwp
{
    /// <summary>
    /// 视频收藏夹视图模型.
    /// </summary>
    public partial class FavoriteVideoViewModel
    {
        private bool _isLoadCompleted;
        private int _pageNumber;

        /// <summary>
        /// 收藏夹Id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 收藏夹名.
        /// </summary>
        [Reactive]
        public string Name { get; set; }

        /// <summary>
        /// 收藏夹说明.
        /// </summary>
        [Reactive]
        public string Description { get; set; }

        /// <summary>
        /// 所属用户.
        /// </summary>
        [Reactive]
        public UserViewModel User { get; set; }

        /// <summary>
        /// 总数.
        /// </summary>
        [Reactive]
        public int TotalCount { get; set; }

        /// <summary>
        /// 是否显示空白.
        /// </summary>
        [Reactive]
        public bool IsShowEmpty { get; set; }

        /// <summary>
        /// 是否为我的收藏夹.
        /// </summary>
        [Reactive]
        public bool IsMine { get; set; }

        /// <summary>
        /// 视频集合.
        /// </summary>
        [Reactive]
        public ObservableCollection<VideoViewModel> VideoCollection { get; set; }
    }
}
