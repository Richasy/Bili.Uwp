// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Bili.Locator.Uwp;
using Bili.Models.App;
using Bili.Models.App.Constants;
using Bili.Models.BiliBili;
using Bili.Models.Data.Local;
using Bili.Models.Enums;
using Bili.ViewModels.Uwp.Account;
using Bilibili.App.View.V1;
using ReactiveUI;
using Splat;

namespace Bili.ViewModels.Uwp
{
    /// <summary>
    /// 用户视图模型，特指非当前登录账户的其它用户.
    /// </summary>
    public partial class UserViewModel : WebRequestViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserViewModel"/> class.
        /// </summary>
        /// <param name="item">用户搜索条目.</param>
        public UserViewModel(UserSearchItem item)
            : this(item.Title, item.Cover, item.UserId)
        {
            item.Title = Regex.Replace(item.Title, "<[^>]+>", string.Empty);
            if (item.Relation != null)
            {
                IsFollow = item.Relation.Status == 2 || item.Relation.Status == 4;
            }

            Sign = item.Sign;
            if (string.IsNullOrEmpty(Sign))
            {
                Sign = _resourceToolkit.GetLocaleString(LanguageNames.UserEmptySign);
            }

            Level = item.Level;
            FollowerCount = _numberToolkit.GetCountText(item.FollowerCount);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserViewModel"/> class.
        /// </summary>
        /// <param name="item">粉丝条目.</param>
        public UserViewModel(RelatedUser item)
            : this(item.Name, item.Avatar, item.Mid)
        {
            IsFollow = item.Attribute != 0;
            Sign = item.Sign;
            if (string.IsNullOrEmpty(Sign))
            {
                Sign = _resourceToolkit.GetLocaleString(LanguageNames.UserEmptySign);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserViewModel"/> class.
        /// </summary>
        /// <param name="info">发布者信息.</param>
        public UserViewModel(PublisherInfo info)
            : this(info.Publisher, info.PublisherAvatar, info.Mid)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserViewModel"/> class.
        /// </summary>
        /// <param name="avatar">发布者信息.</param>
        public UserViewModel(RecommendAvatar avatar)
            : this(avatar.UserName, avatar.Cover, avatar.UserId)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserViewModel"/> class.
        /// </summary>
        /// <param name="staff">参演人员.</param>
        public UserViewModel(Staff staff)
            : this(staff.Name, staff.Face, Convert.ToInt32(staff.Mid))
        {
            AdditionalText = staff.Title;
            IsFollow = staff.Attention == 1;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserViewModel"/> class.
        /// </summary>
        /// <param name="userName">用户名.</param>
        /// <param name="avatar">头像.</param>
        /// <param name="userId">用户Id.</param>
        public UserViewModel(string userName, string avatar = "", int userId = 0)
            : this(userId)
        {
            Name = userName ?? "--";
            Avatar = avatar;
            if (!string.IsNullOrEmpty(avatar) && !avatar.EndsWith("100q.jpg"))
            {
                Avatar = avatar + "@60w_60h_1c_100q.jpg";
            }

            CheckFollowButtonVisibility();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserViewModel"/> class.
        /// </summary>
        /// <param name="userId">用户Id.</param>
        public UserViewModel(int userId)
            : this()
        {
            Id = userId;
            CanFixPublisher = Splat.Locator.Current.GetService<AccountViewModel>().IsConnected && Splat.Locator.Current.GetService<AccountViewModel>().Mid != null;
            if (CanFixPublisher)
            {
                IsPublisherFixed = Splat.Locator.Current.GetService<AccountViewModel>().FixedItemCollection.Any(p => p.Id == userId.ToString());
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserViewModel"/> class.
        /// </summary>
        protected UserViewModel()
            : base()
        {
            ServiceLocator.Instance.LoadService(out _numberToolkit)
                                   .LoadService(out _resourceToolkit);

            VideoCollection = new ObservableCollection<VideoViewModel>();
            SearchCollection = new ObservableCollection<VideoViewModel>();

            this.WhenAnyValue(x => x.IsSearching)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(x =>
                {
                    IsError = false;
                });
        }

        /// <summary>
        /// 切换关注状态.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        public async Task ToggleFollowStateAsync()
        {
            if (_isFollowRequesting)
            {
                return;
            }

            _isFollowRequesting = true;
            await Task.CompletedTask;
            IsFollow = !IsFollow;

            _isFollowRequesting = false;
        }

        /// <summary>
        /// 切换固定状态.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        public async Task ToggleFixStateAsync()
        {
            if (IsPublisherFixed)
            {
                await Splat.Locator.Current.GetService<AccountViewModel>().RemoveFixedItemAsync(Id.ToString());
            }
            else
            {
                var p = new FixedItem
                {
                    Id = Id.ToString(),
                    Cover = Avatar,
                    Title = Name,
                    Type = Models.Enums.App.FixedType.Publisher,
                };

                await Splat.Locator.Current.GetService<AccountViewModel>().AddFixedItemAsync(p);
            }

            IsPublisherFixed = !IsPublisherFixed;
        }

        /// <summary>
        /// 是否为哔哩哔哩番剧出差账户.
        /// </summary>
        /// <returns>检查结果.</returns>
        public bool IsRegionalAnimeUser()
            => Id == AppConstants.RegionalAnimeUserId;

        private void CheckFollowButtonVisibility()
        {
            var accVM = Splat.Locator.Current.GetService<AccountViewModel>();
            var isMe = accVM.State == AuthorizeState.SignedIn && accVM.Mid == Id;
            IsShowFollowButton = accVM.State == AuthorizeState.SignedIn && !isMe;
        }
    }
}
