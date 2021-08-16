// Copyright (c) Richasy. All rights reserved.

using Richasy.Bili.Locator.Uwp;
using Richasy.Bili.Models.BiliBili;

namespace Richasy.Bili.ViewModels.Uwp
{
    /// <summary>
    /// 用户视图模型，特指非当前登录账户的其它用户.
    /// </summary>
    public partial class UserViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserViewModel"/> class.
        /// </summary>
        /// <param name="item">用户搜索条目.</param>
        public UserViewModel(UserSearchItem item)
            : this()
        {
            IsShowFollowButton = AccountViewModel.Instance.Status == AccountViewModelStatus.Login;
            if (item.Relation != null)
            {
                IsFollow = item.Relation.Status == 2 || item.Relation.Status == 4;
            }

            Name = item.Title;
            Sign = item.Sign;
            if (string.IsNullOrEmpty(Sign))
            {
                Sign = _resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.UserEmptySign);
            }

            Level = item.Level;
            FollowerCount = _numberToolkit.GetCountText(item.FollowerCount);
            Avatar = item.Cover + "@60w_60h_1c_100q.jpg";
            Id = item.UserId;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserViewModel"/> class.
        /// </summary>
        protected UserViewModel()
        {
            ServiceLocator.Instance.LoadService(out _numberToolkit)
                                   .LoadService(out _resourceToolkit);
        }
    }
}
