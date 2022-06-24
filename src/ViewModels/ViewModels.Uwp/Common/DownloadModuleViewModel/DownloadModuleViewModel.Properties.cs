// Copyright (c) Richasy. All rights reserved.

using System.Collections.ObjectModel;
using System.Reactive;
using Bili.Lib.Interfaces;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Uwp.Account;
using Bili.ViewModels.Uwp.Core;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Bili.ViewModels.Uwp.Common
{
    /// <summary>
    /// 下载模块视图模型.
    /// </summary>
    public sealed partial class DownloadModuleViewModel
    {
        private readonly ISettingsToolkit _settingsToolkit;
        private readonly IResourceToolkit _resourceToolkit;
        private readonly IAuthorizeProvider _authorizeProvider;
        private readonly AppViewModel _appViewModel;
        private readonly AccountViewModel _accountViewModel;

        /// <summary>
        /// 改变保存位置的命令.
        /// </summary>
        public ReactiveCommand<Unit, Unit> ChangeSaveLocationCommand { get; }

        /// <summary>
        /// 保存下载命令到剪切板.
        /// </summary>
        public ReactiveCommand<Unit, Unit> SaveDownloadTextCommand { get; }

        /// <summary>
        /// 全部分P集合.
        /// </summary>
        public ObservableCollection<NumberPartViewModel> TotalPartCollection { get; }

        /// <summary>
        /// 下载参数.
        /// </summary>
        public string DownloadParameter { get; set; }

        /// <summary>
        /// 使用MP4Box来混流.
        /// </summary>
        [Reactive]
        public bool UseMp4Box { get; set; }

        /// <summary>
        /// 仅下载HEVC源.
        /// </summary>
        [Reactive]
        public bool OnlyHevc { get; set; }

        /// <summary>
        /// 仅下载AVC源.
        /// </summary>
        [Reactive]
        public bool OnlyAvc { get; set; }

        /// <summary>
        /// 是否仅下载音频.
        /// </summary>
        [Reactive]
        public bool OnlyAudio { get; set; }

        /// <summary>
        /// 是否仅下载视频.
        /// </summary>
        [Reactive]
        public bool OnlyVideo { get; set; }

        /// <summary>
        /// 是否仅下载字幕.
        /// </summary>
        [Reactive]
        public bool OnlySubtitle { get; set; }

        /// <summary>
        /// 使用多线程.
        /// </summary>
        [Reactive]
        public bool UseMultiThread { get; set; }

        /// <summary>
        /// 使用TV接口.
        /// </summary>
        [Reactive]
        public bool UseTvInterface { get; set; }

        /// <summary>
        /// 使用App接口.
        /// </summary>
        [Reactive]
        public bool UseAppInterface { get; set; }

        /// <summary>
        /// 使用国际版接口.
        /// </summary>
        [Reactive]
        public bool UseInternationalInterface { get; set; }

        /// <summary>
        /// 是否下载弹幕.
        /// </summary>
        [Reactive]
        public bool DownloadDanmaku { get; set; }

        /// <summary>
        /// 下载文件夹.
        /// </summary>
        [Reactive]
        public string DownloadFolder { get; set; }

        /// <summary>
        /// 使用分P前缀.
        /// </summary>
        [Reactive]
        public bool UsePartPerfix { get; set; }

        /// <summary>
        /// 使用分P后缀.
        /// </summary>
        [Reactive]
        public bool UseQualitySuffix { get; set; }

        /// <summary>
        /// 使用交互式清晰度选择.
        /// </summary>
        [Reactive]
        public bool UseInteractionQuality { get; set; }

        /// <summary>
        /// 是否显示分P.
        /// </summary>
        [Reactive]
        public bool IsShowPart { get; set; }
    }
}
