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
        public IRelayCommand ReloadCommand { get; }

        /// <inheritdoc/>
        public IRelayCommand<VideoIdentifier> ChangePartCommand { get; }

        /// <inheritdoc/>
        public IRelayCommand ResetProgressHistoryCommand { get; }

        /// <inheritdoc/>
        public IRelayCommand<bool> ChangeLiveAudioOnlyCommand { get; }

        /// <inheritdoc/>
        public IRelayCommand<FormatInformation> ChangeFormatCommand { get; }

        /// <inheritdoc/>
        public IRelayCommand PlayPauseCommand { get; }

        /// <inheritdoc/>
        public IRelayCommand ForwardSkipCommand { get; }

        /// <inheritdoc/>
        public IRelayCommand BackwardSkipCommand { get; }

        /// <inheritdoc/>
        public IRelayCommand<double> ChangePlayRateCommand { get; }

        /// <inheritdoc/>
        public IRelayCommand<double> ChangeVolumeCommand { get; }

        /// <inheritdoc/>
        public IRelayCommand ToggleFullScreenCommand { get; }

        /// <inheritdoc/>
        public IRelayCommand ToggleFullWindowCommand { get; }

        /// <inheritdoc/>
        public IRelayCommand ToggleCompactOverlayCommand { get; }

        /// <inheritdoc/>
        public IRelayCommand ScreenShotCommand { get; }

        /// <inheritdoc/>
        public IRelayCommand<double> ChangeProgressCommand { get; }

        /// <inheritdoc/>
        public IRelayCommand StartTempQuickPlayCommand { get; }

        /// <inheritdoc/>
        public IRelayCommand StopTempQuickPlayCommand { get; }

        /// <inheritdoc/>
        public IRelayCommand JumpToLastProgressCommand { get; }

        /// <inheritdoc/>
        public IRelayCommand ClearSourceProgressCommand { get; }

        /// <inheritdoc/>
        public IRelayCommand ReportViewProgressCommand { get; }

        /// <inheritdoc/>
        public IRelayCommand ShowNextVideoTipCommand { get; }

        /// <inheritdoc/>
        public IRelayCommand PlayNextCommand { get; }

        /// <inheritdoc/>
        public IRelayCommand IncreasePlayRateCommand { get; }

        /// <inheritdoc/>
        public IRelayCommand DecreasePlayRateCommand { get; }

        /// <inheritdoc/>
        public IRelayCommand IncreaseVolumeCommand { get; }

        /// <inheritdoc/>
        public IRelayCommand DecreaseVolumeCommand { get; }

        /// <inheritdoc/>
        public IRelayCommand<InteractionInformation> SelectInteractionChoiceCommand { get; }

        /// <inheritdoc/>
        public IRelayCommand BackToInteractionVideoStartCommand { get; }

        /// <inheritdoc/>
        public IRelayCommand BackToDefaultModeCommand { get; }

        /// <inheritdoc/>
        public IRelayCommand ExitFullPlayerCommand { get; }

        /// <inheritdoc/>
        public IRelayCommand ClearCommand { get; }
    }
}
