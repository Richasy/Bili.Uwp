// Copyright (c) Richasy. All rights reserved.

using System;
using System.Numerics;
using Microsoft.Toolkit.Uwp.UI.Animations;
using Richasy.Bili.App.Controls.Shadow;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;

namespace Richasy.Bili.App.Controls
{
    /// <summary>
    /// 卡片容器，包含基本的Pointer动画.
    /// </summary>
    public class CardPanel : Button
    {
        // Light theme
        private const float LightPointerRestShadowRadius = 6f;
        private const float LightPointerRestShadowOffsetY = 2f;
        private const float LightPointerRestShadowOpacity = 0.04f;

        private const float LightPointerOverShadowRadius = 12f;
        private const float LightPointerOverShadowOffsetY = 6f;
        private const float LightPointerOverShadowOpacity = 0.09f;

        private const float LightPointerPressedShadowRadius = 2f;
        private const float LightPointerPressedShadowOffsetY = 0;
        private const float LightPointerPressedShadowOpacity = 0.08f;

        // Dark theme
        private const float DarkPointerRestShadowRadius = 6f;
        private const float DarkPointerRestShadowOffsetY = 2f;
        private const float DarkPointerRestShadowOpacity = 0.1f;

        private const float DarkPointerOverShadowRadius = 12f;
        private const float DarkPointerOverShadowOffsetY = 8f;
        private const float DarkPointerOverShadowOpacity = 0.18f;

        private const float DarkPointerPressedShadowRadius = 2f;
        private const float DarkPointerPressedShadowOffsetY = 0f;
        private const float DarkPointerPressedShadowOpacity = 0.2f;

        private const float PointerOverOffsetY = -4f;

        private static readonly TimeSpan ShowShadowDuration = TimeSpan.FromMilliseconds(300);
        private static readonly TimeSpan HideShadowDuration = TimeSpan.FromMilliseconds(250);
        private static readonly TimeSpan PressShadowDuration = TimeSpan.FromMilliseconds(250);

        private readonly Compositor _compositor;
        private Grid _rootContainer;
        private bool _loaded;
        private bool _templateApplied;
        private bool _shadowCreated;
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
            _compositor = Window.Current.Compositor;
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

            var shadowContext = Shadows.GetAttachedShadow(_rootContainer)?.GetElementContext(_rootContainer);
            shadowContext?.CreateResources();

            if (shadowContext?.Shadow != null)
            {
                switch (ActualTheme)
                {
                    case ElementTheme.Default:
                    case ElementTheme.Dark:
                        shadowContext.Shadow.BlurRadius = DarkPointerRestShadowRadius;
                        shadowContext.Shadow.Offset = new Vector3(0, DarkPointerRestShadowOffsetY, 0);
                        shadowContext.Shadow.Opacity = DarkPointerRestShadowOpacity;
                        break;
                    case ElementTheme.Light:
                        shadowContext.Shadow.BlurRadius = LightPointerRestShadowRadius;
                        shadowContext.Shadow.Offset = new Vector3(0, LightPointerRestShadowOffsetY, 0);
                        shadowContext.Shadow.Opacity = LightPointerRestShadowOpacity;
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

            if (!IsPointerOver || !_shadowCreated)
            {
                return;
            }

            var shadowContext = Shadows.GetAttachedShadow(_rootContainer)?.GetElementContext(_rootContainer);

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

            if (!IsPressed || !_shadowCreated)
            {
                return;
            }

            var shadowContext = Shadows.GetAttachedShadow(_rootContainer)?.GetElementContext(_rootContainer);

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

            var shadowContext = Shadows.GetAttachedShadow(_rootContainer)?.GetElementContext(_rootContainer);

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

            var shadowContext = Shadows.GetAttachedShadow(_rootContainer)?.GetElementContext(_rootContainer);

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
            ApplyShadowAnimation();
        }
    }
}
