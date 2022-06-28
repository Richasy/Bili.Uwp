// Copyright (c) Richasy. All rights reserved.

using System;
using Microsoft.Toolkit.Uwp.UI.Animations;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation.Peers;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Hosting;

namespace Bili.App.Controls
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
            Loading += OnCardPanelLoading;
            Unloaded += OnCardPanelUnloaded;
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

            ElementCompositionPreview.SetIsTranslationEnabled(_rootContainer, true);
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

        private void ShowPointerOverAnimation()
        {
            if (!IsPointerOver || !IsEnableHoverAnimation)
            {
                return;
            }

            AnimationBuilder.Create().Translation(Axis.Y, PointerOverOffsetY, duration: OffsetDuration).Start(_rootContainer);
        }

        private void ShowPointerPressedAnimation()
        {
            if (!IsPressed || !IsEnableHoverAnimation)
            {
                return;
            }

            AnimationBuilder.Create().Translation(Axis.Y, 0, duration: OffsetDuration).Start(_rootContainer);
        }

        private void HideAnimation()
        {
            if (IsPointerOver)
            {
                return;
            }

            AnimationBuilder.Create().Translation(Axis.Y, 0, duration: OffsetDuration).Start(_rootContainer);
        }

        private void OnHideAnimationCompleted(object sender, CompositionBatchCompletedEventArgs args)
        {
            if (sender is CompositionScopedBatch batch)
            {
                batch.Completed -= OnHideAnimationCompleted;
                batch.Dispose();
            }
        }

        private void OnPanelStateChanged(DependencyObject sender, DependencyProperty dp)
        {
            var changedArgs = new CardPanelStateChangedEventArgs(IsPointerOver, IsPressed);
            if (IsPointerOver)
            {
                ShowPointerOverAnimation();
            }
            else if (IsPressed)
            {
                ShowPointerPressedAnimation();
            }
            else
            {
                HideAnimation();
            }

            StateChanged?.Invoke(this, changedArgs);
        }
    }
}
