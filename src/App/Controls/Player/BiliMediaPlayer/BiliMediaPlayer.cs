// Copyright (c) Richasy. All rights reserved.

using System;
using Bili.App.Controls.Danmaku;
using Bili.ViewModels.Interfaces.Core;
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
    public sealed partial class BiliMediaPlayer : ReactiveControl<IMediaPlayerViewModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BiliMediaPlayer"/> class.
        /// </summary>
        public BiliMediaPlayer()
        {
            DefaultStyleKey = typeof(BiliMediaPlayer);
            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
            SizeChanged += OnSizeChanged;
            _unitTimer = new DispatcherTimer();
            _unitTimer.Interval = TimeSpan.FromSeconds(0.5);
            _unitTimer.Tick += OnUnitTimerTick;
        }

        internal override void OnViewModelChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue is IMediaPlayerViewModel oldVM)
            {
                oldVM.MediaPlayerChanged -= OnMediaPlayerChangedAsync;
            }

            var vm = e.NewValue as IMediaPlayerViewModel;
            vm.IsShowMediaTransport = true;
            vm.MediaPlayerChanged -= OnMediaPlayerChangedAsync;
            vm.MediaPlayerChanged += OnMediaPlayerChangedAsync;
        }

        /// <inheritdoc/>
        protected override void OnApplyTemplate()
        {
            _mediaPlayerElement = GetTemplateChild(MediaPlayerElementName) as MediaPlayerElement;
            _interactionControl = GetTemplateChild(InteractionControlName) as Rectangle;
            _mediaTransportControls = GetTemplateChild(MediaTransportControlsName) as BiliMediaTransportControls;
            _tempMessageContainer = GetTemplateChild(TempMessageContaienrName) as Grid;
            _tempMessageBlock = GetTemplateChild(TempMessageBlockName) as TextBlock;
            _subtitleBlock = GetTemplateChild(SubtitleBlockName) as TextBlock;
            _danmakuView = GetTemplateChild(DanmakuViewName) as DanmakuView;

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
