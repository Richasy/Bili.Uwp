// Copyright (c) GodLeaveMe. All rights reserved.

using System.Collections.ObjectModel;
using ReactiveUI.Fody.Helpers;
using Richasy.Bili.Models.Enums.App;

namespace Richasy.Bili.ViewModels.Uwp
{
    /// <summary>
    /// 相关用户的视图模型.
    /// </summary>
    public partial class RelatedUserViewModel
    {
        private readonly RelatedUserType _type;

        private int _pageNumber;
        private int _currentUserId;
        private bool _isLoadCompleted;
        private double _maxQueryNumber;

        /// <summary>
        /// 查询的用户名.
        /// </summary>
        [Reactive]
        public string UserName { get; set; }

        /// <summary>
        /// 用户集合.
        /// </summary>
        public ObservableCollection<UserViewModel> UserCollection { get; set; }

        /// <summary>
        /// 是否显示空白占位符.
        /// </summary>
        [Reactive]
        public bool IsShowEmpty { get; set; }
    }
}
