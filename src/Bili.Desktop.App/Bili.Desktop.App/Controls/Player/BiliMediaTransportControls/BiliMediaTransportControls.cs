// Copyright (c) Richasy. All rights reserved.

using Bili.Desktop.App.Controls.Danmaku;
using Bili.ViewModels.Interfaces.Core;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Bili.Desktop.App.Controls.Player
{
    /// <summary>
    /// 媒体传输控件.
    /// </summary>
    public sealed partial class BiliMediaTransportControls : ReactiveControl<IMediaPlayerViewModel>
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
            if (e.OldValue is IMediaPlayerViewModel oldVM)
            {
                oldVM.PropertyChanged -= OnViewModelPropertyChanged;
            }

            var vm = e.NewValue as IMediaPlayerViewModel;
            vm.PropertyChanged -= OnViewModelPropertyChanged;
            vm.PropertyChanged += OnViewModelPropertyChanged;
        }

        /// <inheritdoc/>
        protected override void OnApplyTemplate()
        {
            _volumeSlider = GetTemplateChild(VolumeSliderName) as Slider;
            _formatListView = GetTemplateChild(FormatListViewName) as ListView;
            _playPauseButton = GetTemplateChild(PlayPauseButtonName) as Button;
            _danmakuBox = GetTemplateChild(DanmakuBoxName) as DanmakuBox;
            _rootGrid = GetTemplateChild(RootGridName) as Grid;

            if (_formatListView != null)
            {
                _formatListView.SelectionChanged += OnFormatListViewSelectionChanged;
            }

            if (_volumeSlider != null)
            {
                _volumeSlider.ValueChanged += OnVolumeSliderValueChanged;
            }
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
                case Models.Enums.PlayerDisplayMode.FullScreen:
                    VisualStateManager.GoToState(this, "FullScreenState", false);
                    break;
                default:
                    VisualStateManager.GoToState(this, "NormalState", false);
                    break;
            }
        }
    }
}
