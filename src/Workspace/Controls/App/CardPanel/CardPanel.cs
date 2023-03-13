// Copyright (c) Richasy. All rights reserved.

using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Automation.Peers;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;

namespace Bili.Workspace.Controls
{
    /// <summary>
    /// 卡片面板.
    /// </summary>
    public partial class CardPanel : ToggleButton
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CardPanel"/> class.
        /// </summary>
        public CardPanel()
        {
            DefaultStyleKey = typeof(CardPanel);
            IsThreeState = false;
        }

        /// <summary>
        /// 状态更改事件.
        /// </summary>
        public event EventHandler<CardPanelStateChangedEventArgs> StateChanged;

        /// <inheritdoc/>
        protected override AutomationPeer OnCreateAutomationPeer()
        {
            return new CardPanelAutomationPeer(this);
        }

        /// <inheritdoc/>
        protected override void OnToggle()
        {
            if (IsEnableCheck)
            {
                IsChecked = !IsChecked;
            }
        }

        /// <inheritdoc/>
        protected override void OnApplyTemplate()
        {
            _rootContainer = GetTemplateChild("RootContainer") as Grid;
            base.OnApplyTemplate();
        }

        /// <inheritdoc/>
        protected override void OnBringIntoViewRequested(BringIntoViewRequestedEventArgs e)
        {
            if (double.IsNaN(e.VerticalAlignmentRatio) || e.VerticalAlignmentRatio != 0.5)
            {
                e.TargetElement.StartBringIntoView(new BringIntoViewOptions()
                {
                    AnimationDesired = true,
                    VerticalAlignmentRatio = 0.5,
                });
            }
        }

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
            var changedArgs = new CardPanelStateChangedEventArgs(IsPointerOver, IsPressed);
            StateChanged?.Invoke(this, changedArgs);
        }
    }
}
