// Copyright (c) Richasy. All rights reserved.

using System.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Bili.ViewModels.Interfaces.Toolbox
{
    /// <summary>
    /// 封面下载器视图模型的接口定义.
    /// </summary>
    public interface ICoverDownloaderViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// 预览图片.
        /// </summary>
        string CoverUrl { get; }

        /// <summary>
        /// 输入Id.
        /// </summary>
        string InputId { get; set; }

        /// <summary>
        /// 是否显示错误信息.
        /// </summary>
        bool IsShowError { get; }

        /// <summary>
        /// 错误信息.
        /// </summary>
        string ErrorMessage { get; }

        /// <summary>
        /// 是否正在下载.
        /// </summary>
        bool IsDownloading { get; }

        /// <summary>
        /// 加载预览命令.
        /// </summary>
        IRelayCommand LoadPreviewCommand { get; }

        /// <summary>
        /// 下载命令.
        /// </summary>
        IRelayCommand DownloadCommand { get; }
    }
}
