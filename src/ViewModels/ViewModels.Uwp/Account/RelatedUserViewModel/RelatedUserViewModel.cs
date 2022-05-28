// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Bili.Models.App.Args;
using Bili.Models.App.Other;
using Bili.Models.Enums;
using Bili.Models.Enums.App;
using Bili.ViewModels.Uwp.Account;
using Splat;

namespace Bili.ViewModels.Uwp
{
    /// <summary>
    /// 相关用户（粉丝，关注）的视图模型.
    /// </summary>
    public partial class RelatedUserViewModel : WebRequestViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FansViewModel"/> class.
        /// </summary>
        /// <param name="type">模型类型.</param>
        protected RelatedUserViewModel(RelatedUserType type)
        {
            UserCollection = new ObservableCollection<UserViewModel>();
            _type = type;

            if (type == RelatedUserType.Fans)
            {
                Controller.FansIteration += OnUsersIteration;
            }
            else
            {
                Controller.FollowsIteration += OnUsersIteration;
            }
        }

        /// <summary>
        /// 设置待请求的用户信息.
        /// </summary>
        /// <param name="userId">用户Id.</param>
        /// <param name="userName">用户名.</param>
        /// <returns>是否可以刷新数据.</returns>
        public bool SetUser(int userId, string userName)
        {
            if (userId != _currentUserId)
            {
                _currentUserId = userId;
                UserName = userName;
                IsRequested = false;
                var isMe = userId == (Splat.Locator.Current.GetService<AccountViewModel>().Mid ?? 0);
                _maxQueryNumber = isMe ? 1000 : 100;
                return true;
            }

            return false;
        }

        /// <summary>
        /// 请求数据.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        public async Task RequestDataAsync()
        {
            if (IsRequested)
            {
                await DeltaRequestAsync();
            }
            else
            {
                await InitializeRequestAsync();
            }

            IsRequested = _pageNumber >= 1;
        }

        /// <summary>
        /// 执行初始请求.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        public async Task InitializeRequestAsync()
        {
            if (!IsInitializeLoading && !IsDeltaLoading)
            {
                IsInitializeLoading = true;
                _isLoadCompleted = false;
                UserCollection.Clear();
                _pageNumber = 0;
                IsError = false;
                ErrorText = string.Empty;
                try
                {
                    if (_type == RelatedUserType.Fans)
                    {
                        await Controller.RequestUserFollowersAsync(_currentUserId, 1);
                    }
                    else
                    {
                        await Controller.RequestUserFollowsAsync(_currentUserId, 1);
                    }
                }
                catch (ServiceException ex)
                {
                    IsError = true;
                    var msg = _type == RelatedUserType.Fans ?
                        ResourceToolkit.GetLocaleString(LanguageNames.RequestFansFailed) :
                        ResourceToolkit.GetLocaleString(LanguageNames.RequestFollowsFailed);
                    ErrorText = $"{msg}\n{ex.Error?.Message ?? ex.Message}";
                }
                catch (Exception e)
                {
                    IsError = true;
                    ErrorText = $"{e.Message}";
                }

                IsInitializeLoading = false;
            }

            IsRequested = _pageNumber != 0;
        }

        /// <summary>
        /// 执行增量请求.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        internal async Task DeltaRequestAsync()
        {
            if (!IsDeltaLoading && !_isLoadCompleted)
            {
                IsDeltaLoading = true;
                if (_type == RelatedUserType.Fans)
                {
                    await Controller.RequestUserFollowersAsync(_currentUserId, _pageNumber);
                }
                else
                {
                    await Controller.RequestUserFollowsAsync(_currentUserId, _pageNumber);
                }

                IsDeltaLoading = false;
            }
        }

        private void OnUsersIteration(object sender, RelatedUserIterationEventArgs e)
        {
            if (e.UserId == _currentUserId)
            {
                if (e.List?.Any() ?? false)
                {
                    e.List.ForEach(p => UserCollection.Add(new UserViewModel(p)));
                }

                _pageNumber = e.NextPageNumber;
                _isLoadCompleted = e.TotalCount <= UserCollection.Count || UserCollection.Count >= _maxQueryNumber;
                IsShowEmpty = UserCollection.Count == 0;
            }
        }
    }
}
