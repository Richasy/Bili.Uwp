// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Bilibili.App.Dynamic.V2;
using Richasy.Bili.Models.App.Args;
using Richasy.Bili.Models.App.Other;
using Richasy.Bili.Models.Enums;

namespace Richasy.Bili.ViewModels.Uwp
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
            DynamicCollection = new ObservableCollection<DynamicItem>();
            Controller.DynamicVideoIteration += OnDynamicVideoIteration;
            Controller.Logged += OnLoggedAsync;
            Controller.LoggedOut += OnLoggedOut;
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
                    await Controller.RequestDynamicVideoListAsync(_updateOffset, _baseLine);
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
            if (!IsDeltaLoading && !_isLoadCompleted)
            {
                IsDeltaLoading = true;
                await Controller.RequestDynamicVideoListAsync(_updateOffset, _baseLine);
                IsDeltaLoading = false;
            }
        }

        private void Reset()
        {
            DynamicCollection.Clear();
            _updateOffset = string.Empty;
            _baseLine = string.Empty;
            _isLoadCompleted = false;
            IsShowLogin = false;
            IsError = false;
        }

        private void OnDynamicVideoIteration(object sender, DynamicVideoIterationEventArgs e)
        {
            _isLoadCompleted = !e.HasMore;
            _baseLine = e.BaseLine;
            _updateOffset = e.UpdateOffset;

            if (e.List != null && e.List.Count > 0)
            {
                foreach (var item in e.List)
                {
                    if (!DynamicCollection.Any(p => p.Extend.DynIdStr == item.Extend.DynIdStr))
                    {
                        DynamicCollection.Add(item);
                    }
                }
            }

            IsShowEmpty = DynamicCollection.Count == 0;
        }

        private async void OnLoggedAsync(object sender, EventArgs e)
        {
            await InitializeRequestAsync();
        }

        private void OnLoggedOut(object sender, EventArgs e)
        {
            Reset();
            IsShowLogin = true;
        }
    }
}
