// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Bilibili.Community.Service.Dm.V1;
using Richasy.Bili.App.Resources.Extension;
using Richasy.Bili.Models.BiliBili;
using Richasy.Bili.Models.Enums;
using Richasy.Bili.Models.Enums.App;
using Richasy.Bili.ViewModels.Uwp;
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
            this.DefaultStyleKey = typeof(BiliPlayerTransportControls);
            this._danmakuDictionary = new Dictionary<int, List<DanmakuModel>>();
            this._segmentIndex = 1;
            this.DanmakuViewModel.DanmakuListAdded += OnDanmakuListAdded;
            this.DanmakuViewModel.RequestClearDanmaku += OnRequestClearDanmaku;
            this.DanmakuViewModel.PropertyChanged += OnDanmakuViewModelPropertyChanged;
            this.DanmakuViewModel.SendDanmakuSucceeded += OnSendDanmakuSucceeded;
            this.ViewModel.MediaPlayerUpdated += OnMediaPlayerUdpated;
            this.SettingViewModel.PropertyChanged += OnSettingViewModelPropertyChanged;
            this.ViewModel.PropertyChanged += OnViewModelPropertyChanged;
            this.ViewModel.NewLiveDanmakuAdded += OnNewLiveDanmakuAdded;
            this.SizeChanged += OnSizeChanged;
            InitializeDanmakuTimer();
            InitializeCursorTimer();
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

            _fullWindowPlayModeButton.Click += OnPlayModeButtonClick;
            _fullScreenPlayModeButton.Click += OnPlayModeButtonClick;
            _compactOverlayPlayModeButton.Click += OnPlayModeButtonClick;
            _interactionControl.Tapped += OnInteractionControlTapped;
            _interactionControl.DoubleTapped += OnInteractionControlDoubleTapped;
            _backButton.Click += OnBackButtonClick;
            _danmakuBarVisibilityButton.Click += OnDanmakuBarVisibilityButtonClick;

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

            CheckCurrentPlayerMode();
            CheckDanmakuZoom();
            CheckMTCControlMode();
            base.OnApplyTemplate();
        }

        /// <inheritdoc/>
        protected override void OnPointerMoved(PointerRoutedEventArgs e)
        {
            _cursorTimer.Start();
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 0);
            _cursorStayTime = 0;
        }

        /// <inheritdoc/>
        protected override void OnPointerExited(PointerRoutedEventArgs e)
        {
            _cursorTimer.Stop();
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 0);
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
            }
        }

        private void OnInteractionControlTapped(object sender, TappedRoutedEventArgs e)
        {
            if (this.ShowAndHideAutomatically)
            {
                _playPauseButton.Focus(FocusState.Programmatic);
                return;
            }

            if (_controlPanel.Opacity == 0d)
            {
                Show();
            }
            else if (_controlPanel.Opacity == 1)
            {
                _playPauseButton.Focus(FocusState.Programmatic);
                Hide();
            }
        }

        private void OnInteractionControlDoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            var playerStatus = ViewModel.PlayerStatus;
            var canDoubleTapped = playerStatus == PlayerStatus.Playing || playerStatus == PlayerStatus.Pause;
            if (canDoubleTapped)
            {
                var behavior = SettingViewModel.DoubleClickBehavior;
                switch (behavior)
                {
                    case DoubleClickBehavior.FullScreen:
                        _fullScreenPlayModeButton.IsChecked = !_fullScreenPlayModeButton.IsChecked;
                        OnPlayModeButtonClick(_fullScreenPlayModeButton, null);
                        break;
                    case DoubleClickBehavior.PlayPause:
                        ViewModel.TogglePlayPause();
                        break;
                    default:
                        break;
                }
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
        }

        private void OnDanmakuListAdded(object sender, List<DanmakuElem> e)
        {
            InitializeDanmaku(e);
            this._danmakuTimer.Start();
        }

        private void OnRequestClearDanmaku(object sender, EventArgs e)
        {
            this._segmentIndex = 1;
            this._danmakuDictionary.Clear();
            this._danmakuTimer.Stop();
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

        private void OnSettingViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SettingViewModel.DefaultMTCControlMode))
            {
                CheckMTCControlMode();
            }
        }

        private void OnDanmakuViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(DanmakuViewModel.DanmakuZoom))
            {
                CheckDanmakuZoom();
            }
        }

        private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ViewModel.CurrentFormat))
            {
                if (ViewModel.CurrentFormat != null &&
                    (_formatListView.SelectedItem == null ||
                    (_formatListView.SelectedItem as VideoFormatViewModel).Data.Quality != ViewModel.CurrentFormat.Quality))
                {
                    _formatListView.SelectedItem = ViewModel.FormatCollection.Where(p => p.Data.Quality == ViewModel.CurrentFormat.Quality).FirstOrDefault();
                }
            }
            else if (e.PropertyName == nameof(ViewModel.CurrentLiveQuality))
            {
                if (ViewModel.CurrentLiveQuality != null &&
                    (_liveQualityListView.SelectedItem == null ||
                    (_liveQualityListView.SelectedItem as LiveQualityViewModel).Data.Quality != ViewModel.CurrentLiveQuality.Quality))
                {
                    _liveQualityListView.SelectedItem = ViewModel.LiveQualityCollection.Where(p => p.Data.Quality == ViewModel.CurrentLiveQuality.Quality).FirstOrDefault();
                }
            }
            else if (e.PropertyName == nameof(ViewModel.CurrentPlayLine))
            {
                if (ViewModel.CurrentPlayLine != null &&
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
                    this.Hide();
                }
            });
        }

        private void OnDanmakuBarVisibilityButtonClick(object sender, RoutedEventArgs e)
        {
            ViewModel.IsShowDanmakuBar = !ViewModel.IsShowDanmakuBar;
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
        }

        private void CheckDanmakuZoom()
        {
            if (this.ActualWidth == 0 || this.ActualHeight == 0 || _danmakuView == null)
            {
                return;
            }

            var baseWidth = 800d;
            var baseHeight = 600d;
            var scale = Math.Min(this.ActualWidth / baseWidth, ActualHeight / baseHeight);
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

        private void CheckMTCControlMode()
        {
            switch (SettingViewModel.DefaultMTCControlMode)
            {
                case MTCControlMode.Automatic:
                    this.ShowAndHideAutomatically = true;
                    break;
                case MTCControlMode.Manual:
                    this.ShowAndHideAutomatically = false;
                    break;
                default:
                    break;
            }
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
            if (_cursorStayTime > 1500)
            {
                Window.Current.CoreWindow.PointerCursor = null;
                _cursorTimer.Stop();
                _cursorStayTime = 0;
            }
        }
    }
}
