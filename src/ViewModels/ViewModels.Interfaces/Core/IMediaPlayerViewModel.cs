// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Bili.Models.Data.Live;
using Bili.Models.Data.Pgc;
using Bili.Models.Data.Player;
using Bili.Models.Data.Video;
using Bili.Models.Enums;
using Bili.ViewModels.Interfaces.Common;
using CommunityToolkit.Mvvm.Input;

namespace Bili.ViewModels.Interfaces.Core
{
    /// <summary>
    /// 媒体播放器视图模型的接口定义.
    /// </summary>
    public interface IMediaPlayerViewModel : INotifyPropertyChanged, IReloadViewModel, IErrorViewModel
    {
        /// <summary>
        /// 媒体播放器改变.
        /// </summary>
        event EventHandler<object> MediaPlayerChanged;

        /// <summary>
        /// 请求显示临时信息.
        /// </summary>
        event EventHandler<string> RequestShowTempMessage;

        /// <summary>
        /// 媒体播放结束.
        /// </summary>
        event EventHandler MediaEnded;

        /// <summary>
        /// 由视图模型内部改变了当前选中的分集.
        /// </summary>
        event EventHandler<VideoIdentifier> InternalPartChanged;

        /// <summary>
        /// 改变分P的命令.
        /// </summary>
        IAsyncRelayCommand<VideoIdentifier> ChangePartCommand { get; }

        /// <summary>
        /// 重置播放历史的命令.
        /// </summary>
        IRelayCommand ResetProgressHistoryCommand { get; }

        /// <summary>
        /// 改变直播源是否仅有音频的命令.
        /// </summary>
        IAsyncRelayCommand<bool> ChangeLiveAudioOnlyCommand { get; }

        /// <summary>
        /// 改变清晰度/视频格式命令.
        /// </summary>
        IAsyncRelayCommand<FormatInformation> ChangeFormatCommand { get; }

        /// <summary>
        /// 播放/暂停命令.
        /// </summary>
        IRelayCommand PlayPauseCommand { get; }

        /// <summary>
        /// 跳进命令.
        /// </summary>
        IRelayCommand ForwardSkipCommand { get; }

        /// <summary>
        /// 后退命令.
        /// </summary>
        IRelayCommand BackwardSkipCommand { get; }

        /// <summary>
        /// 改变播放速率的命令.
        /// </summary>
        IRelayCommand<double> ChangePlayRateCommand { get; }

        /// <summary>
        /// 改变音量的命令.
        /// </summary>
        IRelayCommand<double> ChangeVolumeCommand { get; }

        /// <summary>
        /// 进入/退出全屏状态的命令.
        /// </summary>
        IRelayCommand ToggleFullScreenCommand { get; }

        /// <summary>
        /// 进入/退出全窗口状态的命令.
        /// </summary>
        IRelayCommand ToggleFullWindowCommand { get; }

        /// <summary>
        /// 进入/退出小窗状态的命令.
        /// </summary>
        IRelayCommand ToggleCompactOverlayCommand { get; }

        /// <summary>
        /// 截图命令.
        /// </summary>
        IAsyncRelayCommand ScreenShotCommand { get; }

        /// <summary>
        /// 改变进度的命令.
        /// </summary>
        IRelayCommand<double> ChangeProgressCommand { get; }

        /// <summary>
        /// 开始临时倍速播放.
        /// </summary>
        IRelayCommand StartTempQuickPlayCommand { get; }

        /// <summary>
        /// 停止临时倍速播放.
        /// </summary>
        IRelayCommand StopTempQuickPlayCommand { get; }

        /// <summary>
        /// 跳转到上次观看进度的命令.
        /// </summary>
        IRelayCommand JumpToLastProgressCommand { get; }

        /// <summary>
        /// 清除数据源历史记录的命令.
        /// </summary>
        IRelayCommand ClearSourceProgressCommand { get; }

        /// <summary>
        /// 报告观看进度的命令.
        /// </summary>
        IAsyncRelayCommand ReportViewProgressCommand { get; }

        /// <summary>
        /// 显示播放下一个视频的提示.
        /// </summary>
        IRelayCommand ShowNextVideoTipCommand { get; }

        /// <summary>
        /// 播放下一个视频的命令.
        /// </summary>
        IRelayCommand PlayNextCommand { get; }

        /// <summary>
        /// 增加播放速率命令.
        /// </summary>
        IRelayCommand IncreasePlayRateCommand { get; }

        /// <summary>
        /// 降低播放速率命令.
        /// </summary>
        IRelayCommand DecreasePlayRateCommand { get; }

        /// <summary>
        /// 增加音量命令.
        /// </summary>
        IRelayCommand IncreaseVolumeCommand { get; }

        /// <summary>
        /// 降低音量命令.
        /// </summary>
        IRelayCommand DecreaseVolumeCommand { get; }

        /// <summary>
        /// 选择选项命令.
        /// </summary>
        IRelayCommand<InteractionInformation> SelectInteractionChoiceCommand { get; }

        /// <summary>
        /// 跳进命令.
        /// </summary>
        IRelayCommand BackToInteractionVideoStartCommand { get; }

        /// <summary>
        /// 返回默认模式的命令.
        /// </summary>
        IRelayCommand BackToDefaultModeCommand { get; }

        /// <summary>
        /// 退出全尺寸播放器的命令.
        /// </summary>
        IRelayCommand ExitFullPlayerCommand { get; }

        /// <summary>
        /// 清除播放数据的命令.
        /// </summary>
        IRelayCommand ClearCommand { get; }

        /// <summary>
        /// 视频格式集合.
        /// </summary>
        ObservableCollection<FormatInformation> Formats { get; }

        /// <summary>
        /// 播放速率的预设集合.
        /// </summary>
        ObservableCollection<IPlaybackRateItemViewModel> PlaybackRates { get; }

        /// <summary>
        /// 字幕模块视图模型.
        /// </summary>
        ISubtitleModuleViewModel SubtitleViewModel { get; }

        /// <summary>
        /// 字幕模块视图模型.
        /// </summary>
        IDanmakuModuleViewModel DanmakuViewModel { get; }

        /// <summary>
        /// 互动视频模块视图模型.
        /// </summary>
        IInteractionModuleViewModel InteractionViewModel { get; }

        /// <summary>
        /// 播放器状态.
        /// </summary>
        PlayerStatus Status { get; }

        /// <summary>
        /// 播放器显示模式.
        /// </summary>
        PlayerDisplayMode DisplayMode { get; set; }

        /// <summary>
        /// 当前的视频格式.
        /// </summary>
        FormatInformation CurrentFormat { get; }

        /// <summary>
        /// 音量.
        /// </summary>
        double Volume { get; }

        /// <summary>
        /// 播放速率.
        /// </summary>
        double PlaybackRate { get; }

        /// <summary>
        /// 最大播放速率.
        /// </summary>
        double MaxPlaybackRate { get; }

        /// <summary>
        /// 播放速率调整间隔.
        /// </summary>
        double PlaybackRateStep { get; }

        /// <summary>
        /// 视频时长秒数.
        /// </summary>
        double DurationSeconds { get; }

        /// <summary>
        /// 当前已播放的秒数.
        /// </summary>
        double ProgressSeconds { get; }

        /// <summary>
        /// 当前已播放的秒数的可读文本.
        /// </summary>
        string ProgressText { get; }

        /// <summary>
        /// 视频时长秒数的可读文本.
        /// </summary>
        string DurationText { get; }

        /// <summary>
        /// 是否循环播放.
        /// </summary>
        bool IsLoop { get; set; }

        /// <summary>
        /// 是否显示历史记录提示.
        /// </summary>
        bool IsShowProgressTip { get; set; }

        /// <summary>
        /// 进度提示.
        /// </summary>
        string ProgressTip { get; set; }

        /// <summary>
        /// 是否仅播放直播音频.
        /// </summary>
        bool IsLiveAudioOnly { get; set; }

        /// <summary>
        /// 全屏提示文本.
        /// </summary>
        string FullScreenText { get; set; }

        /// <summary>
        /// 全窗口提示文本.
        /// </summary>
        string FullWindowText { get; set; }

        /// <summary>
        /// 小窗提示文本.
        /// </summary>
        string CompactOverlayText { get; set; }

        /// <summary>
        /// 是否显示媒体传输控件.
        /// </summary>
        bool IsShowMediaTransport { get; set; }

        /// <summary>
        /// 显示的下一个视频提示文本.
        /// </summary>
        string NextVideoTipText { get; set; }

        /// <summary>
        /// 是否显示下一个视频提醒.
        /// </summary>
        bool IsShowNextVideoTip { get; set; }

        /// <summary>
        /// 自动播放下一个视频的倒计时秒数.
        /// </summary>
        double NextVideoCountdown { get; set; }

        /// <summary>
        /// 自动关闭进度提示的倒计时秒数.
        /// </summary>
        double ProgressTipCountdown { get; set; }

        /// <summary>
        /// 是否为互动视频.
        /// </summary>
        bool IsInteractionVideo { get; }

        /// <summary>
        /// 是否显示互动视频选项.
        /// </summary>
        bool IsShowInteractionChoices { get; }

        /// <summary>
        /// 互动视频是否已结束.
        /// </summary>
        bool IsInteractionEnd { get; }

        /// <summary>
        /// 是否正在缓冲.
        /// </summary>
        bool IsBuffering { get; }

        /// <summary>
        /// 媒体是否暂停.
        /// </summary>
        bool IsMediaPause { get; set; }

        /// <summary>
        /// 视频封面.
        /// </summary>
        string Cover { get; }

        /// <summary>
        /// 是否可以播放下一个分集.
        /// </summary>
        bool CanPlayNextPart { get; set; }

        /// <summary>
        /// 是否显示退出全尺寸播放界面按钮.
        /// </summary>
        bool IsShowExitFullPlayerButton { get; set; }

        /// <summary>
        /// 设置视频播放数据.
        /// </summary>
        /// <param name="data">视频视图数据.</param>
        /// <param name="isInPrivate">是否为无痕浏览.</param>
        void SetVideoData(VideoPlayerView data, bool isInPrivate = false);

        /// <summary>
        /// 设置 PGC 播放数据.
        /// </summary>
        /// <param name="view">PGC 内容视图.</param>
        /// <param name="episode">单集信息.</param>
        void SetPgcData(PgcPlayerView view, EpisodeInformation episode);

        /// <summary>
        /// 设置直播播放数据.
        /// </summary>
        /// <param name="data">直播视图数据.</param>
        void SetLiveData(LivePlayerView data);

        /// <summary>
        /// 设置播放下一个内容的动作.
        /// </summary>
        /// <param name="action">动作.</param>
        void SetPlayNextAction(Action action);

        /// <summary>
        /// 激活播放显示（不锁屏）.
        /// </summary>
        void ActiveDisplay();

        /// <summary>
        /// 释放播放显示（允许自动锁屏）.
        /// </summary>
        void ReleaseDisplay();
    }
}
