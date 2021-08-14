// Copyright (c) Richasy. All rights reserved.

using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Richasy.Bili.Controller.Uwp;
using Richasy.Bili.Models.BiliBili;

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
    }
}
