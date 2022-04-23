// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Bilibili.App.View.V1;
using ReactiveUI;
using Richasy.Bili.Locator.Uwp;
using Richasy.Bili.Models.App;
using Richasy.Bili.Models.App.Args;
using Richasy.Bili.Models.App.Constants;
using Richasy.Bili.Models.BiliBili;
using Richasy.Bili.Models.Enums;

namespace Richasy.Bili.ViewModels.Uwp
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
            if (!string.IsNullOrEmpty(avatar))
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
            CanFixPublisher = AccountViewModel.Instance.IsConnected && AccountViewModel.Instance.Mid != null;
            if (CanFixPublisher)
            {
                IsPublisherFixed = AccountViewModel.Instance.FixedItemCollection.Any(p => p.Id == userId.ToString());
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
        /// 激活.
        /// </summary>
        public void Active()
        {
            Controller.UserSpaceVideoIteration -= OnUserSpaceVideoIteration;
            Controller.UserSpaceVideoIteration += OnUserSpaceVideoIteration;
            Controller.UserSpaceSearchVideoIteration -= OnUserSpaceSearchVideoIteration;
            Controller.UserSpaceSearchVideoIteration += OnUserSpaceSearchVideoIteration;
        }

        /// <summary>
        /// 休眠.
        /// </summary>
        public void Deactive()
        {
            Controller.UserSpaceVideoIteration -= OnUserSpaceVideoIteration;
            Controller.UserSpaceSearchVideoIteration -= OnUserSpaceSearchVideoIteration;
        }

        /// <summary>
        /// 初始化用户资料.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        public async Task InitializeUserDetailAsync()
        {
            if (!IsInitializeLoading && !IsDeltaLoading && !IsSearching)
            {
                ResetArchives();
                Active();
                IsInitializeLoading = true;

                try
                {
                    _detail = await Controller.RequestUserSpaceInformationAsync(Id);
                    InitializeUserInformation();
                    IsShowVideoEmpty = VideoCollection.Count == 0;
                    IsRequested = true;
                }
                catch (Exception ex)
                {
                    IsError = true;
                    ErrorText = _resourceToolkit.GetLocaleString(LanguageNames.RequestUserInformationFailed) + $"\n{ex.Message}";
                }

                IsInitializeLoading = false;
            }
        }

        /// <summary>
        /// 执行增量请求.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        public async Task DeltaRequestVideoAsync()
        {
            if (!IsDeltaLoading && !_isVideoLoadCompleted && !IsSearching)
            {
                IsDeltaLoading = true;
                await Controller.RequestUserSpaceVideoSetAsync(Id, _videoOffsetId);
                IsDeltaLoading = false;
            }
        }

        /// <summary>
        /// 初始化搜索结果.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        public async Task InitializeSearchResultAsync()
        {
            if (!string.IsNullOrEmpty(SearchKeyword.Trim())
                && !IsInitializeLoading
                && !IsDeltaLoading
                && IsSearching)
            {
                ResetSearch();
                Active();

                IsInitializeLoading = true;
                try
                {
                    await Controller.RequestSearchUserVideoAsync(Id, SearchKeyword, _searchPageNumber);
                }
                catch (Exception ex)
                {
                    IsError = true;
                    ErrorText = _resourceToolkit.GetLocaleString(LanguageNames.RequestSearchResultFailed) + $"\n{ex.Message}";
                }

                IsInitializeLoading = false;
            }
        }

        /// <summary>
        /// 执行增量请求.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        public async Task DeltaRequestSearchAsync()
        {
            if (!IsDeltaLoading
                && !_isSearchLoadCompleted
                && !string.IsNullOrEmpty(SearchKeyword)
                && IsSearching)
            {
                IsDeltaLoading = true;
                await Controller.RequestSearchUserVideoAsync(Id, SearchKeyword, _searchPageNumber);
                IsDeltaLoading = false;
            }
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
            var result = await Controller.ModifyUserRelationAsync(Id, !IsFollow);
            if (result)
            {
                IsFollow = !IsFollow;
            }

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
                await AccountViewModel.Instance.RemoveFixedItemAsync(Id.ToString());
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

                await AccountViewModel.Instance.AddFixedItemAsync(p);
            }

            IsPublisherFixed = !IsPublisherFixed;
        }

        /// <summary>
        /// 清除空间数据.
        /// </summary>
        public void ResetArchives()
        {
            VideoCollection.Clear();
            IsRequested = false;
            IsError = false;
            _videoOffsetId = string.Empty;
            _isFollowRequesting = false;
            _isVideoLoadCompleted = false;
            SearchKeyword = string.Empty;
        }

        /// <summary>
        /// 清除搜索数据.
        /// </summary>
        public void ResetSearch()
        {
            SearchCollection.Clear();
            _searchPageNumber = 1;
            _isSearchLoadCompleted = false;
            IsShowSearchEmpty = false;
        }

        /// <summary>
        /// 是否为哔哩哔哩番剧出差账户.
        /// </summary>
        /// <returns>检查结果.</returns>
        public bool IsRegionalAnimeUser()
            => Id == AppConstants.RegionalAnimeUserId;

        private void InitializeUserInformation()
        {
            Name = _detail.UserName;
            Avatar = _detail.Avatar;
            Sign = _detail.Sign;
            FollowCount = _numberToolkit.GetCountText(_detail.FollowCount);
            FollowerCount = _numberToolkit.GetCountText(_detail.FollowerCount);
            LikeCount = _numberToolkit.GetCountText(_detail.LikeInformation.LikeCount);
            Level = _detail.LevelInformation.CurrentLevel;
            if (string.IsNullOrEmpty(Sign))
            {
                Sign = _resourceToolkit.GetLocaleString(LanguageNames.UserEmptySign);
            }

            if (_detail.Relation != null)
            {
                IsFollow = _detail.Relation.Status == 2 || _detail.Relation.Status == 4;
            }

            CheckFollowButtonVisibility();
        }

        private void CheckFollowButtonVisibility()
        {
            var accVM = AccountViewModel.Instance;
            var isMe = accVM.Status == AccountViewModelStatus.Login && accVM.Mid == Id;
            IsShowFollowButton = accVM.Status == AccountViewModelStatus.Login && !isMe;
        }

        private void OnUserSpaceVideoIteration(object sender, UserSpaceVideoIterationEventArgs e)
        {
            if (e.UserId != Id)
            {
                return;
            }

            foreach (var item in e.List)
            {
                if (!VideoCollection.Any(p => p.VideoId == item.Id))
                {
                    VideoCollection.Add(new VideoViewModel(item));
                }
            }

            _videoOffsetId = e.NextOffsetId;
            _isVideoLoadCompleted = VideoCollection.Count >= e.TotalCount;
            IsShowVideoEmpty = VideoCollection.Count == 0;
        }

        private void OnUserSpaceSearchVideoIteration(object sender, UserSpaceSearchVideoIterationEventArgs e)
        {
            if (e.UserId != Id)
            {
                return;
            }

            foreach (var item in e.List)
            {
                if (!SearchCollection.Any(p => p.VideoId == item.Archive.Aid.ToString()))
                {
                    SearchCollection.Add(new VideoViewModel(item));
                }
            }

            _searchPageNumber += 1;
            _isSearchLoadCompleted = SearchCollection.Count >= e.TotalCount;
            IsShowSearchEmpty = SearchCollection.Count == 0;
        }
    }
}
