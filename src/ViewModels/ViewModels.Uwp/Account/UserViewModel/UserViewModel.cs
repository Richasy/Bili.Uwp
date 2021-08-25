// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Richasy.Bili.Locator.Uwp;
using Richasy.Bili.Models.App.Args;
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
            CheckFollowButtonVisibility();
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

            CheckFollowButtonVisibility();
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
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserViewModel"/> class.
        /// </summary>
        /// <param name="userId">用户Id.</param>
        public UserViewModel(int userId)
            : this()
        {
            Id = userId;
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
        }

        /// <summary>
        /// 激活.
        /// </summary>
        public void Active()
        {
            Controller.UserSpaceVideoIteration -= OnUserSpaceVideoIteration;
            Controller.UserSpaceVideoIteration += OnUserSpaceVideoIteration;
        }

        /// <summary>
        /// 休眠.
        /// </summary>
        public void Deactive()
        {
            Controller.UserSpaceVideoIteration -= OnUserSpaceVideoIteration;
        }

        /// <summary>
        /// 初始化用户资料.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        public async Task InitializeUserDetailAsync()
        {
            if (!IsInitializeLoading && !IsDeltaLoading)
            {
                Reset();
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
                    IsInitializeLoading = false;
                    return;
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
            if (!IsDeltaLoading && !_isVideoLoadCompleted)
            {
                IsDeltaLoading = true;
                await Controller.RequestUserSpaceVideoSetAsync(Id, _videoOffsetId);
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
        /// 清除数据.
        /// </summary>
        public void Reset()
        {
            VideoCollection.Clear();
            IsRequested = false;
            _isFollowRequesting = false;
            _isVideoLoadCompleted = false;
        }

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
    }
}
