// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using ReactiveUI.Fody.Helpers;
using Richasy.Bili.Models.App.Args;
using Richasy.Bili.Models.App.Other;
using Richasy.Bili.Models.Enums;

namespace Richasy.Bili.ViewModels.Uwp
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
            Controller.PgcIndexResultIteration += OnPgcIndexResultIteration;
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
                    var conditions = await Controller.GetPgcIndexConditionsAsync(Type);
                    FilterCollection.Add(new PgcConditionViewModel(conditions.OrderList));
                    conditions.FilterList.ForEach(p => FilterCollection.Add(new PgcConditionViewModel(p)));
                    if (Type == PgcType.Bangumi || Type == PgcType.Domestic)
                    {
                        var areaCondition = FilterCollection.Where(p => p.Id == "area").FirstOrDefault();

                        // 1,6,7国产，2日本.
                        var areaId = Type == PgcType.Bangumi ? "2" : "1,";
                        var areaItem = areaCondition.ConditionCollection.Where(p => p.Id.Contains(areaId)).FirstOrDefault();
                        areaCondition.SelectedItem = areaItem;
                    }
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
                    queryPrameters.Add(condition.Id, condition.SelectedItem.Id);
                }
            }

            await Controller.RequestPgcIndexResultAsync(Type, _indexPageNumber, queryPrameters);
        }

        private void OnPgcIndexResultIteration(object sender, PgcIndexResultIterationEventArgs e)
        {
            if (e.Type == Type)
            {
                e.List.ForEach(p => ItemCollection.Add(SeasonViewModel.CreateFromIndexItem(p)));

                _isIndexLoadCompleted = !e.HasNext || e.TotalCount <= ItemCollection.Count;
                _indexPageNumber = e.NextPageNumber;
            }
        }
    }
}
