// Copyright (c) Richasy. All rights reserved.

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Richasy.Bili.App.Controls
{
    /// <summary>
    /// 卡片容器，包含基本的Pointer动画.
    /// </summary>
    public class CardPanel : Button
    {
        private long _pointerOverToken;
        private long _pressedToken;

        /// <summary>
        /// Initializes a new instance of the <see cref="CardPanel"/> class.
        /// </summary>
        public CardPanel()
        {
            this.DefaultStyleKey = typeof(CardPanel);
            Loading += OnCardPanelLoading;
            Unloaded += OnCardPanelUnloaded;
        }

        /// <inheritdoc/>
        protected override void OnApplyTemplate() => base.OnApplyTemplate();

        private void OnCardPanelLoading(FrameworkElement sender, object args)
        {
            _pointerOverToken = RegisterPropertyChangedCallback(IsPointerOverProperty, OnPanelStateChanged);
            _pressedToken = RegisterPropertyChangedCallback(IsPressedProperty, OnPanelStateChanged);
        }

        private void OnCardPanelUnloaded(object sender, RoutedEventArgs e)
        {
            UnregisterPropertyChangedCallback(IsPointerOverProperty, _pointerOverToken);
            UnregisterPropertyChangedCallback(IsPressedProperty, _pressedToken);
        }

        private void OnPanelStateChanged(DependencyObject sender, DependencyProperty dp)
        {
            if (IsPressed)
            {
            }
            else if (IsPointerOver)
            {
            }
            else
            {
            }
        }
    }
}
