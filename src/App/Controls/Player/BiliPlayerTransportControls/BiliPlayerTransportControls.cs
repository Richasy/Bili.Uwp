// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Atelier39;
using Bili.App.Controls.Danmaku;
using Bili.Locator.Uwp;
using Bili.Models.App.Args;
using Bili.Models.BiliBili;
using Bili.Models.Data.Player;
using Bili.Models.Enums;
using Bili.Models.Enums.App;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Uwp;
using Bili.ViewModels.Uwp.Account;
using Bili.ViewModels.Uwp.Core;
using Bilibili.Community.Service.Dm.V1;
using Splat;
using Windows.Foundation;
using Windows.Media.Playback;
using Windows.UI.Input;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Shapes;

namespace Bili.App.Controls
{
    /// <summary>
    /// 哔哩播放器的媒体传输控件.
    /// </summary>
    public partial class BiliPlayerTransportControls : MediaTransportControls
    {
        private readonly NavigationViewModel _navigationViewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="BiliPlayerTransportControls"/> class.
        /// </summary>
        public BiliPlayerTransportControls()
        {
            DefaultStyleKey = typeof(BiliPlayerTransportControls);
            ShowAndHideAutomatically = false;
            _navigationViewModel = Splat.Locator.Current.GetService<NavigationViewModel>();
            _navigationViewModel.Navigating += OnNavigating;
            _segmentIndex = 1;
            Instance = this;
            SizeChanged += OnSizeChanged;
            InitializeDanmakuTimer();
            InitializeCursorTimer();
            InitializeNormalTimer();
            InitializeFocusTimer();

            _cursorTimer.Start();
            _normalTimer.Start();
            _focusTimer.Start();
        }

        // TODO: 在切换播放器时释放
        private void OnNavigating(object sender, AppNavigationEventArgs e)
        {
            if (e.Type == NavigationType.Player)
            {
                _cursorTimer.Start();
                _normalTimer.Start();
                _focusTimer.Start();
            }
            else
            {
                _cursorTimer.Stop();
                _normalTimer.Stop();
                _focusTimer.Stop();
            }
        }

        /// <summary>
        /// 检查播放暂停按钮是否聚焦.
        /// </summary>
        public void CheckPlayPauseButtonFocus()
        {
            if (_playPauseButton != null)
            {
                _playPauseButton.Focus(FocusState.Programmatic);
            }
        }

        /// <summary>
        /// 检查当前的播放器模式.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        public async Task CheckCurrentPlayerModeAsync()
        {
            if (ViewModel.IsDetailError
                || ViewModel.IsPlayInformationError
                || _fullScreenPlayModeButton == null
                || !_fullScreenPlayModeButton.IsLoaded
                || _fullWindowPlayModeButton == null
                || !_fullWindowPlayModeButton.IsLoaded
                || _compactOverlayPlayModeButton == null
                || !_compactOverlayPlayModeButton.IsLoaded)
            {
            }
            else
            {
                switch (ViewModel.PlayerDisplayMode)
                {
                    case PlayerDisplayMode.Default:
                        _fullWindowPlayModeButton.IsChecked = false;
                        _fullScreenPlayModeButton.IsChecked = false;
                        _compactOverlayPlayModeButton.IsChecked = false;
                        _backButton.Visibility = Visibility.Collapsed;
                        break;
                    case PlayerDisplayMode.FullWindow:
                        _fullWindowPlayModeButton.IsChecked = true;
                        _fullScreenPlayModeButton.IsChecked = false;
                        _compactOverlayPlayModeButton.IsChecked = false;
                        _backButton.Visibility = Visibility.Visible;
                        break;
                    case PlayerDisplayMode.FullScreen:
                        _fullWindowPlayModeButton.IsChecked = false;
                        _fullScreenPlayModeButton.IsChecked = true;
                        _compactOverlayPlayModeButton.IsChecked = false;
                        _backButton.Visibility = Visibility.Visible;
                        break;
                    case PlayerDisplayMode.CompactOverlay:
                        _fullWindowPlayModeButton.IsChecked = false;
                        _fullScreenPlayModeButton.IsChecked = false;
                        _compactOverlayPlayModeButton.IsChecked = true;
                        _backButton.Visibility = Visibility.Collapsed;
                        break;
                    default:
                        break;
                }
            }

            _playPauseButton?.Focus(FocusState.Programmatic);
            _backToDefaultButton.Visibility = ViewModel.PlayerDisplayMode != PlayerDisplayMode.Default
                ? Visibility.Visible
                : Visibility.Collapsed;

            await _danmakuView.RedrawAsync();
        }

        /// <inheritdoc/>
        protected override async void OnApplyTemplate()
        {
            _danmakuView = GetTemplateChild(DanmakuViewName) as DanmakuView;
            _fullWindowPlayModeButton = GetTemplateChild(FullWindowPlayModeButtonName) as ToggleButton;
            _fullScreenPlayModeButton = GetTemplateChild(FullScreenPlayModeButtonName) as ToggleButton;
            _compactOverlayPlayModeButton = GetTemplateChild(CompactOverlayPlayModeButtonName) as ToggleButton;
            _interactionControl = GetTemplateChild(InteractionControlName) as Rectangle;
            _controlPanel = GetTemplateChild(ControlPanelName) as Border;
            _formatListView = GetTemplateChild(FormatListViewName) as ListView;
            _livePlayLineListView = GetTemplateChild(LivePlayLineListViewName) as ListView;
            _liveQualityListView = GetTemplateChild(LiveQualityListViewName) as ListView;
            _backButton = GetTemplateChild(BackButtonName) as Button;
            _backSkipButton = GetTemplateChild(BackSkipButtonName) as Button;
            _forwardSkipButton = GetTemplateChild(ForwardSkipButtonName) as Button;
            _playPauseButton = GetTemplateChild(PlayPauseButtonName) as Button;
            _danmakuBarVisibilityButton = GetTemplateChild(DanmakuBarVisibilityButtonName) as Button;
            _homeButton = GetTemplateChild(HomeButtonName) as Button;
            _backToDefaultButton = GetTemplateChild(BackToDefaultButtonName) as Button;
            _liveRefreshButton = GetTemplateChild(LiveRefreshButtonName) as Button;
            _previousEpisodeButton = GetTemplateChild(PreviousEpisodeButtonName) as Button;
            _nextEpisodeButton = GetTemplateChild(NextEpisodeButtonName) as Button;
            _screenshotButton = GetTemplateChild(ScreenshotButtonName) as Button;
            _subtitleBlock = GetTemplateChild(SubtitleBlockName) as TextBlock;
            _tempMessageContainer = GetTemplateChild(TempMessageContaienrName) as Grid;
            _tempMessageBlock = GetTemplateChild(TempMessageBlockName) as TextBlock;
            _playbackRateNodeComboBox = GetTemplateChild(PlaybackRateNodeComboBoxName) as ComboBox;
            _increasePlayRateButton = GetTemplateChild(IncreasePlayRateButtonName) as Button;
            _decreasePlayRateButton = GetTemplateChild(DecreasePlayRateButtonName) as Button;
            _increaseVolumeButton = GetTemplateChild(IncreaseVolumeButtonName) as Button;
            _decreaseVolumeButton = GetTemplateChild(DecreaseVolumeButtonName) as Button;
            _nextVidepInformer = GetTemplateChild(NextVideoInformerName) as PlayerTip;
            _previousViewInformer = GetTemplateChild(PreviousViewInformerName) as PlayerTip;
            _formatButton = GetTemplateChild(FormatButtonName) as Button;
            _liveQualityButton = GetTemplateChild(LiveQualityButtonName) as Button;
            _miniViewButton = GetTemplateChild(MiniViewButtonName) as Button;
            _rootGrid = GetTemplateChild(RootGridName) as Grid;
            _gestureRecognizer = new GestureRecognizer();
            _gestureRecognizer.GestureSettings = GestureSettings.HoldWithMouse | GestureSettings.Hold;

            _fullWindowPlayModeButton.Click += OnPlayModeButtonClick;
            _fullScreenPlayModeButton.Click += OnPlayModeButtonClick;
            _compactOverlayPlayModeButton.Click += OnPlayModeButtonClick;
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
            _gestureRecognizer.Holding += OnGestureRecognizerHoldingAsync;
            _backButton.Click += OnBackButtonClick;
            _danmakuBarVisibilityButton.Click += OnDanmakuBarVisibilityButtonClick;
            _homeButton.Click += OnHomeButtonClickAsync;
            _backToDefaultButton.Click += OnBackButtonClick;
            _liveRefreshButton.Click += OnLiveRefreshButtonClickAsync;
            _previousEpisodeButton.Click += OnPreviousEpisodeButtonClickAsync;
            _nextEpisodeButton.Click += OnNextEpisodeButtonClickAsync;
            _screenshotButton.Click += OnScreenshotButtonClickAsync;
            _playbackRateNodeComboBox.SelectionChanged += OnPlaybackRateNodeComboBoxSelectionChanged;
            _increasePlayRateButton.Click += OnIncreasePlayRateButtonClick;
            _decreasePlayRateButton.Click += OnDecreasePlayRateButtonClick;
            _increaseVolumeButton.Click += OnIncreaseVolumeButtonClick;
            _decreaseVolumeButton.Click += OnDecreaseVolumeButtonClick;
            _previousViewInformer.ActionClick += OnContinuePreviousViewButtonClickAsync;
            _nextVidepInformer.ActionClick += OnPlayNextVideoButtonClickAsync;
            _miniViewButton.Click += OnPlayModeButtonClick;

            if (_formatListView != null)
            {
                _formatListView.SelectionChanged += OnFormatListViewSelectionChangedAsync;
            }

            if (_liveQualityListView != null)
            {
                _liveQualityListView.SelectionChanged += OnLiveQualityListViewSelectionChangedAsync;
            }

            if (_livePlayLineListView != null)
            {
                _livePlayLineListView.SelectionChanged += OnLivePlayLineListViewSelectionChangedAsync;
            }

            if (_backSkipButton != null)
            {
                _backSkipButton.Click += OnBackSkipButtonClick;
            }

            if (_forwardSkipButton != null)
            {
                _forwardSkipButton.Click += OnForwardSkipButtonClick;
            }

            _visibilityStateGroup = VisualStateManager.GetVisualStateGroups(_rootGrid).FirstOrDefault(p => p.Name == VisibilityStateGroupName);

            DanmakuViewModel.DanmakuListAdded += OnDanmakuListAdded;
            DanmakuViewModel.RequestClearDanmaku += OnRequestClearDanmaku;
            DanmakuViewModel.SendDanmakuSucceeded += OnSendDanmakuSucceeded;
            DanmakuViewModel.PropertyChanged += OnDanmakuViewModelPropertyChangedAsync;
            ViewModel.MediaPlayerUpdated += OnMediaPlayerUdpated;
            ViewModel.PropertyChanged += OnViewModelPropertyChangedAsync;
            ViewModel.NewLiveDanmakuAdded += OnNewLiveDanmakuAdded;

            await CheckCurrentPlayerModeAsync();
            CheckSubtitleZoom();

            base.OnApplyTemplate();
        }

        /// <inheritdoc/>
        protected override void OnPointerEntered(PointerRoutedEventArgs e)
        {
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 0);
            if (!IsControlPanelShown() && e.Pointer.PointerDeviceType == Windows.Devices.Input.PointerDeviceType.Mouse)
            {
                Show();
            }

            _cursorStayTime = 0;
        }

        /// <inheritdoc/>
        protected override void OnPointerMoved(PointerRoutedEventArgs e)
        {
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 0);

            if (!_cursorTimer.IsEnabled)
            {
                _cursorTimer.Start();
            }

            if (!IsControlPanelShown() && e.Pointer.PointerDeviceType == Windows.Devices.Input.PointerDeviceType.Mouse)
            {
                Show();
            }

            _cursorStayTime = 0;
        }

        /// <inheritdoc/>
        protected override void OnPointerExited(PointerRoutedEventArgs e)
        {
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 0);

            if (IsControlPanelShown() && e.Pointer.PointerDeviceType == Windows.Devices.Input.PointerDeviceType.Mouse)
            {
                HideControlsAsync();
            }

            _cursorStayTime = 0;
        }

        private void OnSendDanmakuSucceeded(object sender, string e)
        {
            var model = new DanmakuItem
            {
                StartMs = (uint)ViewModel.BiliPlayer.MediaPlayer.PlaybackSession.Position.TotalMilliseconds,
                Mode = (DanmakuMode)((int)DanmakuViewModel.Location),
                TextColor = Microsoft.Toolkit.Uwp.Helpers.ColorHelper.ToColor(DanmakuViewModel.Color),
                BaseFontSize = DanmakuViewModel.IsStandardSize ? 25 : 18,
                Text = e,
                HasOutline = true,
            };

            _danmakuView.SendDanmu(model);
        }

        private void OnInteractionControlPointerCanceled(object sender, PointerRoutedEventArgs e)
            => _gestureRecognizer.ProcessUpEvent(e.GetCurrentPoint(this));

        private void OnInteractionControlPointerReleased(object sender, PointerRoutedEventArgs e)
            => _gestureRecognizer.ProcessUpEvent(e.GetCurrentPoint(this));

        private void OnInteractionControlPointerMoved(object sender, PointerRoutedEventArgs e)
            => _gestureRecognizer.ProcessMoveEvents(e.GetIntermediatePoints(this));

        private void OnInteractionControlPointerPressed(object sender, PointerRoutedEventArgs e)
            => _gestureRecognizer.ProcessDownEvent(e.GetCurrentPoint(this));

        private void OnBackButtonClick(object sender, RoutedEventArgs e)
        {
            ViewModel.PlayerDisplayMode = PlayerDisplayMode.Default;
        }

        private async void OnLiveRefreshButtonClickAsync(object sender, RoutedEventArgs e)
        {
            if (ViewModel.CurrentPlayUrl != null)
            {
                await ViewModel.ChangeAppLivePlayLineAsync(ViewModel.CurrentPlayUrl.Data);
            }
        }

        private void OnNewLiveDanmakuAdded(object sender, LiveDanmakuMessage e)
        {
            if (_danmakuView != null)
            {
                var myName = Splat.Locator.Current.GetService<AccountViewModel>().DisplayName;
                var isOwn = !string.IsNullOrEmpty(myName) && myName == e.UserName;
                var model = new DanmakuItem
                {
                    StartMs = 0,
                    Mode = DanmakuMode.Rolling,
                    TextColor = Microsoft.Toolkit.Uwp.Helpers.ColorHelper.ToColor(e.ContentColor ?? "#FFFFFF"),
                    BaseFontSize = DanmakuViewModel.IsStandardSize ? 25 : 18,
                    Text = e.Text,
                    HasOutline = isOwn,
                };

                _danmakuView.SendDanmu(model);
            }
        }

        private async void OnFormatListViewSelectionChangedAsync(object sender, SelectionChangedEventArgs e)
        {
            if (_formatListView.SelectedItem is VideoFormatViewModel item && item.Data.Quality != ViewModel.CurrentFormat?.Quality)
            {
                await ViewModel.ChangeFormatAsync(item.Data.Quality);
                _formatButton?.Flyout?.Hide();
            }
        }

        private async void OnLivePlayLineListViewSelectionChangedAsync(object sender, SelectionChangedEventArgs e)
        {
            if (_livePlayLineListView.SelectedItem is LiveAppPlayLineViewModel data2 && ViewModel.CurrentPlayUrl != data2)
            {
                await ViewModel.ChangeAppLivePlayLineAsync(data2.Data);
            }
        }

        private async void OnLiveQualityListViewSelectionChangedAsync(object sender, SelectionChangedEventArgs e)
        {
            if (_liveQualityListView.SelectedItem is LiveAppQualityViewModel data2 && ViewModel.CurrentAppLiveQuality != data2.Data)
            {
                await ViewModel.ChangeLivePlayBehaviorAsync(data2.Data.Quality);
                _liveQualityButton?.Flyout?.Hide();
            }
        }

        private void OnInteractionControlTapped(object sender, TappedRoutedEventArgs e)
        {
            if (_isHolding)
            {
                _isHolding = false;
                return;
            }

            if (e.PointerDeviceType == Windows.Devices.Input.PointerDeviceType.Mouse)
            {
                _isTouch = false;
                ViewModel.TogglePlayPause();
            }
            else
            {
                _isTouch = true;
                Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 0);
            }
        }

        private void OnInteractionControlDoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            var playerStatus = ViewModel.PlayerStatus;
            var canDoubleTapped = playerStatus == PlayerStatus.Playing || playerStatus == PlayerStatus.Pause;
            if (canDoubleTapped)
            {
                if (e.PointerDeviceType == Windows.Devices.Input.PointerDeviceType.Mouse)
                {
                    ViewModel.PlayerDisplayMode = ViewModel.PlayerDisplayMode == PlayerDisplayMode.Default
                        ? PlayerDisplayMode.FullScreen
                        : PlayerDisplayMode.Default;
                }

                ViewModel.TogglePlayPause();
            }
        }

        private void OnForwardSkipButtonClick(object sender, RoutedEventArgs e)
        {
            var forwardSeconds = SettingViewModel.SingleFastForwardAndRewindSpan;
            ViewModel.ForwardSkip(forwardSeconds);
        }

        private void OnBackSkipButtonClick(object sender, RoutedEventArgs e)
        {
            var backSeconds = SettingViewModel.SingleFastForwardAndRewindSpan;
            ViewModel.BackSkip(backSeconds);
        }

        private void OnPlayModeButtonClick(object sender, RoutedEventArgs e)
        {
            PlayerDisplayMode mode = default;
            switch ((sender as FrameworkElement).Name)
            {
                case FullWindowPlayModeButtonName:
                    _fullScreenPlayModeButton.IsChecked = false;
                    _compactOverlayPlayModeButton.IsChecked = false;
                    mode = _fullWindowPlayModeButton.IsChecked.Value ?
                        PlayerDisplayMode.FullWindow : PlayerDisplayMode.Default;
                    break;
                case FullScreenPlayModeButtonName:
                    _fullWindowPlayModeButton.IsChecked = false;
                    _compactOverlayPlayModeButton.IsChecked = false;
                    mode = _fullScreenPlayModeButton.IsChecked.Value ?
                        PlayerDisplayMode.FullScreen : PlayerDisplayMode.Default;
                    break;
                case CompactOverlayPlayModeButtonName:
                    _fullScreenPlayModeButton.IsChecked = false;
                    _fullWindowPlayModeButton.IsChecked = false;
                    mode = _compactOverlayPlayModeButton.IsChecked.Value ?
                        PlayerDisplayMode.CompactOverlay : PlayerDisplayMode.Default;
                    break;
                case MiniViewButtonName:
                    _fullScreenPlayModeButton.IsChecked = false;
                    _fullWindowPlayModeButton.IsChecked = false;
                    mode = ApplicationView.GetForCurrentView().ViewMode == ApplicationViewMode.CompactOverlay ?
                        PlayerDisplayMode.Default : PlayerDisplayMode.CompactOverlay;
                    break;
                default:
                    break;
            }

            ViewModel.PlayerDisplayMode = mode;
        }

        private async void OnHomeButtonClickAsync(object sender, RoutedEventArgs e)
            => await ViewModel.BackToInteractionStartAsync();

        private async void OnContinuePreviousViewButtonClickAsync(object sender, EventArgs e)
        {
            ViewModel.IsShowHistoryTip = false;
            ViewModel.HistoryTipText = string.Empty;
            await ViewModel.JumpToHistoryAsync();
        }

        private async void OnNextEpisodeButtonClickAsync(object sender, RoutedEventArgs e)
        {
            if (ViewModel.IsPgc)
            {
                var next = ViewModel.EpisodeCollection.Where(p => p.Data.Index == ViewModel.CurrentPgcEpisode.Index + 1).FirstOrDefault();
                if (next != null)
                {
                    await ViewModel.ChangePgcEpisodeAsync(next.Data.Id);
                }
            }
            else if (ViewModel.IsShowUgcSection)
            {
                var index = ViewModel.CurrentUgcSection.Episodes.IndexOf(ViewModel.CurrentUgcEpisode);
                if (index > -1 && index < ViewModel.CurrentUgcSection.Episodes.Count - 1)
                {
                    var next = ViewModel.CurrentUgcSection.Episodes[index + 1];
                    await ViewModel.LoadAsync(new VideoViewModel(next));
                }
            }
            else if (ViewModel.IsShowParts)
            {
                var part = ViewModel.VideoPartCollection.FirstOrDefault(p => p.Data.Page.Page_ == ViewModel.CurrentVideoPart.Page.Page_ + 1);
                if (part != null)
                {
                    var id = part.Data.Page.Cid;
                    await ViewModel.ChangeVideoPartAsync(id);
                }
            }
        }

        private async void OnPreviousEpisodeButtonClickAsync(object sender, RoutedEventArgs e)
        {
            if (ViewModel.IsPgc)
            {
                var prev = ViewModel.EpisodeCollection.Where(p => p.Data.Index == ViewModel.CurrentPgcEpisode.Index - 1).FirstOrDefault();
                if (prev != null)
                {
                    await ViewModel.ChangePgcEpisodeAsync(prev.Data.Id);
                }
            }
            else if (ViewModel.IsShowUgcSection)
            {
                var index = ViewModel.CurrentUgcSection.Episodes.IndexOf(ViewModel.CurrentUgcEpisode);
                if (index > 0)
                {
                    var prev = ViewModel.CurrentUgcSection.Episodes[index - 1];
                    await ViewModel.LoadAsync(new VideoViewModel(prev));
                }
            }
            else if (ViewModel.IsShowParts)
            {
                var part = ViewModel.VideoPartCollection.FirstOrDefault(p => p.Data.Page.Page_ == ViewModel.CurrentVideoPart.Page.Page_ - 1);
                if (part != null)
                {
                    var id = part.Data.Page.Cid;
                    await ViewModel.ChangeVideoPartAsync(id);
                }
            }
        }

        private async void OnScreenshotButtonClickAsync(object sender, RoutedEventArgs e)
            => await ViewModel.ScreenshotAsync();

        private void OnDanmakuListAdded(object sender, IEnumerable<DanmakuInformation> e)
        {
            InitializeDanmaku(e);
            _danmakuTimer.Start();
        }

        private void OnRequestClearDanmaku(object sender, EventArgs e)
        {
            _segmentIndex = 1;
            _danmakuTimer.Stop();
            _danmakuView?.ClearAll();
        }

        private void OnMediaPlayerUdpated(object sender, EventArgs e)
        {
            var player = ViewModel.BiliPlayer.MediaPlayer;
            if (player != null && player.PlaybackSession != null)
            {
                player.PlaybackSession.PlaybackStateChanged += OnPlaybackStateChangedAsync;
            }
        }

        private async void OnViewModelPropertyChangedAsync(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ViewModel.CurrentFormat))
            {
                if (ViewModel.CurrentFormat != null &&
                    _formatListView != null &&
                    (_formatListView.SelectedItem == null ||
                    (_formatListView.SelectedItem as VideoFormatViewModel).Data.Quality != ViewModel.CurrentFormat.Quality))
                {
                    _formatListView.SelectedItem = ViewModel.FormatCollection.Where(p => p.Data.Quality == ViewModel.CurrentFormat.Quality).FirstOrDefault();
                }
            }
            else if (e.PropertyName == nameof(ViewModel.CurrentAppLiveQuality))
            {
                if (ViewModel.CurrentAppLiveQuality != null &&
                    _liveQualityListView != null &&
                    (_liveQualityListView.SelectedItem == null ||
                    (_liveQualityListView.SelectedItem as LiveAppQualityViewModel).Data.Quality != ViewModel.CurrentAppLiveQuality.Quality))
                {
                    _liveQualityListView.SelectedItem = ViewModel.LiveAppQualityCollection.Where(p => p.Data.Quality == ViewModel.CurrentAppLiveQuality.Quality).FirstOrDefault();
                }
            }
            else if (e.PropertyName == nameof(ViewModel.CurrentPlayUrl))
            {
                if (ViewModel.CurrentPlayUrl != null &&
                    _liveQualityListView != null &&
                    (_livePlayLineListView.SelectedItem == null ||
                    (_livePlayLineListView.SelectedItem as LiveAppPlayLineViewModel).Data.Host != ViewModel.CurrentPlayUrl.Data.Host))
                {
                    _livePlayLineListView.SelectedItem = ViewModel.LiveAppPlayLineCollection.Where(p => p.Data.Host == ViewModel.CurrentPlayUrl.Data.Host).FirstOrDefault();
                }
            }
            else if (e.PropertyName == nameof(ViewModel.PlayerDisplayMode))
            {
                await CheckCurrentPlayerModeAsync();
            }
        }

        private async void OnDanmakuViewModelPropertyChangedAsync(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(DanmakuViewModel.IsShowDanmaku))
            {
                if (DanmakuViewModel.IsShowDanmaku)
                {
                    await _danmakuView.RedrawAsync();
                }
                else
                {
                    _danmakuView.PauseDanmaku();
                }
            }
        }

        private async void OnPlaybackStateChangedAsync(MediaPlaybackSession sender, object args)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                if (sender.PlaybackState == MediaPlaybackState.Buffering)
                {
                    _danmakuView?.PauseDanmaku();
                }
                else if (sender.PlaybackState == MediaPlaybackState.Paused && sender.Position < sender.NaturalDuration)
                {
                    _danmakuView?.PauseDanmaku();
                }
                else if (sender.PlaybackState == MediaPlaybackState.Playing)
                {
                    _danmakuView?.ResumeDanmaku();
                    HideControlsAsync();
                }

                _playPauseButton?.Focus(FocusState.Programmatic);
            });
        }

        private void OnDanmakuBarVisibilityButtonClick(object sender, RoutedEventArgs e)
            => ViewModel.IsShowDanmakuBar = !ViewModel.IsShowDanmakuBar;

        private void InitializeDanmakuTimer()
        {
            if (_danmakuTimer == null)
            {
                _danmakuTimer = new DispatcherTimer();
                _danmakuTimer.Interval = TimeSpan.FromSeconds(0.1);
                _danmakuTimer.Tick += OnDanmkuTimerTickAsync;
            }
        }

        private void InitializeCursorTimer()
        {
            if (_cursorTimer == null)
            {
                _cursorTimer = new DispatcherTimer();
                _cursorTimer.Interval = TimeSpan.FromSeconds(0.5);
                _cursorTimer.Tick += OnCursorTimerTickAsync;
            }
        }

        private void InitializeNormalTimer()
        {
            if (_normalTimer == null)
            {
                _normalTimer = new DispatcherTimer();
                _normalTimer.Interval = TimeSpan.FromSeconds(0.5);
                _normalTimer.Tick += OnNormalTimerTickAsync;
            }
        }

        private void InitializeFocusTimer()
        {
            if (_focusTimer == null)
            {
                _focusTimer = new DispatcherTimer();
                _focusTimer.Interval = TimeSpan.FromSeconds(5);
                _focusTimer.Tick += OnFocusTimerTick;
            }
        }

        private void InitializeDanmaku(IEnumerable<DanmakuInformation> elements)
            => _danmakuView.Prepare(BilibiliDanmakuXmlParser.GetDanmakuList(elements, DanmakuViewModel.IsDanmakuMerge), true);

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            CheckSubtitleZoom();

            DanmakuViewModel.CanShowDanmaku = e.NewSize.Width >= 480;
            IsCompact = e.NewSize.Width < 480;
        }

        private void CheckSubtitleZoom()
        {
            if (ActualWidth == 0 || ActualHeight == 0 || _subtitleBlock == null)
            {
                return;
            }

            var baseWidth = 800d;
            var baseHeight = 600d;
            var scale = Math.Min(ActualWidth / baseWidth, ActualHeight / baseHeight);
            if (scale > 2.0)
            {
                scale = 2.0;
            }
            else if (scale < 0.4)
            {
                scale = 0.4;
            }

            _subtitleBlock.FontSize = 24 * scale;
        }

        private async void OnDanmkuTimerTickAsync(object sender, object e)
        {
            if (ViewModel.BiliPlayer == null || _danmakuView == null)
            {
                return;
            }

            var player = ViewModel.BiliPlayer.MediaPlayer;

            if (player == null || player.PlaybackSession == null)
            {
                return;
            }

            var position = player.PlaybackSession.Position.TotalSeconds;

            var segmentIndex = Convert.ToInt32(Math.Ceiling(position / 360d));
            if (segmentIndex < 1)
            {
                segmentIndex = 1;
            }

            if (segmentIndex != _segmentIndex)
            {
                var oldSegmentIndex = _segmentIndex;
                _segmentIndex = segmentIndex;
                try
                {
                    await Task.CompletedTask;
                }
                catch (Exception)
                {
                    _segmentIndex = oldSegmentIndex;
                }
            }

            if (player.PlaybackSession.PlaybackState != MediaPlaybackState.Playing)
            {
                return;
            }

            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                _danmakuView.UpdateTime(Convert.ToUInt32(position * 1000));
            });
        }

        private void OnCursorTimerTickAsync(object sender, object e)
        {
            _cursorStayTime += 500;
            if (_cursorStayTime > 2000)
            {
                if (_isTouch && IsControlPanelShown())
                {
                    HideControlsAsync();
                }
                else if (IsCursorInMediaElement() && !IsCursorInControlPanel())
                {
                    Window.Current.CoreWindow.PointerCursor = null;
                    if (IsControlPanelShown())
                    {
                        HideControlsAsync();
                    }
                }
                else if (!ViewModel.IsPointerInMediaElement && IsControlPanelShown())
                {
                    HideControlsAsync();
                }

                _cursorStayTime = 0;
            }
        }

        private async void OnNormalTimerTickAsync(object sender, object e)
        {
            if (_tempMessageHoldSeconds >= 2)
            {
                HideTempMessage();
            }
            else if (_tempMessageHoldSeconds != -1)
            {
                _tempMessageHoldSeconds += 0.5;
            }

            if (ViewModel.IsShowHistoryTip)
            {
                _historyMessageHoldSeconds += 0.5;
                if (_historyMessageHoldSeconds > 4)
                {
                    ViewModel.IsShowHistoryTip = false;
                    _historyMessageHoldSeconds = 0;
                }
            }
            else
            {
                _historyMessageHoldSeconds = 0;
            }

            if (ViewModel.IsShowNextVideoTip)
            {
                _nextVideoHoldSeconds += 0.5;

                if (_nextVideoHoldSeconds > 5)
                {
                    _nextVideoHoldSeconds = 0;
                    ViewModel.NextVideoCountdown = 0;
                    ViewModel.IsShowNextVideoTip = false;

                    await ViewModel.PlayNextVideoAsync();
                }
                else
                {
                    ViewModel.NextVideoCountdown = Math.Ceiling(5 - _nextVideoHoldSeconds);
                }
            }
            else
            {
                _nextVideoHoldSeconds = 0;
            }
        }

        private void OnFocusTimerTick(object sender, object e)
        {
            if (ViewModel.PlayerDisplayMode != PlayerDisplayMode.Default)
            {
                if (IsControlPanelShown() && ViewModel.IsFocusInputControl)
                {
                    return;
                }

                _playPauseButton?.Focus(FocusState.Programmatic);
            }
        }

        private void OnInteractionControlManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            _manipulationVolume = 0;
            _manipulationProgress = 0;
            _manipulationDeltaX = 0;
            _manipulationDeltaY = 0;
            _manipulationType = PlayerManipulationType.None;

            if (_manipulationBeforeIsPlay)
            {
                ViewModel.BiliPlayer.MediaPlayer.Play();
            }

            _manipulationBeforeIsPlay = false;
        }

        private async void OnGestureRecognizerHoldingAsync(GestureRecognizer sender, HoldingEventArgs args)
        {
            if (args.ContactCount == 1)
            {
                _isHolding = true;
                if (args.HoldingState == HoldingState.Started)
                {
                    // 开启倍速播放.
                    var isStarted = await ViewModel.StartTempQuickPlayAsync();
                    if (isStarted)
                    {
                        var resourceToolkit = ServiceLocator.Instance.GetService<IResourceToolkit>();
                        ShowTempMessage(resourceToolkit.GetLocaleString(LanguageNames.StartQuickPlay));
                    }
                }
                else
                {
                    // 停止倍速播放.
                    await ViewModel.StopTempQuickPlayAsync();
                    HideTempMessage();
                }
            }
        }

        private void OnInteractionControlManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            if (ViewModel.PlayerStatus != PlayerStatus.Playing && ViewModel.PlayerStatus != PlayerStatus.Pause)
            {
                return;
            }

            _manipulationDeltaX += e.Delta.Translation.X;
            _manipulationDeltaY -= e.Delta.Translation.Y;
            if (Math.Abs(_manipulationDeltaX) > 15 || Math.Abs(_manipulationDeltaY) > 15)
            {
                var resourceToolkit = ServiceLocator.Instance.GetService<IResourceToolkit>();
                if (_manipulationType == PlayerManipulationType.None)
                {
                    var isVolume = Math.Abs(_manipulationDeltaY) > Math.Abs(_manipulationDeltaX);
                    _manipulationType = isVolume ? PlayerManipulationType.Volume : PlayerManipulationType.Progress;
                    if (!isVolume)
                    {
                        ViewModel.BiliPlayer.MediaPlayer.Pause();
                    }
                }

                if (_manipulationType == PlayerManipulationType.Volume)
                {
                    var volume = _manipulationVolume + (_manipulationDeltaY / 2.0);
                    if (volume > 100)
                    {
                        volume = 100;
                    }
                    else if (volume < 0)
                    {
                        volume = 0;
                    }

                    ShowTempMessage($"{resourceToolkit.GetLocaleString(LanguageNames.CurrentVolume)}: {Math.Round(volume)}");
                    ViewModel.BiliPlayer.MediaPlayer.Volume = volume / 100.0;
                    if (volume == 0)
                    {
                        ShowTempMessage(resourceToolkit.GetLocaleString(LanguageNames.Muted));
                    }
                }
                else
                {
                    var progress = _manipulationProgress + (_manipulationDeltaX * _manipulationUnitLength);
                    if (progress > ViewModel.BiliPlayer.MediaPlayer.PlaybackSession.NaturalDuration.TotalSeconds)
                    {
                        progress = ViewModel.BiliPlayer.MediaPlayer.PlaybackSession.NaturalDuration.TotalSeconds;
                    }
                    else if (progress < 0)
                    {
                        progress = 0;
                    }

                    ShowTempMessage($"{resourceToolkit.GetLocaleString(LanguageNames.CurrentProgress)}: {TimeSpan.FromSeconds(progress):g}");
                    ViewModel.BiliPlayer.MediaPlayer.PlaybackSession.Position = TimeSpan.FromSeconds(progress);
                }
            }
        }

        private void OnInteractionControlManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            var player = ViewModel.BiliPlayer.MediaPlayer;
            _manipulationProgress = player.PlaybackSession.Position.TotalSeconds;
            _manipulationVolume = player.Volume * 100.0;
            _manipulationBeforeIsPlay = ViewModel.PlayerStatus == PlayerStatus.Playing;
            if (player.PlaybackSession != null && player.PlaybackSession.NaturalDuration.TotalSeconds > 0)
            {
                // 获取单位像素对应的时长
                var unit = player.PlaybackSession.NaturalDuration.TotalSeconds / ActualWidth;
                _manipulationUnitLength = unit / 1.5;
            }
        }

        private void OnPlaybackRateNodeComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = _playbackRateNodeComboBox.SelectedItem;
            if (item is double rate)
            {
                ViewModel.PlaybackRate = rate;
                _playbackRateNodeComboBox.SelectedItem = null;
            }
        }

        private void OnDecreasePlayRateButtonClick(object sender, RoutedEventArgs e)
        {
            if (!ViewModel.IsLive)
            {
                var resourceToolkit = ServiceLocator.Instance.GetService<IResourceToolkit>();
                var rate = ViewModel.PlaybackRate - ViewModel.PlaybackRateStep;
                if (rate < 0.5)
                {
                    rate = 0.5;
                }

                ViewModel.PlaybackRate = rate;
                ShowTempMessage($"{resourceToolkit.GetLocaleString(LanguageNames.CurrentPlaybackRate)}: {rate}x");
            }
        }

        private void OnIncreasePlayRateButtonClick(object sender, RoutedEventArgs e)
        {
            if (!ViewModel.IsLive)
            {
                var resourceToolkit = ServiceLocator.Instance.GetService<IResourceToolkit>();
                var rate = ViewModel.PlaybackRate + ViewModel.PlaybackRateStep;
                if (rate > ViewModel.MaxPlaybackRate)
                {
                    rate = ViewModel.MaxPlaybackRate;
                }

                ViewModel.PlaybackRate = rate;
                ShowTempMessage($"{resourceToolkit.GetLocaleString(LanguageNames.CurrentPlaybackRate)}: {rate}x");
            }
        }

        private void OnDecreaseVolumeButtonClick(object sender, RoutedEventArgs e)
        {
            if (ViewModel.BiliPlayer?.MediaPlayer != null)
            {
                var resourceToolkit = ServiceLocator.Instance.GetService<IResourceToolkit>();
                var volume = ViewModel.BiliPlayer.MediaPlayer.Volume - 0.05;
                if (volume < 0)
                {
                    volume = 0;
                }

                ViewModel.BiliPlayer.MediaPlayer.Volume = volume;
                ShowTempMessage($"{resourceToolkit.GetLocaleString(LanguageNames.CurrentVolume)}: {Math.Round(volume * 100)}");
            }
        }

        private void OnIncreaseVolumeButtonClick(object sender, RoutedEventArgs e)
        {
            if (ViewModel.BiliPlayer?.MediaPlayer != null)
            {
                var resourceToolkit = ServiceLocator.Instance.GetService<IResourceToolkit>();
                var volume = ViewModel.BiliPlayer.MediaPlayer.Volume + 0.05;
                if (volume > 1)
                {
                    volume = 1;
                }

                ViewModel.BiliPlayer.MediaPlayer.Volume = volume;
                ShowTempMessage($"{resourceToolkit.GetLocaleString(LanguageNames.CurrentVolume)}: {Math.Round(volume * 100)}");
            }
        }

        private async void OnPlayNextVideoButtonClickAsync(object sender, EventArgs e)
        {
            ViewModel.IsShowNextVideoTip = false;
            _nextVideoHoldSeconds = 0;
            await ViewModel.PlayNextVideoAsync();
        }

        private void ShowTempMessage(string message)
        {
            _tempMessageContainer.Visibility = Visibility.Visible;
            _tempMessageBlock.Text = message;
            _tempMessageHoldSeconds = 0;
        }

        private void HideTempMessage()
        {
            _tempMessageContainer.Visibility = Visibility.Collapsed;
            _tempMessageBlock.Text = string.Empty;
            _tempMessageHoldSeconds = -1;
        }

        private bool IsControlPanelShown()
        {
            if (_visibilityStateGroup == null)
            {
                _visibilityStateGroup = VisualStateManager.GetVisualStateGroups(_rootGrid).FirstOrDefault(p => p.Name == VisibilityStateGroupName);
            }

            return _visibilityStateGroup?.CurrentState?.Name == "ControlPanelFadeIn";
        }

        private bool IsCursorInControlPanel()
        {
            if (IsControlPanelShown() && ViewModel.IsPointerInMediaElement)
            {
                var pointerPosition = Window.Current.CoreWindow.PointerPosition;
                pointerPosition.X -= Window.Current.Bounds.X;
                pointerPosition.Y -= Window.Current.Bounds.Y;
                var rect = new Rect(0, 0, ActualWidth, ActualHeight);
                var controlPanelBounds = _controlPanel.TransformToVisual(Window.Current.Content)
                    .TransformBounds(rect);
                return controlPanelBounds.Contains(pointerPosition);
            }

            return false;
        }

        private bool IsCursorInMediaElement()
        {
            if (!ViewModel.IsPointerInMediaElement)
            {
                return false;
            }

            var pointerPosition = Window.Current.CoreWindow.PointerPosition;
            pointerPosition.X -= Window.Current.Bounds.X;
            pointerPosition.Y -= Window.Current.Bounds.Y;
            var rect = new Rect(0, 0, ActualWidth, ActualHeight);
            var rootBounds = _rootGrid.TransformToVisual(Window.Current.Content)
                    .TransformBounds(rect);
            return rootBounds.Contains(pointerPosition);
        }

        private async void HideControlsAsync()
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                Hide();
                ViewModel.IsFocusInputControl = false;
            });
        }
    }
}
