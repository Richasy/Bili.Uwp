// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using Bili.Models.Data.Player;
using Bili.Models.Data.Video;
using Bili.Models.Enums;
using Bili.ViewModels.Interfaces.Common;
using ReactiveUI;

namespace Bili.ViewModels.Interfaces.Core
{
    /// <summary>
    /// 媒体播放器视图模型的接口定义.
    /// </summary>
    public interface IMediaPlayerViewModel : IReactiveObject, IReloadViewModel, IErrorViewModel
    {
        /// <summary>
        /// 媒体播放器改变.
        /// </summary>
        public event EventHandler<object> MediaPlayerChanged;

        /// <summary>
        /// 请求显示临时信息.
        /// </summary>
        public event EventHandler<string> RequestShowTempMessage;

        /// <summary>
        /// 媒体播放结束.
        /// </summary>
        public event EventHandler MediaEnded;

        /// <summary>
        /// 由视图模型内部改变了当前选中的分集.
        /// </summary>
        public event EventHandler<VideoIdentifier> InternalPartChanged;

        /// <summary>
        /// 视频格式集合.
        /// </summary>
        public ObservableCollection<FormatInformation> Formats { get; }

        /// <summary>
        /// 播放速率的预设集合.
        /// </summary>
        public ObservableCollection<IPlaybackRateItemViewModel> PlaybackRates { get; }

        /// <summary>
        /// 字幕模块视图模型.
        /// </summary>
        public ISubtitleModuleViewModel SubtitleViewModel { get; }

        /// <summary>
        /// 字幕模块视图模型.
        /// </summary>
        public IDanmakuModuleViewModel DanmakuViewModel { get; }

        /// <summary>
        /// 互动视频模块视图模型.
        /// </summary>
        public IInteractionModuleViewModel InteractionViewModel { get; }

        /// <summary>
        /// 播放器状态.
        /// </summary>
        public PlayerStatus Status { get; }

        /// <summary>
        /// 播放器显示模式.
        /// </summary>
        public PlayerDisplayMode DisplayMode { get; }

        /// <summary>
        /// 当前的视频格式.
        /// </summary>
        public FormatInformation CurrentFormat { get; }

        /// <summary>
        /// 音量.
        /// </summary>
        public double Volume { get; }

        /// <summary>
        /// 播放速率.
        /// </summary>
        public double PlaybackRate { get; }

        /// <summary>
        /// 最大播放速率.
        /// </summary>
        public double MaxPlaybackRate { get; }

        /// <summary>
        /// 播放速率调整间隔.
        /// </summary>
        public double PlaybackRateStep { get; }

        /// <summary>
        /// 视频时长秒数.
        /// </summary>
        public double DurationSeconds { get; }

        /// <summary>
        /// 当前已播放的秒数.
        /// </summary>
        public double ProgressSeconds { get; }

        /// <summary>
        /// 当前已播放的秒数的可读文本.
        /// </summary>
        public string ProgressText { get; }

        /// <summary>
        /// 视频时长秒数的可读文本.
        /// </summary>
        public string DurationText { get; }

        /// <summary>
        /// 是否循环播放.
        /// </summary>
        public bool IsLoop { get; }

        /// <summary>
        /// 是否显示历史记录提示.
        /// </summary>
        public bool IsShowProgressTip { get; }

        /// <summary>
        /// 进度提示.
        /// </summary>
        public string ProgressTip { get; }

        /// <summary>
        /// 是否仅播放直播音频.
        /// </summary>
        public bool IsLiveAudioOnly { get; }

        /// <summary>
        /// 全屏提示文本.
        /// </summary>
        public string FullScreenText { get; }

        /// <summary>
        /// 全窗口提示文本.
        /// </summary>
        public string FullWindowText { get; }

        /// <summary>
        /// 小窗提示文本.
        /// </summary>
        public string CompactOverlayText { get; }

        /// <summary>
        /// 是否显示媒体传输控件.
        /// </summary>
        public bool IsShowMediaTransport { get; }

        /// <summary>
        /// 显示的下一个视频提示文本.
        /// </summary>
        public string NextVideoTipText { get; }

        /// <summary>
        /// 是否显示下一个视频提醒.
        /// </summary>
        public bool IsShowNextVideoTip { get; }

        /// <summary>
        /// 自动播放下一个视频的倒计时秒数.
        /// </summary>
        public double NextVideoCountdown { get; }

        /// <summary>
        /// 自动关闭进度提示的倒计时秒数.
        /// </summary>
        public double ProgressTipCountdown { get; }

        /// <summary>
        /// 是否为互动视频.
        /// </summary>
        public bool IsInteractionVideo { get; }

        /// <summary>
        /// 是否显示互动视频选项.
        /// </summary>
        public bool IsShowInteractionChoices { get; }

        /// <summary>
        /// 互动视频是否已结束.
        /// </summary>
        public bool IsInteractionEnd { get; }

        /// <summary>
        /// 是否正在缓冲.
        /// </summary>
        public bool IsBuffering { get; }

        /// <summary>
        /// 媒体是否暂停.
        /// </summary>
        public bool IsMediaPause { get; }

        /// <summary>
        /// 视频封面.
        /// </summary>
        public string Cover { get; }

        /// <summary>
        /// 是否可以播放下一个分集.
        /// </summary>
        public bool CanPlayNextPart { get; }

        /// <summary>
        /// 是否显示退出全尺寸播放界面按钮.
        /// </summary>
        public bool IsShowExitFullPlayerButton { get; }
    }
}
