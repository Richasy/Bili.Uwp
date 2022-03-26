// Copyright (c) Richasy. All rights reserved.

using System;
using System.Numerics;
using Microsoft.Toolkit.Uwp.UI;
using Microsoft.Toolkit.Uwp.UI.Animations;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation.Peers;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Hosting;

namespace Richasy.Bili.App.Controls
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
            _compositor = Window.Current.Compositor;
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

            _templateApplied = true;
            ApplyShadowAnimation();
            base.OnApplyTemplate();
        }

        private static void OnIsEnableShadowChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as CardPanel;
            if (instance._rootContainer == null)
            {
                return;
            }

            var attachedShadow = Effects.GetShadow(instance._rootContainer);
            var isEnabled = Convert.ToBoolean(e.NewValue);
            if (isEnabled && attachedShadow != null)
            {
                instance._rootContainer.ClearValue(Effects.ShadowProperty);
                instance.DestroyShadow();
            }
            else
            {
                instance.CreateShadow();
            }
        }

        private void OnCardPanelLoading(FrameworkElement sender, object args)
        {
            ActualThemeChanged += OnActualThemeChanged;

            _pointerOverToken = RegisterPropertyChangedCallback(IsPointerOverProperty, OnPanelStateChanged);
            _pressedToken = RegisterPropertyChangedCallback(IsPressedProperty, OnPanelStateChanged);

            _loaded = true;
            ApplyShadowAnimation();
        }

        private void OnCardPanelUnloaded(object sender, RoutedEventArgs e)
        {
            ActualThemeChanged -= OnActualThemeChanged;

            UnregisterPropertyChangedCallback(IsPointerOverProperty, _pointerOverToken);
            UnregisterPropertyChangedCallback(IsPressedProperty, _pressedToken);

            _loaded = false;
            DestroyShadow();
        }

        private void CreateShadow()
        {
            if (_shadowCreated || !_loaded || !_templateApplied)
            {
                // The shadow is either already created, or we're not ready to create it yet
                return;
            }

            var attachedShadow = Effects.GetShadow(_rootContainer);
            if (!IsEnableShadow && attachedShadow != null)
            {
                _rootContainer.ClearValue(Effects.ShadowProperty);
                return;
            }

            var shadowContext = attachedShadow?.GetElementContext(_rootContainer);
            shadowContext?.CreateResources();

            if (shadowContext?.Shadow != null)
            {
                switch (ActualTheme)
                {
                    case ElementTheme.Light:
                        shadowContext.Shadow.BlurRadius = LightPointerRestShadowRadius;
                        shadowContext.Shadow.Offset = new Vector3(0, LightPointerRestShadowOffsetY, 0);
                        shadowContext.Shadow.Opacity = LightPointerRestShadowOpacity;
                        break;
                    default:
                        shadowContext.Shadow.BlurRadius = DarkPointerRestShadowRadius;
                        shadowContext.Shadow.Offset = new Vector3(0, DarkPointerRestShadowOffsetY, 0);
                        shadowContext.Shadow.Opacity = DarkPointerRestShadowOpacity;
                        break;
                }
            }

            _shadowCreated = true;
        }

        private void DestroyShadow()
        {
            if (!_shadowCreated)
            {
                // The shadow has not yet been created or it has already been destroyed
                return;
            }

            _shadowCreated = false;
        }

        private void ShowPointerOverShadow()
        {
            CreateShadow();

            if (!IsPointerOver || !_shadowCreated || !IsEnableHoverAnimation)
            {
                return;
            }

            var shadowContext = Effects.GetShadow(_rootContainer)?.GetElementContext(_rootContainer);

            if (shadowContext?.SpriteVisual != null)
            {
                shadowContext.SpriteVisual.IsVisible = true;
            }

            AnimationBuilder.Create().Translation(Axis.Y, PointerOverOffsetY, duration: ShowShadowDuration).Start(_rootContainer);

            var shadowRadius = 0f;
            var shadowOffset = Vector3.Zero;
            var shadowOpacity = 0f;

            switch (ActualTheme)
            {
                case ElementTheme.Default:
                case ElementTheme.Dark:
                    shadowRadius = DarkPointerOverShadowRadius;
                    shadowOffset = new Vector3(0, DarkPointerOverShadowOffsetY, 0);
                    shadowOpacity = DarkPointerOverShadowOpacity;
                    break;
                case ElementTheme.Light:
                    shadowRadius = LightPointerOverShadowRadius;
                    shadowOffset = new Vector3(0, LightPointerOverShadowOffsetY, 0);
                    shadowOpacity = LightPointerOverShadowOpacity;
                    break;
            }

            if (shadowContext?.Shadow != null)
            {
                var shadowAnimationGroup = _compositor.CreateAnimationGroup();
                shadowAnimationGroup.Add(_compositor.CreateScalarKeyFrameAnimation(nameof(DropShadow.BlurRadius), shadowRadius, duration: ShowShadowDuration));
                shadowAnimationGroup.Add(_compositor.CreateVector3KeyFrameAnimation(nameof(DropShadow.Offset), shadowOffset, duration: ShowShadowDuration));
                shadowAnimationGroup.Add(_compositor.CreateScalarKeyFrameAnimation(nameof(DropShadow.Opacity), shadowOpacity, duration: ShowShadowDuration));
                shadowContext.Shadow.StartAnimationGroup(shadowAnimationGroup);
            }
        }

        private void ShowPointerPressedShadow()
        {
            CreateShadow();

            if (!IsPressed || !_shadowCreated || !IsEnableHoverAnimation)
            {
                return;
            }

            var shadowContext = Effects.GetShadow(_rootContainer)?.GetElementContext(_rootContainer);

            if (shadowContext?.SpriteVisual != null)
            {
                shadowContext.SpriteVisual.IsVisible = true;
            }

            AnimationBuilder.Create().Translation(Axis.Y, 0, duration: PressShadowDuration).Start(_rootContainer);

            var shadowRadius = 0f;
            var shadowOffset = Vector3.Zero;
            var shadowOpacity = 0f;

            switch (ActualTheme)
            {
                case ElementTheme.Default:
                case ElementTheme.Dark:
                    shadowRadius = DarkPointerPressedShadowRadius;
                    shadowOffset = new Vector3(0, DarkPointerPressedShadowOffsetY, 0);
                    shadowOpacity = DarkPointerPressedShadowOpacity;
                    break;
                case ElementTheme.Light:
                    shadowRadius = LightPointerPressedShadowRadius;
                    shadowOffset = new Vector3(0, LightPointerPressedShadowOffsetY, 0);
                    shadowOpacity = LightPointerPressedShadowOpacity;
                    break;
            }

            if (shadowContext?.Shadow != null)
            {
                var shadowAnimationGroup = _compositor.CreateAnimationGroup();
                shadowAnimationGroup.Add(_compositor.CreateScalarKeyFrameAnimation(nameof(DropShadow.BlurRadius), shadowRadius, duration: PressShadowDuration));
                shadowAnimationGroup.Add(_compositor.CreateVector3KeyFrameAnimation(nameof(DropShadow.Offset), shadowOffset, duration: PressShadowDuration));
                shadowAnimationGroup.Add(_compositor.CreateScalarKeyFrameAnimation(nameof(DropShadow.Opacity), shadowOpacity, duration: PressShadowDuration));
                shadowContext.Shadow.StartAnimationGroup(shadowAnimationGroup);
            }
        }

        private void HideShadow()
        {
            CreateShadow();

            if (IsPointerOver || !_shadowCreated)
            {
                return;
            }

            AnimationBuilder.Create().Translation(Axis.Y, 0, duration: HideShadowDuration).Start(_rootContainer);

            var shadowRadius = 0f;
            var shadowOffset = Vector3.Zero;
            var shadowOpacity = 0f;

            switch (ActualTheme)
            {
                case ElementTheme.Default:
                case ElementTheme.Dark:
                    shadowRadius = DarkPointerRestShadowRadius;
                    shadowOffset = new Vector3(0, DarkPointerRestShadowOffsetY, 0);
                    shadowOpacity = DarkPointerRestShadowOpacity;
                    break;
                case ElementTheme.Light:
                    shadowRadius = LightPointerRestShadowRadius;
                    shadowOffset = new Vector3(0, LightPointerRestShadowOffsetY, 0);
                    shadowOpacity = LightPointerRestShadowOpacity;
                    break;
            }

            var shadowContext = Effects.GetShadow(_rootContainer)?.GetElementContext(_rootContainer);

            if (shadowContext?.Shadow != null)
            {
                var shadowAnimationGroup = _compositor.CreateAnimationGroup();
                shadowAnimationGroup.Add(_compositor.CreateScalarKeyFrameAnimation(nameof(DropShadow.BlurRadius), shadowRadius, duration: HideShadowDuration));
                shadowAnimationGroup.Add(_compositor.CreateVector3KeyFrameAnimation(nameof(DropShadow.Offset), shadowOffset, duration: HideShadowDuration));
                shadowAnimationGroup.Add(_compositor.CreateScalarKeyFrameAnimation(nameof(DropShadow.Opacity), shadowOpacity, duration: HideShadowDuration));
                shadowContext.Shadow.StartAnimationGroup(shadowAnimationGroup);
            }
        }

        private void ApplyShadowAnimation()
        {
            if (IsPressed)
            {
                ShowPointerPressedShadow();
            }
            else if (IsPointerOver)
            {
                ShowPointerOverShadow();
            }
            else
            {
                HideShadow();
            }
        }

        private void OnHideAnimationCompleted(object sender, CompositionBatchCompletedEventArgs args)
        {
            if (sender is CompositionScopedBatch batch)
            {
                batch.Completed -= OnHideAnimationCompleted;
                batch.Dispose();
            }

            var shadowContext = Effects.GetShadow(_rootContainer)?.GetElementContext(_rootContainer);

            // Set SpriteVisible.IsVisible to false when the hide animation is complete to make sure it doesn't use GPU resources.
            if (!IsPointerOver && _shadowCreated && shadowContext?.SpriteVisual != null)
            {
                shadowContext.SpriteVisual.IsVisible = false;
            }
        }

        private void OnActualThemeChanged(FrameworkElement sender, object args)
        {
            ApplyShadowAnimation();
        }

        private void OnPanelStateChanged(DependencyObject sender, DependencyProperty dp)
        {
            var changedArgs = new CardPanelStateChangedEventArgs(IsPointerOver, IsPressed);
            StateChanged?.Invoke(this, changedArgs);
            ApplyShadowAnimation();
        }
    }
}
