// Copyright (c) Richasy. All rights reserved.

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

        /// <inheritdoc/>
        public ReactiveCommand<VideoIdentifier, Unit> ChangePartCommand { get; }

        /// <inheritdoc/>
        public ReactiveCommand<Unit, Unit> ResetProgressHistoryCommand { get; }

        /// <inheritdoc/>
        public ReactiveCommand<bool, Unit> ChangeLiveAudioOnlyCommand { get; }

        /// <inheritdoc/>
        public ReactiveCommand<FormatInformation, Unit> ChangeFormatCommand { get; }

        /// <inheritdoc/>
        public ReactiveCommand<Unit, Unit> PlayPauseCommand { get; }

        /// <inheritdoc/>
        public ReactiveCommand<Unit, Unit> ForwardSkipCommand { get; }

        /// <inheritdoc/>
        public ReactiveCommand<Unit, Unit> BackwardSkipCommand { get; }

        /// <inheritdoc/>
        public ReactiveCommand<double, Unit> ChangePlayRateCommand { get; }

        /// <inheritdoc/>
        public ReactiveCommand<double, Unit> ChangeVolumeCommand { get; }

        /// <inheritdoc/>
        public ReactiveCommand<Unit, Unit> ToggleFullScreenCommand { get; }

        /// <inheritdoc/>
        public ReactiveCommand<Unit, Unit> ToggleFullWindowCommand { get; }

        /// <inheritdoc/>
        public ReactiveCommand<Unit, Unit> ToggleCompactOverlayCommand { get; }

        /// <inheritdoc/>
        public ReactiveCommand<Unit, Unit> ScreenShotCommand { get; }

        /// <inheritdoc/>
        public ReactiveCommand<double, Unit> ChangeProgressCommand { get; }

        /// <inheritdoc/>
        public ReactiveCommand<Unit, Unit> StartTempQuickPlayCommand { get; }

        /// <inheritdoc/>
        public ReactiveCommand<Unit, Unit> StopTempQuickPlayCommand { get; }

        /// <inheritdoc/>
        public ReactiveCommand<Unit, Unit> JumpToLastProgressCommand { get; }

        /// <inheritdoc/>
        public ReactiveCommand<Unit, Unit> ClearSourceProgressCommand { get; }

        /// <inheritdoc/>
        public ReactiveCommand<Unit, Unit> ReportViewProgressCommand { get; }

        /// <inheritdoc/>
        public ReactiveCommand<Unit, Unit> ShowNextVideoTipCommand { get; }

        /// <inheritdoc/>
        public ReactiveCommand<Unit, Unit> PlayNextCommand { get; }

        /// <inheritdoc/>
        public ReactiveCommand<Unit, Unit> IncreasePlayRateCommand { get; }

        /// <inheritdoc/>
        public ReactiveCommand<Unit, Unit> DecreasePlayRateCommand { get; }

        /// <inheritdoc/>
        public ReactiveCommand<Unit, Unit> IncreaseVolumeCommand { get; }

        /// <inheritdoc/>
        public ReactiveCommand<Unit, Unit> DecreaseVolumeCommand { get; }

        /// <inheritdoc/>
        public ReactiveCommand<InteractionInformation, Unit> SelectInteractionChoiceCommand { get; }

        /// <inheritdoc/>
        public ReactiveCommand<Unit, Unit> BackToInteractionVideoStartCommand { get; }

        /// <inheritdoc/>
        public ReactiveCommand<Unit, Unit> BackToDefaultModeCommand { get; }

        /// <inheritdoc/>
        public ReactiveCommand<Unit, Unit> ExitFullPlayerCommand { get; }
    }
}
