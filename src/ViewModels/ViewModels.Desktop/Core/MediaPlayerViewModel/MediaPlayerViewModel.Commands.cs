// Copyright (c) Richasy. All rights reserved.

using Bili.Models.Data.Player;
using Bili.Models.Data.Video;
using CommunityToolkit.Mvvm.Input;

namespace Bili.ViewModels.Desktop.Core
{
    /// <summary>
    /// 媒体播放视图模型.
    /// </summary>
    public sealed partial class MediaPlayerViewModel
    {
        /// <inheritdoc/>
        public IAsyncRelayCommand ReloadCommand { get; }

        /// <inheritdoc/>
        public IAsyncRelayCommand<VideoIdentifier> ChangePartCommand { get; }

        /// <inheritdoc/>
        public IRelayCommand ResetProgressHistoryCommand { get; }

        /// <inheritdoc/>
        public IAsyncRelayCommand<bool> ChangeLiveAudioOnlyCommand { get; }

        /// <inheritdoc/>
        public IAsyncRelayCommand<FormatInformation> ChangeFormatCommand { get; }

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
        public IAsyncRelayCommand ScreenShotCommand { get; }

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
        public IAsyncRelayCommand ReportViewProgressCommand { get; }

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
