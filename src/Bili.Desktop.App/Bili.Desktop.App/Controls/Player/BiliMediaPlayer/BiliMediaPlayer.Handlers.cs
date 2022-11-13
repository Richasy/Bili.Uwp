// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Atelier39;
using Bili.Models.Data.Player;
using Bili.Models.Enums;
using Bili.Models.Enums.App;
using Windows.Media.Playback;
using Windows.UI.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Input;

namespace Bili.Desktop.App.Controls.Player
{
    /// <summary>
    /// 媒体播放器.
    /// </summary>
    public sealed partial class BiliMediaPlayer
    {
        /// <inheritdoc/>
        protected override async void OnPointerEntered(PointerRoutedEventArgs e)
            => await ShowAndResetMediaTransportAsync(e.Pointer.PointerDeviceType == Windows.Devices.Input.PointerDeviceType.Mouse);

        /// <inheritdoc/>
        protected override async void OnPointerMoved(PointerRoutedEventArgs e)
            => await ShowAndResetMediaTransportAsync(e.Pointer.PointerDeviceType == Windows.Devices.Input.PointerDeviceType.Mouse);

        /// <inheritdoc/>
        protected override void OnPointerExited(PointerRoutedEventArgs e)
            => HideAndResetMediaTransport();

        /// <inheritdoc/>
        protected override void OnPointerCanceled(PointerRoutedEventArgs e)
            => HideAndResetMediaTransport();

        /// <inheritdoc/>
        protected override void OnPointerCaptureLost(PointerRoutedEventArgs e)
            => HideAndResetMediaTransport();

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            _unitTimer.Stop();
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 0);
            _interactionControl.Tapped -= OnInteractionControlTapped;
            _interactionControl.DoubleTapped -= OnInteractionControlDoubleTapped;
            _interactionControl.ManipulationStarted -= OnInteractionControlManipulationStarted;
            _interactionControl.ManipulationDelta -= OnInteractionControlManipulationDelta;
            _interactionControl.ManipulationCompleted -= OnInteractionControlManipulationCompleted;
            _interactionControl.PointerPressed -= OnInteractionControlPointerPressed;
            _interactionControl.PointerMoved -= OnInteractionControlPointerMoved;
            _interactionControl.PointerReleased -= OnInteractionControlPointerReleased;
            _interactionControl.PointerCanceled -= OnInteractionControlPointerCanceled;
            _gestureRecognizer.Holding -= OnGestureRecognizerHolding;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            _unitTimer.Start();
            _danmakuView?.ClearAll();
            _isForceHiddenTransportControls = true;
            ViewModel.RequestShowTempMessage -= OnRequestShowTempMessage;
            ViewModel.PropertyChanged -= OnViewModelPropertyChanged;
            ViewModel.RequestShowTempMessage += OnRequestShowTempMessage;
            ViewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        private void OnRequestClearDanmaku(object sender, EventArgs e)
            => _danmakuView?.ClearAll();

        private void OnDanmakuListAdded(object sender, IEnumerable<DanmakuInformation> e)
            => _danmakuView.Prepare(BilibiliDanmakuXmlParser.GetDanmakuList(e, ViewModel.DanmakuViewModel.IsDanmakuMerge), true);

        private async void OnMediaPlayerChangedAsync(object sender, object e)
        {
            if(e is MediaPlayer mp)
            {
                _mediaPlayerElement.SetMediaPlayer(mp);

                await Task.Delay(200);
                await _danmakuView?.RedrawAsync();
            }
            else
            {
                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    _mediaPlayerElement.MediaPlayer?.Dispose();
                    _mediaPlayerElement.SetMediaPlayer(null);
                });
            }
        }

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
                ViewModel.PlayPauseCommand.ExecuteAsync(null);
            }
            else
            {
                _isTouch = true;
                ViewModel.IsShowMediaTransport = !ViewModel.IsShowMediaTransport;
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
                    ViewModel.ToggleFullScreenCommand.Execute(null);
                    if (ViewModel.IsMediaPause)
                    {
                        ViewModel.PlayPauseCommand.ExecuteAsync(null);
                    }
                }
                else
                {
                    ViewModel.PlayPauseCommand.ExecuteAsync(null);
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
                ViewModel.PlayPauseCommand.ExecuteAsync(null);
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
                    ViewModel.StartTempQuickPlayCommand.ExecuteAsync(null);
                }
                else
                {
                    ViewModel.StopTempQuickPlayCommand.ExecuteAsync(null);
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

                    ViewModel.ChangeVolumeCommand.Execute(volume);
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

                    ViewModel.ChangeProgressCommand.Execute(progress);
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

            // 点击事件先于手势事件，当该事件触发时，可能已经切换了播放状态.
            _manipulationBeforeIsPlay = ViewModel.Status == PlayerStatus.Pause;
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

            if (ViewModel.IsShowProgressTip)
            {
                _progressTipStayTime += 0.5;
            }

            HandleTransportAutoHide();
            HandleCursorAutoHide();
            HandleTempMessageAutoHide();
            HandleNextVideoAutoHide();
            HandleProgressTipAutoHide();
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
            => ResizeSubtitle();

        private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ViewModel.Status))
            {
                if (ViewModel.Status == PlayerStatus.Playing)
                {
                    _danmakuView.ResumeDanmaku();
                }
                else
                {
                    _danmakuView.PauseDanmaku();
                }
            }
        }
    }
}
