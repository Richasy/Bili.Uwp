// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Bili.Models.App.Args;
using Bili.Models.App.Other;
using Bili.Models.BiliBili;
using Bili.Models.Enums;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Bili.ViewModels.Uwp
{
    /// <summary>
    /// 我的关注视图模型.
    /// </summary>
    public sealed class MyFollowingViewModel : WebRequestViewModelBase
    {
        private int _pageNumber;
        private bool _isLoadCompleted;

        /// <summary>
        /// Initializes a new instance of the <see cref="MyFollowingViewModel"/> class.
        /// </summary>
        private MyFollowingViewModel()
        {
            FollowingTagCollection = new ObservableCollection<RelatedTag>();
            DisplayFollowingUserCollection = new ObservableCollection<UserViewModel>();
            Controller.FollowsIteration += OnFollowsIteration;

            this.WhenAnyValue(x => x.CurrentTag)
                .ObserveOn(RxApp.MainThreadScheduler)
                .WhereNotNull()
                .Subscribe(async p =>
                {
                    await InitializeRequestAsync();
                });
        }

        /// <summary>
        /// 单例.
        /// </summary>
        public static MyFollowingViewModel Instance { get; } = new Lazy<MyFollowingViewModel>(() => new MyFollowingViewModel()).Value;

        /// <summary>
        /// 我的关注分组.
        /// </summary>
        public ObservableCollection<RelatedTag> FollowingTagCollection { get; }

        /// <summary>
        /// 显示的关注列表.
        /// </summary>
        public ObservableCollection<UserViewModel> DisplayFollowingUserCollection { get; }

        /// <summary>
        /// 当前分组.
        /// </summary>
        [Reactive]
        public RelatedTag CurrentTag { get; set; }

        /// <summary>
        /// 是否显示空白占位符.
        /// </summary>
        [Reactive]
        public bool IsShowEmpty { get; set; }

        /// <summary>
        /// 用户名.
        /// </summary>
        [Reactive]
        public string UserName { get; set; }

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
        /// 初始化分组.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        public async Task InitializeTagsAsync()
        {
            CurrentTag = null;
            FollowingTagCollection.Clear();
            var tags = await Controller.GetMyFollowingTagsAsync();
            tags.ForEach(p => FollowingTagCollection.Add(p));
            CurrentTag = FollowingTagCollection.FirstOrDefault(p => p.Count > 0);
        }

        /// <summary>
        /// 执行初始请求.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        public async Task InitializeRequestAsync()
        {
            UserName = AccountViewModel.Instance.DisplayName;

            if (!IsInitializeLoading && !IsDeltaLoading)
            {
                IsInitializeLoading = true;
                _isLoadCompleted = false;
                DisplayFollowingUserCollection.Clear();
                _pageNumber = 0;
                IsError = false;
                ErrorText = string.Empty;
                try
                {
                    await Controller.GetMyFollowingTagDetailAsync(CurrentTag.TagId, 1);
                }
                catch (ServiceException ex)
                {
                    IsError = true;
                    var msg = ResourceToolkit.GetLocaleString(LanguageNames.RequestFollowsFailed);
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
                await Controller.GetMyFollowingTagDetailAsync(CurrentTag.TagId, _pageNumber);
                IsDeltaLoading = false;
            }
        }

        private void OnFollowsIteration(object sender, RelatedUserIterationEventArgs e)
        {
            if (e.UserId == -1)
            {
                if (e.List?.Any() ?? false)
                {
                    foreach (var item in e.List)
                    {
                        if (!DisplayFollowingUserCollection.Any(p => p.Id == item.Mid))
                        {
                            DisplayFollowingUserCollection.Add(new UserViewModel(item)
                            {
                                IsFollow = true,
                            });
                        }
                    }
                }

                _pageNumber = e.NextPageNumber;
                _isLoadCompleted = CurrentTag.Count <= DisplayFollowingUserCollection.Count;
                IsShowEmpty = DisplayFollowingUserCollection.Count == 0;
            }
        }
    }
}
