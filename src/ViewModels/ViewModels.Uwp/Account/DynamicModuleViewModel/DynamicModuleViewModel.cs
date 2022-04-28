// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Bili.Models.App.Args;
using Bili.Models.App.Other;
using Bili.Models.Enums;
using Bilibili.App.Dynamic.V2;

namespace Bili.ViewModels.Uwp
{
    /// <summary>
    /// 动态模块视图模型.
    /// </summary>
    public partial class DynamicModuleViewModel : WebRequestViewModelBase, IDeltaRequestViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicModuleViewModel"/> class.
        /// </summary>
        protected DynamicModuleViewModel()
        {
            VideoDynamicCollection = new ObservableCollection<DynamicItem>();
            AllDynamicCollection = new ObservableCollection<DynamicItem>();
            Controller.DynamicVideoIteration += OnDynamicVideoIteration;
            Controller.Logged += OnLoggedAsync;
            Controller.LoggedOut += OnLoggedOut;
            IsVideo = true;
        }

        /// <summary>
        /// 点赞动态.
        /// </summary>
        /// <param name="item">条目.</param>
        /// <param name="isLike">是否点赞.</param>
        /// <returns>是否操作成功.</returns>
        public Task<bool> LikeDynamicAsync(DynamicItem item, bool isLike)
            => Controller.LikeDynamicAsync(item.Extend.DynIdStr, isLike, item.Extend.Uid, item.Extend.BusinessId);

        /// <inheritdoc/>
        public async Task RequestDataAsync()
        {
            if (!IsRequested)
            {
                await InitializeRequestAsync();
            }
            else
            {
                await DeltaRequestAsync();
            }
        }

        /// <inheritdoc/>
        public async Task InitializeRequestAsync()
        {
            if (!AccountViewModel.Instance.IsConnected)
            {
                Reset();
                IsShowLogin = true;
                return;
            }

            if (!IsInitializeLoading)
            {
                IsInitializeLoading = true;
                Reset();
                try
                {
                    await RequestAsync();
                    IsRequested = true;
                }
                catch (ServiceException ex)
                {
                    IsError = true;
                    ErrorText = $"{ResourceToolkit.GetLocaleString(LanguageNames.RequestDynamicFailed)}\n{ex.Error?.Message ?? ex.Message}";
                }
                catch (Exception invalidEx)
                {
                    IsError = true;
                    ErrorText = invalidEx.Message;
                }

                IsInitializeLoading = false;
            }
        }

        internal async Task DeltaRequestAsync()
        {
            if (!IsDeltaLoading && !_isLoadCompleted && AccountViewModel.Instance.Status == AccountViewModelStatus.Login)
            {
                IsDeltaLoading = true;
                await RequestAsync();
                IsDeltaLoading = false;
            }
        }

        private async Task RequestAsync()
        {
            if (IsVideo)
            {
                await Controller.RequestDynamicVideoListAsync(_videoUpdateOffset, _videoBaseLine);
            }
            else
            {
                await Controller.RequestDynamicComprehensiveListAsync(_allUpdateOffset, _allBaseLine);
            }
        }

        private void Reset()
        {
            VideoDynamicCollection.Clear();
            _videoUpdateOffset = string.Empty;
            _videoBaseLine = string.Empty;
            _allUpdateOffset = string.Empty;
            _allBaseLine = string.Empty;
            _isLoadCompleted = false;
            IsShowLogin = false;
            IsError = false;
        }

        private void OnDynamicVideoIteration(object sender, DynamicVideoIterationEventArgs e)
        {
            _isLoadCompleted = !e.HasMore;

            if (e.IsComprehensive)
            {
                _allBaseLine = e.BaseLine;
                _allUpdateOffset = e.UpdateOffset;
            }
            else
            {
                _videoBaseLine = e.BaseLine;
                _videoUpdateOffset = e.UpdateOffset;
            }

            if (e.List != null && e.List.Count > 0)
            {
                foreach (var item in e.List)
                {
                    if (e.IsComprehensive)
                    {
                        if (!AllDynamicCollection.Any(p => p.Extend.DynIdStr == item.Extend.DynIdStr))
                        {
                            AllDynamicCollection.Add(item);
                        }
                    }
                    else
                    {
                        if (!VideoDynamicCollection.Any(p => p.Extend.DynIdStr == item.Extend.DynIdStr))
                        {
                            VideoDynamicCollection.Add(item);
                        }
                    }
                }
            }

            IsShowEmpty = IsVideo
                ? VideoDynamicCollection.Count == 0
                : AllDynamicCollection.Count == 0;
        }

        private async void OnLoggedAsync(object sender, EventArgs e) => await InitializeRequestAsync();

        private void OnLoggedOut(object sender, EventArgs e)
        {
            Reset();
            IsShowLogin = true;
        }
    }
}
