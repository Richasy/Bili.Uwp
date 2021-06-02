// Copyright (c) Richasy. All rights reserved.

using System.Collections.ObjectModel;
using Richasy.Bili.Models.BiliBili;

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
        /// <param name="partition">数据.</param>
        public PartitionViewModel(Partition partition)
        {
            this._partition = partition;
            this.Init();
        }

        private void Init()
        {
            this.Title = this._partition.Name;
            this.ImageUrl = this._partition.Logo;
            this.SubPartitionCollection = new ObservableCollection<SubPartitionViewModel>();

            if (this._partition.Children != null && this._partition.Children.Count > 0)
            {
                this._partition.Children.ForEach(p => this.SubPartitionCollection.Add(new SubPartitionViewModel(p)));
            }

            if (this._partition.IsBangumi != 1)
            {
                this.SubPartitionCollection.Insert(0, new SubPartitionViewModel(null));
            }
        }
    }
}
