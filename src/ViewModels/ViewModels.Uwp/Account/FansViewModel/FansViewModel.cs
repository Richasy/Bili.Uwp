// Copyright (c) Richasy. All rights reserved.

using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Richasy.Bili.Models.App.Args;
using Richasy.Bili.Models.App.Other;
using Richasy.Bili.Models.Enums;

namespace Richasy.Bili.ViewModels.Uwp
{
    /// <summary>
    /// 粉丝视图模型.
    /// </summary>
    public partial class FansViewModel : WebRequestViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FansViewModel"/> class.
        /// </summary>
        public FansViewModel()
        {
            FansCollection = new ObservableCollection<UserViewModel>();
            Controller.FansIteration += OnFansIteration;
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
                var isMe = userId == (AccountViewModel.Instance.Mid ?? 0);
                _maxFansNumber = isMe ? 1000 : 100;
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
                FansCollection.Clear();
                _pageNumber = 0;
                IsError = false;
                ErrorText = string.Empty;
                try
                {
                    await Controller.RequestUserFollowersAsync(_currentUserId, _pageNumber);
                }
                catch (ServiceException ex)
                {
                    IsError = true;
                    ErrorText = $"{ResourceToolkit.GetLocaleString(LanguageNames.RequestFansFailed)}\n{ex.Error?.Message ?? ex.Message}";
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
                await Controller.RequestUserFollowersAsync(_currentUserId, _pageNumber);
                IsDeltaLoading = false;
            }
        }

        private void OnFansIteration(object sender, FansIterationEventArgs e)
        {
            if (e.UserId == _currentUserId)
            {
                if (e.List?.Any() ?? false)
                {
                    e.List.ForEach(p => FansCollection.Add(new UserViewModel(p)));
                }

                _pageNumber = e.NextPageNumber;
                _isLoadCompleted = e.TotalCount <= FansCollection.Count || FansCollection.Count >= _maxFansNumber;
                IsShowEmpty = FansCollection.Count == 0;
            }
        }
    }
}
