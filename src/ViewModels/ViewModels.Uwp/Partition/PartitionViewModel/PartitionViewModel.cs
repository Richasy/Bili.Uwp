// Copyright (c) GodLeaveMe. All rights reserved.

using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
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

        /// <summary>
        /// 激活该分区.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        public async Task ActivateAsync()
        {
            if (this.CurrentSelectedSubPartition == null)
            {
                await SelectSubPartitionAsync(SubPartitionCollection.First());
            }
            else
            {
                this.CurrentSelectedSubPartition.Activate();
            }
        }

        /// <summary>
        /// 停用该分区.
        /// </summary>
        public void Deactive()
        {
            if (this.CurrentSelectedSubPartition != null)
            {
                this.CurrentSelectedSubPartition.Deactive();
            }
        }

        /// <summary>
        /// 设置选中的子分区.
        /// </summary>
        /// <param name="vm">子分区视图模型.</param>
        /// <returns><see cref="Task"/>.</returns>
        public async Task SelectSubPartitionAsync(SubPartitionViewModel vm)
        {
            if (this.CurrentSelectedSubPartition != null)
            {
                this.CurrentSelectedSubPartition.Deactive();
            }

            this.CurrentSelectedSubPartition = vm;
            vm.Activate();
            if (!vm.IsRequested)
            {
                await vm.InitializeRequestAsync();
            }
        }

        private void Init()
        {
            this.Title = this._partition.Name;
            this.ImageUrl = this._partition.Logo;
            this.SubPartitionCollection = new ObservableCollection<SubPartitionViewModel>();

            if (this._partition.Children?.Any() ?? false)
            {
                this._partition.Children.ForEach(p => this.SubPartitionCollection.Add(new SubPartitionViewModel(p)));
            }
        }
    }
}
