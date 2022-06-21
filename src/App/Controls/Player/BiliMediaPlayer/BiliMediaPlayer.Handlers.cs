// Copyright (c) Richasy. All rights reserved.

using System;
using Bili.Models.Enums;
using Bili.Models.Enums.App;
using Windows.Media.Playback;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;

namespace Bili.App.Controls.Player
{
    /// <summary>
    /// 媒体播放器.
    /// </summary>
    public sealed partial class BiliMediaPlayer
    {
        /// <inheritdoc/>
        protected override void OnPointerEntered(PointerRoutedEventArgs e)
            => ShowAndResetMediaTransport(e.Pointer.PointerDeviceType == Windows.Devices.Input.PointerDeviceType.Mouse);

        /// <inheritdoc/>
        protected override void OnPointerMoved(PointerRoutedEventArgs e)
            => ShowAndResetMediaTransport(e.Pointer.PointerDeviceType == Windows.Devices.Input.PointerDeviceType.Mouse);

        /// <inheritdoc/>
        protected override void OnPointerExited(PointerRoutedEventArgs e)
            => HideAndResetMediaTransport(e.Pointer.PointerDeviceType == Windows.Devices.Input.PointerDeviceType.Mouse);

        /// <inheritdoc/>
        protected override void OnPointerCanceled(PointerRoutedEventArgs e)
            => HideAndResetMediaTransport(e.Pointer.PointerDeviceType == Windows.Devices.Input.PointerDeviceType.Mouse);

        /// <inheritdoc/>
        protected override void OnPointerCaptureLost(PointerRoutedEventArgs e)
            => HideAndResetMediaTransport(e.Pointer.PointerDeviceType == Windows.Devices.Input.PointerDeviceType.Mouse);

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            _unitTimer.Stop();
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 0);
            ViewModel.MediaPlayerChanged -= OnMediaPlayerChanged;
            ViewModel.RequestShowTempMessage -= OnRequestShowTempMessage;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            _unitTimer.Start();
            ViewModel.RequestShowTempMessage += OnRequestShowTempMessage;
        }

        private void OnMediaPlayerChanged(object sender, MediaPlayer e)
            => _mediaPlayerElement.SetMediaPlayer(e);

        private void OnInteractionControlTapped(object sender, TappedRoutedEventArgs e)
        {
            if (_isHolding)
            {
                _isHolding = false;
                return;
            }

            if (e.PointerDeviceType == Windows.Devices.Input.PointerDeviceType.Mouse)
            {
                _isTouch = false;
                ViewModel.PlayPauseCommand.Execute().Subscribe();
            }
            else
            {
                _isTouch = true;
                Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 0);
            }
        }

        private void OnInteractionControlDoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            var playerStatus = ViewModel.Status;
            var canDoubleTapped = playerStatus == PlayerStatus.Playing || playerStatus == PlayerStatus.Pause;
            if (canDoubleTapped)
            {
                if (e.PointerDeviceType == Windows.Devices.Input.PointerDeviceType.Mouse)
                {
                    ViewModel.ToggleFullScreenCommand.Execute().Subscribe();
                }
            }
        }

        private void OnInteractionControlManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            _manipulationVolume = 0;
            _manipulationProgress = 0;
            _manipulationDeltaX = 0;
            _manipulationDeltaY = 0;
            _manipulationType = PlayerManipulationType.None;

            if (_manipulationBeforeIsPlay)
            {
                ViewModel.PlayPauseCommand.Execute().Subscribe();
            }

            _manipulationBeforeIsPlay = false;
        }

        private void OnGestureRecognizerHolding(GestureRecognizer sender, HoldingEventArgs args)
        {
            if (args.ContactCount == 1)
            {
                _isHolding = true;
                if (args.HoldingState == HoldingState.Started)
                {
                    ViewModel.StartTempQuickPlayCommand.Execute().Subscribe();
                }
                else
                {
                    ViewModel.StopTempQuickPlayCommand.Execute().Subscribe();
                }
            }
        }

        private void OnInteractionControlManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            if (ViewModel.Status != PlayerStatus.Playing
                && ViewModel.Status != PlayerStatus.Pause)
            {
                return;
            }

            _manipulationDeltaX += e.Delta.Translation.X;
            _manipulationDeltaY -= e.Delta.Translation.Y;
            if (Math.Abs(_manipulationDeltaX) > 15 || Math.Abs(_manipulationDeltaY) > 15)
            {
                if (_manipulationType == PlayerManipulationType.None)
                {
                    var isVolume = Math.Abs(_manipulationDeltaY) > Math.Abs(_manipulationDeltaX);
                    _manipulationType = isVolume ? PlayerManipulationType.Volume : PlayerManipulationType.Progress;
                }

                if (_manipulationType == PlayerManipulationType.Volume)
                {
                    var volume = _manipulationVolume + (_manipulationDeltaY / 2.0);
                    if (volume > 100)
                    {
                        volume = 100;
                    }
                    else if (volume < 0)
                    {
                        volume = 0;
                    }

                    ViewModel.ChangeVolumeCommand.Execute(volume).Subscribe();
                }
                else
                {
                    var progress = _manipulationProgress + (_manipulationDeltaX * _manipulationUnitLength);
                    if (progress > ViewModel.DurationSeconds)
                    {
                        progress = ViewModel.DurationSeconds;
                    }
                    else if (progress < 0)
                    {
                        progress = 0;
                    }

                    ViewModel.ChangeProgressCommand.Execute(progress).Subscribe();
                }
            }
        }

        private void OnInteractionControlManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            if (ViewModel.Status == PlayerStatus.NotLoad || ViewModel.Status == PlayerStatus.Buffering)
            {
                return;
            }

            _manipulationProgress = ViewModel.ProgressSeconds;
            _manipulationVolume = ViewModel.Volume;
            _manipulationBeforeIsPlay = ViewModel.Status == PlayerStatus.Playing;
            if (ViewModel.DurationSeconds > 0)
            {
                // 获取单位像素对应的时长
                var unit = ViewModel.DurationSeconds / ActualWidth;
                _manipulationUnitLength = unit / 1.5;
            }
        }

        private void OnInteractionControlPointerCanceled(object sender, PointerRoutedEventArgs e)
            => _gestureRecognizer.ProcessUpEvent(e.GetCurrentPoint(this));

        private void OnInteractionControlPointerReleased(object sender, PointerRoutedEventArgs e)
            => _gestureRecognizer.ProcessUpEvent(e.GetCurrentPoint(this));

        private void OnInteractionControlPointerMoved(object sender, PointerRoutedEventArgs e)
            => _gestureRecognizer.ProcessMoveEvents(e.GetIntermediatePoints(this));

        private void OnInteractionControlPointerPressed(object sender, PointerRoutedEventArgs e)
            => _gestureRecognizer.ProcessDownEvent(e.GetCurrentPoint(this));

        private void OnRequestShowTempMessage(object sender, string e)
            => ShowTempMessage(e);

        private void OnUnitTimerTick(object sender, object e)
        {
            _cursorStayTime += 0.5;
            _transportStayTime += 0.5;

            if (_tempMessageStayTime != -1)
            {
                _tempMessageStayTime += 0.5;
            }

            if (ViewModel.IsShowNextVideoTip)
            {
                _nextVideoStayTime += 0.5;
            }

            HandleTransportAutoHide();
            HandleCursorAutoHide();
            HandleTempMessageAutoHide();
            HandleNextVideoAutoHide();
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
            => ResizeSubtitle();
    }
}
