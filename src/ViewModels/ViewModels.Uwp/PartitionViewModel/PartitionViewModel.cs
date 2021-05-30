// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Richasy.Bili.Models.BiliBili;
using Windows.Storage;

namespace Richasy.Bili.ViewModels.Uwp
{
    /// <summary>
    /// 分区视图模型.
    /// </summary>
    public partial class PartitionViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PartitionViewModel"/> class.
        /// </summary>
        internal PartitionViewModel()
        {
            PartitionCollection = new ObservableCollection<Partition>();
        }

        /// <summary>
        /// 初始化分区索引.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        public async Task InitializePartitionAsync()
        {
            IsLoading = true;
            PartitionCollection.Clear();
            var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/Mock/PartitionList.json"));
            var json = await FileIO.ReadTextAsync(file);
            var response = JsonConvert.DeserializeObject<ServerResponse<List<Partition>>>(json);
            response.Data.Where(p => p.IsNeedToShow()).ToList().ForEach(p => PartitionCollection.Add(p));
            IsLoading = false;
        }
    }
}
