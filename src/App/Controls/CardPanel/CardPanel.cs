// Copyright (c) Richasy. All rights reserved.

using System;
using System.Numerics;
using Microsoft.Toolkit.Uwp.UI.Animations;
using Richasy.Bili.App.Controls.Shadow;
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
            this.DefaultStyleKey = typeof(CardPanel);
            this.Loading += this.OnCardPanelLoading;
            this.Unloaded += this.OnCardPanelUnloaded;
            this._compositor = Window.Current.Compositor;
            this.IsThreeState = false;
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
        protected override void OnApplyTemplate()
        {
            this._rootContainer = this.GetTemplateChild("RootContainer") as Grid;

            ElementCompositionPreview.SetIsTranslationEnabled(this._rootContainer, true);

            this._templateApplied = true;
            this.ApplyShadowAnimation();
            base.OnApplyTemplate();
        }

        private void OnCardPanelLoading(FrameworkElement sender, object args)
        {
            this.ActualThemeChanged += this.OnActualThemeChanged;

            this._pointerOverToken = this.RegisterPropertyChangedCallback(IsPointerOverProperty, this.OnPanelStateChanged);
            this._pressedToken = this.RegisterPropertyChangedCallback(IsPressedProperty, this.OnPanelStateChanged);
            this._checkedToken = this.RegisterPropertyChangedCallback(IsCheckedProperty, this.OnIsCheckedChanged);

            this._loaded = true;
            this.ApplyShadowAnimation();
        }

        private void OnCardPanelUnloaded(object sender, RoutedEventArgs e)
        {
            this.ActualThemeChanged -= this.OnActualThemeChanged;

            this.UnregisterPropertyChangedCallback(IsPointerOverProperty, this._pointerOverToken);
            this.UnregisterPropertyChangedCallback(IsPressedProperty, this._pressedToken);
            this.UnregisterPropertyChangedCallback(IsCheckedProperty, this._checkedToken);

            this._loaded = false;
            this.DestroyShadow();
        }

        private void OnIsCheckedChanged(DependencyObject sender, DependencyProperty dp)
        {
            if (!this.IsEnableCheck)
            {
                this.IsChecked = false;
            }
        }

        private void CreateShadow()
        {
            if (this._shadowCreated || !this._loaded || !this._templateApplied)
            {
                // The shadow is either already created, or we're not ready to create it yet
                return;
            }

            var attachedShadow = Shadows.GetAttachedShadow(this._rootContainer);
            if (!this.IsEnableShadow && attachedShadow != null)
            {
                this._rootContainer.ClearValue(Shadows.AttachedShadowProperty);
                return;
            }

            var shadowContext = attachedShadow?.GetElementContext(this._rootContainer);
            shadowContext?.CreateResources();

            if (shadowContext?.Shadow != null)
            {
                switch (this.ActualTheme)
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

            this._shadowCreated = true;
        }

        private void DestroyShadow()
        {
            if (!this._shadowCreated)
            {
                // The shadow has not yet been created or it has already been destroyed
                return;
            }

            this._shadowCreated = false;
        }

        private void ShowPointerOverShadow()
        {
            this.CreateShadow();

            if (!this.IsPointerOver || !this._shadowCreated || !this.IsEnableHoverAnimation)
            {
                return;
            }

            var shadowContext = Shadows.GetAttachedShadow(this._rootContainer)?.GetElementContext(this._rootContainer);

            if (shadowContext?.SpriteVisual != null)
            {
                shadowContext.SpriteVisual.IsVisible = true;
            }

            AnimationBuilder.Create().Translation(Axis.Y, PointerOverOffsetY, duration: ShowShadowDuration).Start(this._rootContainer);

            var shadowRadius = 0f;
            var shadowOffset = Vector3.Zero;
            var shadowOpacity = 0f;

            switch (this.ActualTheme)
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
                var shadowAnimationGroup = this._compositor.CreateAnimationGroup();
                shadowAnimationGroup.Add(this._compositor.CreateScalarKeyFrameAnimation(nameof(DropShadow.BlurRadius), shadowRadius, duration: ShowShadowDuration));
                shadowAnimationGroup.Add(this._compositor.CreateVector3KeyFrameAnimation(nameof(DropShadow.Offset), shadowOffset, duration: ShowShadowDuration));
                shadowAnimationGroup.Add(this._compositor.CreateScalarKeyFrameAnimation(nameof(DropShadow.Opacity), shadowOpacity, duration: ShowShadowDuration));
                shadowContext.Shadow.StartAnimationGroup(shadowAnimationGroup);
            }
        }

        private void ShowPointerPressedShadow()
        {
            this.CreateShadow();

            if (!this.IsPressed || !this._shadowCreated || !this.IsEnableHoverAnimation)
            {
                return;
            }

            var shadowContext = Shadows.GetAttachedShadow(this._rootContainer)?.GetElementContext(this._rootContainer);

            if (shadowContext?.SpriteVisual != null)
            {
                shadowContext.SpriteVisual.IsVisible = true;
            }

            AnimationBuilder.Create().Translation(Axis.Y, 0, duration: PressShadowDuration).Start(this._rootContainer);

            var shadowRadius = 0f;
            var shadowOffset = Vector3.Zero;
            var shadowOpacity = 0f;

            switch (this.ActualTheme)
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
                var shadowAnimationGroup = this._compositor.CreateAnimationGroup();
                shadowAnimationGroup.Add(this._compositor.CreateScalarKeyFrameAnimation(nameof(DropShadow.BlurRadius), shadowRadius, duration: PressShadowDuration));
                shadowAnimationGroup.Add(this._compositor.CreateVector3KeyFrameAnimation(nameof(DropShadow.Offset), shadowOffset, duration: PressShadowDuration));
                shadowAnimationGroup.Add(this._compositor.CreateScalarKeyFrameAnimation(nameof(DropShadow.Opacity), shadowOpacity, duration: PressShadowDuration));
                shadowContext.Shadow.StartAnimationGroup(shadowAnimationGroup);
            }
        }

        private void HideShadow()
        {
            this.CreateShadow();

            if (this.IsPointerOver || !this._shadowCreated)
            {
                return;
            }

            AnimationBuilder.Create().Translation(Axis.Y, 0, duration: HideShadowDuration).Start(this._rootContainer);

            var shadowRadius = 0f;
            var shadowOffset = Vector3.Zero;
            var shadowOpacity = 0f;

            switch (this.ActualTheme)
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

            var shadowContext = Shadows.GetAttachedShadow(this._rootContainer)?.GetElementContext(this._rootContainer);

            if (shadowContext?.Shadow != null)
            {
                var shadowAnimationGroup = this._compositor.CreateAnimationGroup();
                shadowAnimationGroup.Add(this._compositor.CreateScalarKeyFrameAnimation(nameof(DropShadow.BlurRadius), shadowRadius, duration: HideShadowDuration));
                shadowAnimationGroup.Add(this._compositor.CreateVector3KeyFrameAnimation(nameof(DropShadow.Offset), shadowOffset, duration: HideShadowDuration));
                shadowAnimationGroup.Add(this._compositor.CreateScalarKeyFrameAnimation(nameof(DropShadow.Opacity), shadowOpacity, duration: HideShadowDuration));
                shadowContext.Shadow.StartAnimationGroup(shadowAnimationGroup);
            }
        }

        private void ApplyShadowAnimation()
        {
            if (this.IsPressed)
            {
                this.ShowPointerPressedShadow();
            }
            else if (this.IsPointerOver)
            {
                this.ShowPointerOverShadow();
            }
            else
            {
                this.HideShadow();
            }
        }

        private void OnHideAnimationCompleted(object sender, CompositionBatchCompletedEventArgs args)
        {
            if (sender is CompositionScopedBatch batch)
            {
                batch.Completed -= this.OnHideAnimationCompleted;
                batch.Dispose();
            }

            var shadowContext = Shadows.GetAttachedShadow(this._rootContainer)?.GetElementContext(this._rootContainer);

            // Set SpriteVisible.IsVisible to false when the hide animation is complete to make sure it doesn't use GPU resources.
            if (!this.IsPointerOver && this._shadowCreated && shadowContext?.SpriteVisual != null)
            {
                shadowContext.SpriteVisual.IsVisible = false;
            }
        }

        private void OnActualThemeChanged(FrameworkElement sender, object args)
        {
            this.ApplyShadowAnimation();
        }

        private void OnPanelStateChanged(DependencyObject sender, DependencyProperty dp)
        {
            var changedArgs = new CardPanelStateChangedEventArgs(this.IsPointerOver, this.IsPressed);
            this.StateChanged?.Invoke(this, changedArgs);
            this.ApplyShadowAnimation();
        }
    }
}
