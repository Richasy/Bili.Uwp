// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Bili.Models.App.Args;
using Bili.Models.App.Other;
using Bili.Models.Enums;
using ReactiveUI.Fody.Helpers;

namespace Bili.ViewModels.Uwp
{
    /// <summary>
    /// PGC内容基础视图模型.
    /// </summary>
    public class PgcViewModelBase : WebRequestViewModelBase
    {
        private int _indexPageNumber;
        private bool _isIndexLoadCompleted;

        /// <summary>
        /// Initializes a new instance of the <see cref="PgcViewModelBase"/> class.
        /// </summary>
        /// <param name="type">PGC内容类型.</param>
        public PgcViewModelBase(PgcType type)
        {
            Type = type;
            FilterCollection = new ObservableCollection<PgcConditionViewModel>();
            ItemCollection = new ObservableCollection<SeasonViewModel>();
            switch (type)
            {
                case PgcType.Bangumi:
                    PgcName = ResourceToolkit.GetLocaleString(LanguageNames.Bangumi);
                    break;
                case PgcType.Domestic:
                    PgcName = ResourceToolkit.GetLocaleString(LanguageNames.DomesticAnime);
                    break;
                case PgcType.Movie:
                    PgcName = ResourceToolkit.GetLocaleString(LanguageNames.Movie);
                    break;
                case PgcType.Documentary:
                    PgcName = ResourceToolkit.GetLocaleString(LanguageNames.Documentary);
                    break;
                case PgcType.TV:
                    PgcName = ResourceToolkit.GetLocaleString(LanguageNames.TV);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// PGC类型.
        /// </summary>
        public PgcType Type { get; set; }

        /// <summary>
        /// PGC模块名.
        /// </summary>
        [Reactive]
        public string PgcName { get; set; }

        /// <summary>
        /// 筛选条件集合.
        /// </summary>
        [Reactive]
        public ObservableCollection<PgcConditionViewModel> FilterCollection { get; set; }

        /// <summary>
        /// 条目集合.
        /// </summary>
        [Reactive]
        public ObservableCollection<SeasonViewModel> ItemCollection { get; set; }

        /// <summary>
        /// 是否正在请求排序条件.
        /// </summary>
        [Reactive]
        public bool IsIndexInitializeLoading { get; set; }

        /// <summary>
        /// 是否在请求索引的过程中出错.
        /// </summary>
        [Reactive]
        public bool IsIndexError { get; set; }

        /// <summary>
        /// 索引查询失败的显示文本.
        /// </summary>
        [Reactive]
        public string IndexErrorText { get; set; }

        /// <summary>
        /// 索引是否在增量加载.
        /// </summary>
        [Reactive]
        public bool IsIndexDeltaLoading { get; set; }

        /// <summary>
        /// 索引是否已经请求.
        /// </summary>
        [Reactive]
        public bool IsIndexRequested { get; set; }

        /// <summary>
        /// 是否显示为空白.
        /// </summary>
        [Reactive]
        public bool IsEmpty { get; set; }

        /// <summary>
        /// 加载索引.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        public async Task LoadIndexAsync()
        {
            if (IsIndexInitializeLoading)
            {
                return;
            }

            IsIndexInitializeLoading = true;
            IsIndexError = false;
            if (FilterCollection.Count == 0)
            {
                try
                {
                    await Task.CompletedTask;
                }
                catch (ServiceException ex)
                {
                    IsIndexError = true;
                    IndexErrorText = $"{ResourceToolkit.GetLocaleString(LanguageNames.RequestIndexFilterFailed)}\n{ex.Error?.Message ?? ex.Message}";
                }
                catch (InvalidOperationException invalidEx)
                {
                    IsIndexError = true;
                    IndexErrorText = invalidEx.Message;
                }
            }

            ItemCollection.Clear();
            _indexPageNumber = 1;
            _isIndexLoadCompleted = false;
            IsIndexRequested = false;

            try
            {
                await RequestIndexRequestAsync();
                IsIndexRequested = true;
            }
            catch (ServiceException ex)
            {
                IsIndexError = true;
                IndexErrorText = $"{ResourceToolkit.GetLocaleString(LanguageNames.RequestIndexResultFailed)}\n{ex.Error?.Message ?? ex.Message}";
            }
            catch (Exception invalidEx)
            {
                IsIndexError = true;
                IndexErrorText = invalidEx.Message;
            }

            IsIndexInitializeLoading = false;
        }

        /// <summary>
        /// 增量请求索引结果.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        public async Task DeltaRequestIndexAsync()
        {
            if (!IsIndexDeltaLoading && !_isIndexLoadCompleted)
            {
                IsIndexDeltaLoading = true;
                await RequestIndexRequestAsync();
                IsIndexDeltaLoading = false;
            }
        }

        private async Task RequestIndexRequestAsync()
        {
            var queryPrameters = new Dictionary<string, string>();
            foreach (var condition in FilterCollection)
            {
                if (condition.SelectedItem != null)
                {
                    var id = condition.SelectedItem.Id;
                    if (condition.Id == "year")
                    {
                        id = Uri.EscapeDataString(condition.SelectedItem.Id);
                    }

                    queryPrameters.Add(condition.Id, id);
                }
            }

            await Task.CompletedTask;
        }

        private void OnPgcIndexResultIteration(object sender, PgcIndexResultIterationEventArgs e)
        {
            if (e.Type == Type)
            {
                if (e.List != null)
                {
                    e.List.ForEach(p => ItemCollection.Add(new SeasonViewModel(p)));
                }

                _isIndexLoadCompleted = !e.HasNext || e.TotalCount <= ItemCollection.Count;
                _indexPageNumber = e.NextPageNumber;
                IsEmpty = ItemCollection.Count == 0;
            }
        }
    }
}
