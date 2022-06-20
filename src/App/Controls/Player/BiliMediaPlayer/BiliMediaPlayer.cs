﻿// Copyright (c) Richasy. All rights reserved.

using System;
using Bili.ViewModels.Uwp.Core;
using Windows.Media.Playback;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Shapes;

namespace Bili.App.Controls.Player
{
    /// <summary>
    /// 媒体播放器.
    /// </summary>
    public sealed partial class BiliMediaPlayer : ReactiveControl<MediaPlayerViewModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BiliMediaPlayer"/> class.
        /// </summary>
        public BiliMediaPlayer()
        {
            DefaultStyleKey = typeof(BiliMediaPlayer);
            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
            _unitTimer = new DispatcherTimer();
            _unitTimer.Interval = TimeSpan.FromSeconds(0.5);
            _unitTimer.Tick += OnUnitTimerTick;
        }

        internal override void OnViewModelChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue is MediaPlayerViewModel oldVM)
            {
                oldVM.MediaPlayerChanged -= OnMediaPlayerChanged;
            }

            var vm = e.NewValue as MediaPlayerViewModel;
            vm.MediaPlayerChanged -= OnMediaPlayerChanged;
            vm.MediaPlayerChanged += OnMediaPlayerChanged;
        }

        /// <inheritdoc/>
        protected override void OnApplyTemplate()
        {
            _mediaPlayerElement = GetTemplateChild(MediaPlayerElementName) as MediaPlayerElement;
            _interactionControl = GetTemplateChild(InteractionControlName) as Rectangle;
            _mediaTransportControls = GetTemplateChild(MediaTransportControlsName) as BiliMediaTransportControls;
            _tempMessageContainer = GetTemplateChild(TempMessageContaienrName) as Grid;
            _tempMessageBlock = GetTemplateChild(TempMessageBlockName) as TextBlock;
            _gestureRecognizer = new GestureRecognizer
            {
                GestureSettings = GestureSettings.HoldWithMouse | GestureSettings.Hold,
            };

            _interactionControl.ManipulationMode = ManipulationModes.TranslateX | ManipulationModes.TranslateY;
            _interactionControl.Tapped += OnInteractionControlTapped;
            _interactionControl.DoubleTapped += OnInteractionControlDoubleTapped;
            _interactionControl.ManipulationStarted += OnInteractionControlManipulationStarted;
            _interactionControl.ManipulationDelta += OnInteractionControlManipulationDelta;
            _interactionControl.ManipulationCompleted += OnInteractionControlManipulationCompleted;
            _interactionControl.PointerPressed += OnInteractionControlPointerPressed;
            _interactionControl.PointerMoved += OnInteractionControlPointerMoved;
            _interactionControl.PointerReleased += OnInteractionControlPointerReleased;
            _interactionControl.PointerCanceled += OnInteractionControlPointerCanceled;
            _gestureRecognizer.Holding += OnGestureRecognizerHolding;
        }
    }
}