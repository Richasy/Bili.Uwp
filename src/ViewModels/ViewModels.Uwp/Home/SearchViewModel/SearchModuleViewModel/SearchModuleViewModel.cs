// Copyright (c) Richasy. All rights reserved.

using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Richasy.Bili.Controller.Uwp;
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
            ModuleCollection = new ObservableCollection<SearchSubModuleViewModel>();
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
                var list = await Controller.GetHotSearchListAsync();
                if (list != null)
                {
                    list.ForEach(p => HotSearchCollection.Add(p));
                }
            }

            IsHotSearchFlyoutEnabled = HotSearchCollection.Count > 0;
        }

        /// <summary>
        /// 查询.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        public async Task SearchAsync()
        {
            if (string.IsNullOrEmpty(Keyword) || ModuleCollection.First().Keyword == Keyword)
            {
                return;
            }

            foreach (var module in ModuleCollection)
            {
                module.Reset();
                module.Keyword = Keyword;
            }

            await ModuleCollection.First().InitializeRequestAsync();
        }

        private void LoadModule()
        {
            ModuleCollection.Add(new SearchSubModuleViewModel(SearchModuleType.Video));
            ModuleCollection.Add(new SearchSubModuleViewModel(SearchModuleType.Bangumi));
            ModuleCollection.Add(new SearchSubModuleViewModel(SearchModuleType.Live));
            ModuleCollection.Add(new SearchSubModuleViewModel(SearchModuleType.User));
            ModuleCollection.Add(new SearchSubModuleViewModel(SearchModuleType.Movie));
            ModuleCollection.Add(new SearchSubModuleViewModel(SearchModuleType.Article));
        }
    }
}
