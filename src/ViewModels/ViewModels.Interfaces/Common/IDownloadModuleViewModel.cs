// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Bili.ViewModels.Interfaces.Common
{
    /// <summary>
    /// 下载模块视图模型.
    /// </summary>
    public interface IDownloadModuleViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// 改变保存位置的命令.
        /// </summary>
        IAsyncRelayCommand ChangeSaveLocationCommand { get; }

        /// <summary>
        /// 保存下载命令到剪切板.
        /// </summary>
        IAsyncRelayCommand SaveDownloadTextCommand { get; }

        /// <summary>
        /// 全部分P集合.
        /// </summary>
        ObservableCollection<INumberPartViewModel> TotalPartCollection { get; }

        /// <summary>
        /// 下载参数.
        /// </summary>
        string DownloadParameter { get; set; }

        /// <summary>
        /// 使用MP4Box来混流.
        /// </summary>
        bool UseMp4Box { get; set; }

        /// <summary>
        /// 仅下载HEVC源.
        /// </summary>
        bool OnlyHevc { get; set; }

        /// <summary>
        /// 仅下载AVC源.
        /// </summary>
        bool OnlyAvc { get; set; }

        /// <summary>
        /// 仅下载AV1源.
        /// </summary>
        bool OnlyAv1 { get; set; }

        /// <summary>
        /// 是否仅下载音频.
        /// </summary>
        bool OnlyAudio { get; set; }

        /// <summary>
        /// 是否仅下载视频.
        /// </summary>
        bool OnlyVideo { get; set; }

        /// <summary>
        /// 是否仅下载字幕.
        /// </summary>
        bool OnlySubtitle { get; set; }

        /// <summary>
        /// 使用多线程.
        /// </summary>
        bool UseMultiThread { get; set; }

        /// <summary>
        /// 使用TV接口.
        /// </summary>
        bool UseTvInterface { get; set; }

        /// <summary>
        /// 使用App接口.
        /// </summary>
        bool UseAppInterface { get; set; }

        /// <summary>
        /// 使用国际版接口.
        /// </summary>
        bool UseInternationalInterface { get; set; }

        /// <summary>
        /// 是否下载弹幕.
        /// </summary>
        bool DownloadDanmaku { get; set; }

        /// <summary>
        /// 下载文件夹.
        /// </summary>
        string DownloadFolder { get; set; }

        /// <summary>
        /// 使用交互式清晰度选择.
        /// </summary>
        bool UseInteractionQuality { get; set; }

        /// <summary>
        /// 是否显示分P.
        /// </summary>
        bool IsShowPart { get; set; }

        /// <summary>
        /// 加载.
        /// </summary>
        /// <param name="downloadParam">下载参数标识，比如视频 Id.</param>
        /// <param name="partList">分集列表.</param>
        void SetData(string downloadParam, IEnumerable<int> partList);
    }
}
