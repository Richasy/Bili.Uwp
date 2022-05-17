// Copyright (c) Richasy. All rights reserved.

using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Bili.ViewModels.Uwp
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
            _controller = Controller.Uwp.BiliController.Instance;
        }

        /// <summary>
        /// 初始化分区索引.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        public async Task InitializeAllPartitionAsync()
        {
            IsLoading = true;
            IsError = false;
            PartitionCollection.Clear();
            await Task.CompletedTask;
            IsError = true;
            IsLoading = false;
        }

        /// <summary>
        /// 打开分区.
        /// </summary>
        /// <param name="vm">分区视图模型.</param>
        /// <returns><see cref="Task"/>.</returns>
        public async Task SelectPartitionAsync(PartitionViewModel vm)
        {
            if (CurrentPartition != null && CurrentPartition != vm)
            {
                CurrentPartition.Deactive();
            }

            CurrentPartition = vm;
            await vm.ActivateAsync();
        }
    }
}
