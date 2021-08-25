// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using ReactiveUI.Fody.Helpers;

namespace Richasy.Bili.ViewModels.Uwp
{
    /// <summary>
    /// 粉丝视图模型.
    /// </summary>
    public partial class FansViewModel
    {
        private int _pageNumber;
        private int _currentUserId;
        private bool _isLoadCompleted;
        private double _maxFansNumber;

        /// <summary>
        /// 单例.
        /// </summary>
        public static FansViewModel Instance { get; } = new Lazy<FansViewModel>(() => new FansViewModel()).Value;

        /// <summary>
        /// 查询的用户名.
        /// </summary>
        [Reactive]
        public string UserName { get; set; }

        /// <summary>
        /// 粉丝集合.
        /// </summary>
        public ObservableCollection<UserViewModel> FansCollection { get; set; }

        /// <summary>
        /// 是否显示空白占位符.
        /// </summary>
        [Reactive]
        public bool IsShowEmpty { get; set; }
    }
}
