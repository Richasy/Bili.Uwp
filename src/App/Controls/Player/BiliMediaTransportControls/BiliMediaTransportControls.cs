// Copyright (c) Richasy. All rights reserved.

using Bili.ViewModels.Uwp.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Bili.App.Controls.Player
{
    /// <summary>
    /// 媒体传输控件.
    /// </summary>
    public sealed partial class BiliMediaTransportControls : ReactiveControl<MediaPlayerViewModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BiliMediaTransportControls"/> class.
        /// </summary>
        public BiliMediaTransportControls()
        {
            DefaultStyleKey = typeof(BiliMediaTransportControls);
            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
        }

        internal override void OnViewModelChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue is MediaPlayerViewModel oldVM)
            {
                oldVM.PropertyChanged -= OnViewModelPropertyChanged;
            }

            var vm = e.NewValue as MediaPlayerViewModel;
            vm.PropertyChanged -= OnViewModelPropertyChanged;
            vm.PropertyChanged += OnViewModelPropertyChanged;
        }

        /// <inheritdoc/>
        protected override void OnApplyTemplate()
        {
            _volumeSlider = GetTemplateChild(VolumeSliderName) as Slider;
            _formatListView = GetTemplateChild(FormatListViewName) as ListView;
            _playPauseButton = GetTemplateChild(PlayPauseButtonName) as Button;
            _normalProgressContainer = GetTemplateChild(NormalProgressContainerName) as Panel;
            _interactionProgressContainer = GetTemplateChild(InteractionProgressContainerName) as Panel;
            _interactionProgressSlider = GetTemplateChild(InteractionProgressSliderName) as Slider;

            _volumeSlider.ValueChanged += OnVolumeSliderValueChanged;
            _formatListView.SelectionChanged += OnFormatListViewSelectionChanged;
            _normalProgressContainer.PointerEntered += OnNormalProgressContainerPointerEntered;
            _interactionProgressContainer.PointerExited += OnInteractionProgressContainerPointerExited;
            _interactionProgressContainer.PointerCanceled += OnInteractionProgressContainerPointerExited;
            _interactionProgressSlider.ValueChanged += OnInteractionProgressSliderValueChanged;
        }

        private void ChangeVisualStateFromStatus()
        {
            if (ViewModel.Status == Models.Enums.PlayerStatus.Playing)
            {
                VisualStateManager.GoToState(this, "PlayingState", false);
            }
            else if (ViewModel.Status == Models.Enums.PlayerStatus.Pause
                || ViewModel.Status == Models.Enums.PlayerStatus.End)
            {
                VisualStateManager.GoToState(this, "PauseState", false);
            }
        }

        private void ChangeVisualStateFromDisplayMode()
        {
            switch (ViewModel.DisplayMode)
            {
                case Models.Enums.PlayerDisplayMode.FullWindow:
                    VisualStateManager.GoToState(this, "FullWindowState", false);
                    break;
                case Models.Enums.PlayerDisplayMode.FullScreen:
                    VisualStateManager.GoToState(this, "FullScreenState", false);
                    break;
                case Models.Enums.PlayerDisplayMode.CompactOverlay:
                    VisualStateManager.GoToState(this, "CompactOverlayState", false);
                    break;
                default:
                    VisualStateManager.GoToState(this, "NormalState", false);
                    break;
            }
        }
    }
}
