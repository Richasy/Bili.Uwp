// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Richasy.Bili.Controller.Uwp;
using Richasy.Bili.Models.App.Args;
using Richasy.Bili.Models.BiliBili;
using Richasy.Bili.Models.Enums;

namespace Richasy.Bili.ViewModels.Uwp
{
    /// <summary>
    /// 搜索视图模型.
    /// </summary>
    public partial class SearchModuleViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SearchModuleViewModel"/> class.
        /// </summary>
        internal SearchModuleViewModel()
        {
            Controller = BiliController.Instance;
            HotSearchCollection = new ObservableCollection<SearchRecommendItem>();
            LoadModule();
        }

        /// <summary>
        /// 加载热搜列表.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        public async Task LoadHostSearchAsync()
        {
            if (HotSearchCollection.Count == 0)
            {
                try
                {
                    var list = await Controller.GetHotSearchListAsync();
                    if (list != null)
                    {
                        list.ForEach(p => HotSearchCollection.Add(p));
                    }
                }
                catch (System.Exception)
                {
                }
            }

            IsHotSearchFlyoutEnabled = HotSearchCollection.Count > 0;
        }

        /// <summary>
        /// 获取搜索建议.
        /// </summary>
        /// <param name="keyWord">关键字.</param>
        /// <returns>关键字列表.</returns>
        public async Task GetSearchSuggestTagAsync(string keyWord)
        {
            try
            {
                var list = await Controller.GetSearchSuggestTagsAsync(keyWord);
                if (list.Any())
                {
                    SuggestTagList = list;
                }
            }
            catch (System.Exception)
            {
            }
        }

        /// <summary>
        /// 查询.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        public async Task SearchAsync()
        {
            if (string.IsNullOrEmpty(InputWords) || InputWords == Keyword)
            {
                return;
            }

            Keyword = InputWords;
            CurrentType = SearchModuleType.Video;
            BangumiModule.Reset(true);
            MovieModule.Reset(true);
            ArticleModule.Reset(true);
            UserModule.Reset(true);
            LiveModule.Reset(true);
            await VideoModule.InitializeRequestAsync();
        }

        /// <summary>
        /// 设置模块可见性.
        /// </summary>
        /// <param name="type">模块类型..</param>
        /// <returns><see cref="Task"/>.</returns>
        public async Task ShowModuleAsync(SearchModuleType type)
        {
            SearchSubModuleViewModel selectItem = null;
            switch (type)
            {
                case SearchModuleType.Video:
                    selectItem = VideoModule;
                    break;
                case SearchModuleType.Bangumi:
                    selectItem = BangumiModule;
                    break;
                case SearchModuleType.Live:
                    selectItem = LiveModule;
                    break;
                case SearchModuleType.User:
                    selectItem = UserModule;
                    break;
                case SearchModuleType.Movie:
                    selectItem = MovieModule;
                    break;
                case SearchModuleType.Article:
                    selectItem = ArticleModule;
                    break;
                default:
                    break;
            }

            if (!selectItem.IsRequested)
            {
                await selectItem.RequestDataAsync();
            }
        }

        /// <summary>
        /// 请求增量加载.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        public async Task RequestLoadMoreAsync()
        {
            switch (CurrentType)
            {
                case SearchModuleType.Video:
                    await VideoModule.RequestDataAsync();
                    break;
                case SearchModuleType.Bangumi:
                    await BangumiModule.RequestDataAsync();
                    break;
                case SearchModuleType.Live:
                    await LiveModule.RequestDataAsync();
                    break;
                case SearchModuleType.User:
                    await UserModule.RequestDataAsync();
                    break;
                case SearchModuleType.Movie:
                    await MovieModule.RequestDataAsync();
                    break;
                case SearchModuleType.Article:
                    await ArticleModule.RequestDataAsync();
                    break;
                default:
                    break;
            }
        }

        private void LoadModule()
        {
            VideoModule = new SearchSubModuleViewModel(SearchModuleType.Video);
            BangumiModule = new SearchSubModuleViewModel(SearchModuleType.Bangumi);
            MovieModule = new SearchSubModuleViewModel(SearchModuleType.Movie);
            ArticleModule = new SearchSubModuleViewModel(SearchModuleType.Article);
            UserModule = new SearchSubModuleViewModel(SearchModuleType.User);
            LiveModule = new SearchSubModuleViewModel(SearchModuleType.Live);
            Controller.SearchMetaChanged += OnMetaChanged;
        }

        private void OnMetaChanged(object sender, SearchMetaEventArgs e)
        {
            if (e.Keyword == VideoModule.Keyword)
            {
                foreach (var item in e.MetaList)
                {
                    var isEnabled = item.Value > 0;
                    var number = item.Value;
                    switch (item.Key)
                    {
                        case SearchModuleType.Bangumi:
                            BangumiModule.IsEnabled = isEnabled;
                            BangumiModule.Total = number;
                            break;
                        case SearchModuleType.Live:
                            LiveModule.IsEnabled = isEnabled;
                            LiveModule.Total = number;
                            break;
                        case SearchModuleType.User:
                            UserModule.IsEnabled = isEnabled;
                            UserModule.Total = number;
                            break;
                        case SearchModuleType.Movie:
                            MovieModule.IsEnabled = isEnabled;
                            MovieModule.Total = number;
                            break;
                        case SearchModuleType.Article:
                            ArticleModule.IsEnabled = isEnabled;
                            ArticleModule.Total = number;
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }
}
