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
            : this()
        {
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

            IsShowFollowButton = AccountViewModel.Instance.Status == AccountViewModelStatus.Login;
            VideoCollection = new ObservableCollection<VideoViewModel>();
            Controller.UserSpaceVideoIteration += OnUserSpaceVideoIteration;
        }

        /// <summary>
        /// 初始化用户资料.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        public async Task InitializeUserDetailAsync()
        {
            if (!IsInitializeLoading && !IsDeltaLoading)
            {
                VideoCollection.Clear();
                IsInitializeLoading = true;

                try
                {
                    _detail = await Controller.RequestUserSpaceInformationAsync(Id);
                    InitializeUserInformation();
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

        private void InitializeUserInformation()
        {
            Name = _detail.UserName;
            Avatar = _detail.Avatar;
            Sign = _detail.Sign;
            FollowCount = _numberToolkit.GetCountText(_detail.FollowCount);
            FollowerCount = _numberToolkit.GetCountText(_detail.FollowerCount);
            LikeCount = _numberToolkit.GetCountText(_detail.LikeInformation.LikeCount);
            Level = _detail.LevelInformation.CurrentLevel;

            if (_detail.Relation != null)
            {
                IsFollow = _detail.Relation.Status == 2 || _detail.Relation.Status == 4;
            }
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
        }
    }
}
