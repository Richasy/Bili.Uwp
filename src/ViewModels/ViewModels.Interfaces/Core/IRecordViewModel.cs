// Copyright (c) Richasy. All rights reserved.

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using Bili.Models.Data.Local;
using CommunityToolkit.Mvvm.Input;

namespace Bili.ViewModels.Interfaces.Core
{
    /// <summary>
    /// 本机播放记录视图模型的接口定义.
    /// </summary>
    /// <remarks>
    /// 这个视图模型不涉及在线数据，仅记录本地的播放记录，比如上一次播放了什么视频，或者在这次应用运行过程中看过哪些视频等.
    /// </remarks>
    public interface IRecordViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// 添加继续播放视图模型的命令.
        /// </summary>
        IAsyncRelayCommand<PlaySnapshot> AddLastPlayItemCommand { get; }

        /// <summary>
        /// 清除本地的继续播放视图模型的命令.
        /// </summary>
        IAsyncRelayCommand DeleteLastPlayItemCommand { get; }

        /// <summary>
        /// 添加本地播放历史条目的命令.
        /// </summary>
        IRelayCommand<PlayRecord> AddPlayRecordCommand { get; }

        /// <summary>
        /// 移除本地播放历史条目的命令.
        /// </summary>
        IRelayCommand<PlayRecord> RemovePlayRecordCommand { get; }

        /// <summary>
        /// 清空本地历史条目的命令.
        /// </summary>
        IRelayCommand ClearPlayRecordCommand { get; }

        /// <summary>
        /// 检查继续播放的命令.
        /// </summary>
        IRelayCommand CheckContinuePlayCommand { get; }

        /// <summary>
        /// 应用生命周期内的播放历史记录.
        /// </summary>
        ObservableCollection<PlayRecord> PlayRecords { get; }

        /// <summary>
        /// 是否可以显示播放历史记录的按钮.
        /// </summary>
        bool IsShowPlayRecordButton { get; }

        /// <summary>
        /// 获取上一次播放的条目.
        /// </summary>
        /// <returns><see cref="PlaySnapshot"/>.</returns>
        Task<PlaySnapshot> GetLastPlayItemAsync();
    }
}
