// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Bilibili.Community.Service.Dm.V1;
using Richasy.Bili.App.Resources.Extension;
using Richasy.Bili.Locator.Uwp;
using Richasy.Bili.Models.BiliBili;
using Richasy.Bili.Models.Enums;
using Richasy.Bili.Models.Enums.App;
using Richasy.Bili.Toolkit.Interfaces;
using Richasy.Bili.ViewModels.Uwp;
using Windows.Foundation;
using Windows.Media.Playback;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Shapes;

namespace Richasy.Bili.App.Controls
{
    /// <summary>
    /// 哔哩播放器的媒体传输控件.
    /// </summary>
    public partial class BiliPlayerTransportControls : MediaTransportControls
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BiliPlayerTransportControls"/> class.
        /// </summary>
        public BiliPlayerTransportControls()
        {
            DefaultStyleKey = typeof(BiliPlayerTransportControls);
            ShowAndHideAutomatically = false;
            _danmakuDictionary = new Dictionary<int, List<DanmakuModel>>();
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

        /// <summary>
        /// 检查后退操作.
        /// </summary>
        /// <returns>是否处理了后退操作.</returns>
        public bool CheckBack()
        {
            if (ViewModel.PlayerDisplayMode != PlayerDisplayMode.Default)
            {
                ViewModel.PlayerDisplayMode = PlayerDisplayMode.Default;
                return true;
            }

            return false;
        }

        /// <inheritdoc/>
        protected override void OnApplyTemplate()
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
            _rootGrid = GetTemplateChild(RootGridName) as Grid;

            _fullWindowPlayModeButton.Click += OnPlayModeButtonClick;
            _fullScreenPlayModeButton.Click += OnPlayModeButtonClick;
            _compactOverlayPlayModeButton.Click += OnPlayModeButtonClick;
            _interactionControl.ManipulationMode = ManipulationModes.TranslateX | ManipulationModes.TranslateY;
            _interactionControl.Tapped += OnInteractionControlTapped;
            _interactionControl.DoubleTapped += OnInteractionControlDoubleTapped;
            _interactionControl.ManipulationStarted += OnInteractionControlManipulationStarted;
            _interactionControl.ManipulationDelta += OnInteractionControlManipulationDelta;
            _interactionControl.ManipulationCompleted += OnInteractionControlManipulationCompleted;
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
            DanmakuViewModel.PropertyChanged += OnDanmakuViewModelPropertyChanged;
            DanmakuViewModel.SendDanmakuSucceeded += OnSendDanmakuSucceeded;
            ViewModel.MediaPlayerUpdated += OnMediaPlayerUdpated;
            ViewModel.PropertyChanged += OnViewModelPropertyChanged;
            ViewModel.NewLiveDanmakuAdded += OnNewLiveDanmakuAdded;
            AppViewModel.Instance.PropertyChanged += OnAppViewModelPropertyChanged;

            CheckCurrentPlayerMode();
            CheckDanmakuZoom();
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
                Hide();
            }

            _cursorStayTime = 0;
        }

        private void OnSendDanmakuSucceeded(object sender, string e)
        {
            var model = new DanmakuModel
            {
                Color = Microsoft.Toolkit.Uwp.Helpers.ColorHelper.ToColor(DanmakuViewModel.Color),
                Size = DanmakuViewModel.IsStandardSize ? 25 : 18,
                Text = e,
                Location = DanmakuViewModel.Location,
            };

            _danmakuView.AddScreenDanmaku(model, true);
        }

        private void OnBackButtonClick(object sender, RoutedEventArgs e)
        {
            ViewModel.PlayerDisplayMode = PlayerDisplayMode.Default;
        }

        private async void OnLiveRefreshButtonClickAsync(object sender, RoutedEventArgs e)
        {
            if (ViewModel.CurrentPlayLine != null)
            {
                await ViewModel.ChangeLivePlayLineAsync(ViewModel.CurrentPlayLine.Order);
            }
        }

        private void OnNewLiveDanmakuAdded(object sender, LiveDanmakuMessage e)
        {
            if (_danmakuView != null)
            {
                var myName = AccountViewModel.Instance.DisplayName;
                var isOwn = !string.IsNullOrEmpty(myName) && myName == e.UserName;
                _danmakuView.AddLiveDanmakuAsync(e.Text, isOwn, e.ContentColor?.ToColor());
            }
        }

        private async void OnFormatListViewSelectionChangedAsync(object sender, SelectionChangedEventArgs e)
        {
            if (_formatListView.SelectedItem is VideoFormatViewModel item && item.Data.Quality != ViewModel.CurrentFormat?.Quality)
            {
                await ViewModel.ChangeFormatAsync(item.Data.Quality);
                _formatButton.Flyout.Hide();
            }
        }

        private async void OnLivePlayLineListViewSelectionChangedAsync(object sender, SelectionChangedEventArgs e)
        {
            if (_livePlayLineListView.SelectedItem is LivePlayLineViewModel data && ViewModel.CurrentPlayLine != data.Data)
            {
                await ViewModel.ChangeLivePlayLineAsync(data.Data.Order);
            }
        }

        private async void OnLiveQualityListViewSelectionChangedAsync(object sender, SelectionChangedEventArgs e)
        {
            if (_liveQualityListView.SelectedItem is LiveQualityViewModel data && ViewModel.CurrentLiveQuality != data.Data)
            {
                await ViewModel.ChangeLiveQualityAsync(data.Data.Quality);
                _liveQualityButton.Flyout.Hide();
            }
        }

        private void OnInteractionControlTapped(object sender, TappedRoutedEventArgs e)
        {
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
            var btn = sender as ToggleButton;
            PlayerDisplayMode mode = default;
            switch (btn.Name)
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
                default:
                    break;
            }

            ViewModel.PlayerDisplayMode = mode;
        }

        private async void OnHomeButtonClickAsync(object sender, RoutedEventArgs e)
        {
            await ViewModel.BackToInteractionStartAsync();
        }

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
            else
            {
                var next = ViewModel.VideoPartCollection.Where(p => p.Data.Page.Page_ == ViewModel.CurrentVideoPart.Page.Page_ + 1).FirstOrDefault();
                if (next != null)
                {
                    await ViewModel.ChangeVideoPartAsync(next.Data.Page.Cid);
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
            else
            {
                var prev = ViewModel.VideoPartCollection.Where(p => p.Data.Page.Page_ == ViewModel.CurrentVideoPart.Page.Page_ - 1).FirstOrDefault();
                if (prev != null)
                {
                    await ViewModel.ChangeVideoPartAsync(prev.Data.Page.Cid);
                }
            }
        }

        private async void OnScreenshotButtonClickAsync(object sender, RoutedEventArgs e)
            => await ViewModel.ScreenshotAsync();

        private void CheckCurrentPlayerMode()
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

            _playPauseButton.Focus(FocusState.Programmatic);
            _backToDefaultButton.Visibility = ViewModel.PlayerDisplayMode != PlayerDisplayMode.Default
                ? Visibility.Visible
                : Visibility.Collapsed;
        }

        private void OnDanmakuListAdded(object sender, List<DanmakuElem> e)
        {
            InitializeDanmaku(e);
            _danmakuTimer.Start();
        }

        private void OnRequestClearDanmaku(object sender, EventArgs e)
        {
            _segmentIndex = 1;
            _danmakuDictionary.Clear();
            _danmakuTimer.Stop();
            _danmakuView.ClearAll();
        }

        private void OnMediaPlayerUdpated(object sender, EventArgs e)
        {
            var player = ViewModel.BiliPlayer.MediaPlayer;
            if (player != null && player.PlaybackSession != null)
            {
                player.PlaybackSession.PlaybackStateChanged += OnPlaybackStateChangedAsync;
            }
        }

        private void OnDanmakuViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(DanmakuViewModel.DanmakuZoom))
            {
                CheckDanmakuZoom();
            }
            else if (e.PropertyName == nameof(DanmakuViewModel.DanmakuArea))
            {
                _danmakuView.DanmakuArea = DanmakuViewModel.DanmakuArea;
            }
            else if (e.PropertyName == nameof(DanmakuViewModel.DanmakuSpeed))
            {
                _danmakuView.DanmakuDuration = Convert.ToInt32((2.1 - DanmakuViewModel.DanmakuSpeed) * 10);
            }
        }

        private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
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
            else if (e.PropertyName == nameof(ViewModel.CurrentLiveQuality))
            {
                if (ViewModel.CurrentLiveQuality != null &&
                    _liveQualityListView != null &&
                    (_liveQualityListView.SelectedItem == null ||
                    (_liveQualityListView.SelectedItem as LiveQualityViewModel).Data.Quality != ViewModel.CurrentLiveQuality.Quality))
                {
                    _liveQualityListView.SelectedItem = ViewModel.LiveQualityCollection.Where(p => p.Data.Quality == ViewModel.CurrentLiveQuality.Quality).FirstOrDefault();
                }
            }
            else if (e.PropertyName == nameof(ViewModel.CurrentPlayLine))
            {
                if (ViewModel.CurrentPlayLine != null &&
                    _liveQualityListView != null &&
                    (_livePlayLineListView.SelectedItem == null ||
                    (_livePlayLineListView.SelectedItem as LivePlayLineViewModel).Data.Order != ViewModel.CurrentPlayLine.Order))
                {
                    _livePlayLineListView.SelectedItem = ViewModel.LivePlayLineCollection.Where(p => p.Data.Order == ViewModel.CurrentPlayLine.Order).FirstOrDefault();
                }
            }
            else if (e.PropertyName == nameof(ViewModel.PlayerDisplayMode))
            {
                CheckCurrentPlayerMode();
            }
        }

        private async void OnPlaybackStateChangedAsync(MediaPlaybackSession sender, object args)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                if (sender.PlaybackState == MediaPlaybackState.Buffering)
                {
                    _danmakuView.PauseDanmaku();
                }
                else if (sender.PlaybackState == MediaPlaybackState.Paused && sender.Position < sender.NaturalDuration)
                {
                    _danmakuView.PauseDanmaku();
                }
                else if (sender.PlaybackState == MediaPlaybackState.Playing)
                {
                    _danmakuView.ResumeDanmaku();
                    Hide();
                }

                _playPauseButton.Focus(FocusState.Programmatic);
            });
        }

        private void OnDanmakuBarVisibilityButtonClick(object sender, RoutedEventArgs e)
            => ViewModel.IsShowDanmakuBar = !ViewModel.IsShowDanmakuBar;

        private void OnAppViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(AppViewModel.Instance.IsOpenPlayer))
            {
                if (AppViewModel.Instance.IsOpenPlayer)
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
        }

        private void InitializeDanmakuTimer()
        {
            if (_danmakuTimer == null)
            {
                _danmakuTimer = new DispatcherTimer();
                _danmakuTimer.Interval = TimeSpan.FromSeconds(1);
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

        private void InitializeDanmaku(List<DanmakuElem> elements)
        {
            var list = new List<DanmakuModel>();
            foreach (var item in elements)
            {
                var location = DanmakuLocation.Scroll;
                if (item.Mode == 4)
                {
                    location = DanmakuLocation.Bottom;
                }
                else if (item.Mode == 5)
                {
                    location = DanmakuLocation.Top;
                }

                var newDm = new DanmakuModel()
                {
                    Color = item.Color.ToString().ToColor(),
                    Location = location,
                    Pool = item.Pool.ToString(),
                    Id = item.IdStr,
                    SendId = item.MidHash,
                    Size = item.Fontsize,
                    Weight = item.Weight,
                    Text = item.Content,
                    SendTime = item.Ctime.ToString(),
                    Time = item.Progress / 1000,
                };

                list.Add(newDm);
            }

            var group = list.GroupBy(p => p.Time).ToDictionary(x => x.Key, x => x.ToList());
            foreach (var g in group)
            {
                if (_danmakuDictionary.ContainsKey(g.Key))
                {
                    _danmakuDictionary[g.Key] = _danmakuDictionary[g.Key].Concat(g.Value).ToList();
                }
                else
                {
                    _danmakuDictionary.Add(g.Key, g.Value);
                }
            }
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            CheckDanmakuZoom();
            CheckSubtitleZoom();

            DanmakuViewModel.CanShowDanmaku = e.NewSize.Width >= 480;
            IsCompact = e.NewSize.Width < 480;
        }

        private void CheckDanmakuZoom()
        {
            if (ActualWidth == 0 || ActualHeight == 0 || _danmakuView == null)
            {
                return;
            }

            var baseWidth = 800d;
            var baseHeight = 600d;
            var scale = Math.Min(ActualWidth / baseWidth, ActualHeight / baseHeight);
            if (scale > 1)
            {
                scale = 1;
            }
            else if (scale < 0.4)
            {
                scale = 0.4;
            }

            scale *= DanmakuViewModel.DanmakuZoom;
            _danmakuView.DanmakuSizeZoom = scale;
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
            if (segmentIndex > _segmentIndex)
            {
                _segmentIndex = segmentIndex;
                DanmakuViewModel.RequestNewSegmentDanmakuAsync(segmentIndex);
            }

            if (player.PlaybackSession.PlaybackState != MediaPlaybackState.Playing)
            {
                return;
            }

            try
            {
                var positionInt = Convert.ToInt32(position);
                if (_danmakuDictionary.ContainsKey(positionInt))
                {
                    var data = _danmakuDictionary[positionInt];

                    if (DanmakuViewModel.IsDanmakuMerge)
                    {
                        data = data.Distinct(new DanmakuModelComparer()).ToList();
                    }

                    if (DanmakuViewModel.UseCloudShieldSettings && DanmakuViewModel.DanmakuConfig != null)
                    {
                        var isUseDefault = DanmakuViewModel.DanmakuConfig.PlayerConfig.DanmukuPlayerConfig.PlayerDanmakuUseDefaultConfig;
                        var defaultConfig = DanmakuViewModel.DanmakuConfig.PlayerConfig.DanmukuDefaultPlayerConfig;
                        var customCofig = DanmakuViewModel.DanmakuConfig.PlayerConfig.DanmukuPlayerConfig;

                        var isSheldLevel = isUseDefault ?
                                defaultConfig.PlayerDanmakuAiRecommendedSwitch : customCofig.PlayerDanmakuAiRecommendedSwitch;

                        if (isSheldLevel)
                        {
                            var shieldLevel = isUseDefault ?
                                defaultConfig.PlayerDanmakuAiRecommendedLevel : customCofig.PlayerDanmakuAiRecommendedLevel;
                            data = data.Where(p => p.Weight >= shieldLevel).ToList();
                        }

                        var list = DanmakuViewModel.DanmakuConfig.ReportFilterContent.ToList();
                    }

                    await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                    {
                        foreach (var item in data)
                        {
                            _danmakuView.AddScreenDanmaku(item, false);
                        }
                    });
                }
            }
            catch (Exception)
            {
            }
        }

        private void OnCursorTimerTickAsync(object sender, object e)
        {
            _cursorStayTime += 500;
            if (_cursorStayTime > 2000)
            {
                if (IsCursorInMediaElement() && (!IsCursorInControlPanel() || _isTouch))
                {
                    Window.Current.CoreWindow.PointerCursor = null;
                    if (IsControlPanelShown())
                    {
                        Hide();
                    }
                }
                else if (!ViewModel.IsPointerInMediaElement && IsControlPanelShown())
                {
                    Hide();
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
                _playPauseButton.Focus(FocusState.Programmatic);
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
    }
}
