﻿// Copyright (c) Richasy. All rights reserved.

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
        }

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
            if (!IsInitializeLoading)
            {
                IsInitializeLoading = true;
                Reset();
                try
                {
                    await Controller.RequestDynamicVideoListAsync(_pageNumber, _updateOffset, _baseLine);
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
                await Controller.RequestDynamicVideoListAsync(_pageNumber, _updateOffset, _baseLine);
                IsDeltaLoading = false;
            }
        }

        private void Reset()
        {
            DynamicCollection.Clear();
            _pageNumber = 1;
            _updateOffset = string.Empty;
            _baseLine = string.Empty;
            _isLoadCompleted = false;
        }

        private void OnDynamicVideoIteration(object sender, DynamicVideoIterationEventArgs e)
        {
            _pageNumber = e.NextPageNumebr;
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
    }
}
