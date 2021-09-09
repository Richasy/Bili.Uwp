﻿// Copyright (c) Richasy. All rights reserved.

using System;
using Richasy.Bili.App.Pages;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Richasy.Bili.App.Controls
{
    /// <summary>
    /// 居中显示的浮出层.
    /// </summary>
    public partial class CenterPopup : ContentControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CenterPopup"/> class.
        /// </summary>
        public CenterPopup()
        {
            this.DefaultStyleKey = typeof(CenterPopup);
        }

        /// <summary>
        /// 显示弹出层.
        /// </summary>
        public void Show()
        {
            SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequest;
            ((Window.Current.Content as Frame).Content as RootPage).ShowOnHolder(this);
        }

        /// <summary>
        /// 隐藏弹出层.
        /// </summary>
        public void Hide()
        {
            SystemNavigationManager.GetForCurrentView().BackRequested -= OnBackRequest;
            ((Window.Current.Content as Frame).Content as RootPage).ClearHolder();
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

        private void OnCloseButtonClick(object sender, RoutedEventArgs e)
        {
            Hide();
        }
    }
}
