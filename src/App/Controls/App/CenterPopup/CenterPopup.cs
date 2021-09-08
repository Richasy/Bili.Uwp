// Copyright (c) Richasy. All rights reserved.

using System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace Richasy.Bili.App.Controls
{
    /// <summary>
    /// 居中显示的浮出层.
    /// </summary>
    public sealed partial class CenterPopup : ContentControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CenterPopup"/> class.
        /// </summary>
        public CenterPopup()
        {
            this.DefaultStyleKey = typeof(CenterPopup);
            _popup = new Popup();
            _popup.Child = this;
        }

        /// <summary>
        /// 显示弹出层.
        /// </summary>
        public void Show()
        {
            Window.Current.SizeChanged += OnWindowSizeChanged;
            this.Width = Window.Current.Bounds.Width;
            this.Height = Window.Current.Bounds.Height;
            SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequest;
            _popup.IsOpen = true;
        }

        /// <summary>
        /// 隐藏弹出层.
        /// </summary>
        public void Hide()
        {
            SystemNavigationManager.GetForCurrentView().BackRequested -= OnBackRequest;
            _popup.IsOpen = false;
            Window.Current.SizeChanged -= OnWindowSizeChanged;
            Closed?.Invoke(this, EventArgs.Empty);
        }

        /// <inheritdoc/>
        protected override void OnApplyTemplate()
        {
            _closeButton = GetTemplateChild(CloseButtonName) as Button;
            if (_closeButton != null)
            {
                _closeButton.Click += OnCloseButtonClick;
            }
        }

        private void OnBackRequest(object sender, BackRequestedEventArgs e)
        {
            e.Handled = true;
            Hide();
        }

        private void OnWindowSizeChanged(object sender, WindowSizeChangedEventArgs e)
        {
            Width = e.Size.Width;
            Height = e.Size.Height;
        }

        private void OnCloseButtonClick(object sender, RoutedEventArgs e)
        {
            Hide();
        }
    }
}
