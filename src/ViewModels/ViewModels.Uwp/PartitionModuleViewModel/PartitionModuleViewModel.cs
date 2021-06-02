// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Richasy.Bili.Models.BiliBili;

namespace Richasy.Bili.ViewModels.Uwp
{
    /// <summary>
    /// 分区视图模型.
    /// </summary>
    public partial class PartitionModuleViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PartitionModuleViewModel"/> class.
        /// </summary>
        internal PartitionModuleViewModel()
        {
            PartitionCollection = new ObservableCollection<PartitionViewModel>();
        }

        /// <summary>
        /// 初始化分区索引.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        public async Task InitializeAllPartitionAsync()
        {
            IsLoading = true;
            PartitionCollection.Clear();
            var response = await LoadMockDataAsync<ServerResponse<List<Partition>>>("PartitionList");
            response.Data.Where(p => p.IsNeedToShow()).ToList().ForEach(p => PartitionCollection.Add(new PartitionViewModel(p)));
            IsLoading = false;
        }
    }
}
