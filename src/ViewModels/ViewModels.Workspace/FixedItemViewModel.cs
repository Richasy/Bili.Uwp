// Copyright (c) Richasy. All rights reserved.

using System;
using Bili.Models.Data.Community;
using Bili.ViewModels.Interfaces.Workspace;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Bili.ViewModels.Workspace
{
    /// <summary>
    /// 固定条目的视图模型.
    /// </summary>
    /// <typeparam name="T">条目类型.</typeparam>
    public abstract partial class FixedItemViewModel<T> : ViewModelBase, IFixedItemViewModel<T>
    {
        private Action<bool, IFixedItemViewModel<T>> _action;

        [ObservableProperty]
        private T _data;

        [ObservableProperty]
        private bool _isFixed;

        /// <inheritdoc/>
        public void InjectAction(Action<bool, IFixedItemViewModel<T>> action)
            => _action = action;

        [RelayCommand]
        private void Toggle()
        {
            IsFixed = !IsFixed;
            _action.Invoke(IsFixed, this);
        }
    }

    /// <summary>
    /// 视频分区条目视图模型.
    /// </summary>
    public sealed class VideoPartitionViewModel : FixedItemViewModel<Partition>, IVideoPartitionViewModel
    {
    }
}
