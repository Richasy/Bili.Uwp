﻿// Copyright (c) Richasy. All rights reserved.

using Windows.Foundation;
using Windows.UI.Xaml;

namespace Bili.App.Controls.Player
{
    /// <summary>
    /// 媒体播放器.
    /// </summary>
    public sealed partial class BiliMediaPlayer
    {
        private bool IsCursorInTransportControls()
        {
            if (ViewModel.IsShowMediaTransport)
            {
                var pointerPosition = Window.Current.CoreWindow.PointerPosition;
                pointerPosition.X -= Window.Current.Bounds.X;
                pointerPosition.Y -= Window.Current.Bounds.Y;
                var rect = new Rect(0, 0, ActualWidth, ActualHeight);
                var controlPanelBounds = _mediaTransportControls.TransformToVisual(Window.Current.Content)
                    .TransformBounds(rect);
                return controlPanelBounds.Contains(pointerPosition);
            }

            return false;
        }

        private bool IsCursorInMediaElement()
        {
            var pointerPosition = Window.Current.CoreWindow.PointerPosition;
            pointerPosition.X -= Window.Current.Bounds.X;
            pointerPosition.Y -= Window.Current.Bounds.Y;
            var rect = new Rect(0, 0, ActualWidth, ActualHeight);
            var rootBounds = _interactionControl.TransformToVisual(Window.Current.Content)
                    .TransformBounds(rect);
            return rootBounds.Contains(pointerPosition) && _isCursorInPlayer;
        }

        private void ShowTempMessage(string msg)
        {
            _tempMessageContainer.Visibility = string.IsNullOrEmpty(msg)
                ? Visibility.Collapsed
                : Visibility.Visible;

            if (!string.IsNullOrEmpty(msg))
            {
                _tempMessageBlock.Text = msg;
                _tempMessageStayTime = 0;
            }
            else
            {
                _tempMessageStayTime = -1;
            }
        }

        private void HideTempMessage()
        {
            _tempMessageContainer.Visibility = Visibility.Collapsed;
            _tempMessageBlock.Text = string.Empty;
            _tempMessageStayTime = -1;
        }

        private void HandleTransportAutoHide()
        {
            if (_transportStayTime > 1.5
                && ViewModel.IsShowMediaTransport)
            {
                if (_isTouch || !IsCursorInTransportControls())
                {
                    ViewModel.IsShowMediaTransport = false;
                }
            }
        }

        private void HandleCursorAutoHide()
        {
            if (_cursorStayTime > 2
                && !ViewModel.IsShowMediaTransport
                && IsCursorInMediaElement())
            {
                Window.Current.CoreWindow.PointerCursor = null;
                _cursorStayTime = 0;
            }
        }

        private void HandleTempMessageAutoHide()
        {
            if (_tempMessageStayTime >= 2)
            {
                HideTempMessage();
            }
        }

        private void ShowAndResetMediaTransport(bool isMouse)
        {
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 0);
            if (!ViewModel.IsShowMediaTransport
                && isMouse)
            {
                ViewModel.IsShowMediaTransport = true;
            }

            _cursorStayTime = 0;
            _transportStayTime = 0;
            _isCursorInPlayer = true;
        }

        private void HideAndResetMediaTransport(bool isMouse)
        {
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 0);

            if (ViewModel.IsShowMediaTransport
                && isMouse)
            {
                ViewModel.IsShowMediaTransport = false;
            }

            _cursorStayTime = 0;
            _transportStayTime = 0;
            _isCursorInPlayer = false;
        }
    }
}