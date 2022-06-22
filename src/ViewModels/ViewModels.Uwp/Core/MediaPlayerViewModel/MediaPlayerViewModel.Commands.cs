// Copyright (c) Richasy. All rights reserved.

using System;
using System.Reactive;
using Bili.Models.Data.Player;
using Bili.Models.Data.Video;
using ReactiveUI;

namespace Bili.ViewModels.Uwp.Core
{
    /// <summary>
    /// 媒体播放视图模型.
    /// </summary>
    public sealed partial class MediaPlayerViewModel
    {
        /// <inheritdoc/>
        public ReactiveCommand<Unit, Unit> ReloadCommand { get; }

        /// <summary>
        /// 改变分P的命令.
        /// </summary>
        public ReactiveCommand<VideoIdentifier, Unit> ChangePartCommand { get; }

        /// <summary>
        /// 重置播放历史的命令.
        /// </summary>
        public ReactiveCommand<Unit, Unit> ResetProgressHistoryCommand { get; }

        /// <summary>
        /// 清除播放数据的命令.
        /// </summary>
        public ReactiveCommand<Unit, Unit> ClearCommand { get; }

        /// <summary>
        /// 改变直播源是否仅有音频的命令.
        /// </summary>
        public ReactiveCommand<bool, Unit> ChangeLiveAudioOnlyCommand { get; }

        /// <summary>
        /// 改变清晰度/视频格式命令.
        /// </summary>
        public ReactiveCommand<FormatInformation, Unit> ChangeFormatCommand { get; }

        /// <summary>
        /// 播放/暂停命令.
        /// </summary>
        public ReactiveCommand<Unit, Unit> PlayPauseCommand { get; }

        /// <summary>
        /// 跳进命令.
        /// </summary>
        public ReactiveCommand<Unit, Unit> ForwardSkipCommand { get; }

        /// <summary>
        /// 改变播放速率的命令.
        /// </summary>
        public ReactiveCommand<double, Unit> ChangePlayRateCommand { get; }

        /// <summary>
        /// 改变音量的命令.
        /// </summary>
        public ReactiveCommand<double, Unit> ChangeVolumeCommand { get; }

        /// <summary>
        /// 进入/退出全屏状态的命令.
        /// </summary>
        public ReactiveCommand<Unit, Unit> ToggleFullScreenCommand { get; }

        /// <summary>
        /// 进入/退出全窗口状态的命令.
        /// </summary>
        public ReactiveCommand<Unit, Unit> ToggleFullWindowCommand { get; }

        /// <summary>
        /// 进入/退出小窗状态的命令.
        /// </summary>
        public ReactiveCommand<Unit, Unit> ToggleCompactOverlayCommand { get; }

        /// <summary>
        /// 截图命令.
        /// </summary>
        public ReactiveCommand<Unit, Unit> ScreenShotCommand { get; }

        /// <summary>
        /// 改变进度的命令.
        /// </summary>
        public ReactiveCommand<double, Unit> ChangeProgressCommand { get; }

        /// <summary>
        /// 开始临时倍速播放.
        /// </summary>
        public ReactiveCommand<Unit, Unit> StartTempQuickPlayCommand { get; }

        /// <summary>
        /// 停止临时倍速播放.
        /// </summary>
        public ReactiveCommand<Unit, Unit> StopTempQuickPlayCommand { get; }

        /// <summary>
        /// 跳转到上次观看进度的命令.
        /// </summary>
        public ReactiveCommand<Unit, Unit> JumpToLastProgressCommand { get; }

        /// <summary>
        /// 报告观看进度的命令.
        /// </summary>
        public ReactiveCommand<Unit, Unit> ReportViewProgressCommand { get; }

        /// <summary>
        /// 显示播放下一个视频的提示.
        /// </summary>
        public ReactiveCommand<Action, Unit> ShowNextVideoTipCommand { get; }

        /// <summary>
        /// 播放下一个视频的命令.
        /// </summary>
        public ReactiveCommand<Unit, Unit> PlayNextVideoCommand { get; }

        /// <summary>
        /// 增加播放速率命令.
        /// </summary>
        public ReactiveCommand<Unit, Unit> IncreasePlayRateCommand { get; }

        /// <summary>
        /// 降低播放速率命令.
        /// </summary>
        public ReactiveCommand<Unit, Unit> DecreasePlayRateCommand { get; }

        /// <summary>
        /// 增加音量命令.
        /// </summary>
        public ReactiveCommand<Unit, Unit> IncreaseVolumeCommand { get; }

        /// <summary>
        /// 降低音量命令.
        /// </summary>
        public ReactiveCommand<Unit, Unit> DecreaseVolumeCommand { get; }

        /// <summary>
        /// 选择选项命令.
        /// </summary>
        public ReactiveCommand<InteractionInformation, Unit> SelectInteractionChoiceCommand { get; }

        /// <summary>
        /// 跳进命令.
        /// </summary>
        public ReactiveCommand<Unit, Unit> BackToInteractionVideoStartCommand { get; }
    }
}
