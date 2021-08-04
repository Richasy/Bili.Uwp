// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Bilibili.Community.Service.Dm.V1;
using NSDanmaku.Controls;
using NSDanmaku.Model;
using Richasy.Bili.App.Resources.Extension;
using Richasy.Bili.Models.Enums;
using Windows.Media.Playback;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
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
            this.ViewModel.MediaPlayerUpdated += OnMediaPlayerUdpated;
            this.SettingViewModel.PropertyChanged += OnSettingViewModelPropertyChanged;
            this.SizeChanged += OnSizeChanged;
            InitializeDanmakuTimer();
        }

        /// <inheritdoc/>
        protected override void OnApplyTemplate()
        {
            _danmakuControl = GetTemplateChild(DanmakuControlName) as Danmaku;
            _fullWindowPlayModeButton = GetTemplateChild(FullWindowPlayModeButtonName) as AppBarToggleButton;
            _fullScreenPlayModeButton = GetTemplateChild(FullScreenPlayModeButtonName) as AppBarToggleButton;
            _compactOverlayPlayModeButton = GetTemplateChild(CompactOverlayPlayModeButtonName) as AppBarToggleButton;
            _interactionControl = GetTemplateChild(InteractionControlName) as Rectangle;
            _controlPanel = GetTemplateChild(ControlPanelName) as Border;

            _fullWindowPlayModeButton.Click += OnPlayModeButtonClick;
            _fullScreenPlayModeButton.Click += OnPlayModeButtonClick;
            _compactOverlayPlayModeButton.Click += OnPlayModeButtonClick;
            _interactionControl.Tapped += OnInteractionControlTapped;

            CheckCurrentPlayerMode();
            CheckDanmakuZoom();
            CheckMTCControlMode();
            base.OnApplyTemplate();
        }

        private void OnInteractionControlTapped(object sender, TappedRoutedEventArgs e)
        {
            if (this.ShowAndHideAutomatically)
            {
                return;
            }

            if (_controlPanel.Opacity == 0d)
            {
                Show();
            }
            else if (_controlPanel.Opacity == 1)
            {
                Hide();
            }
        }

        private void OnPlayModeButtonClick(object sender, RoutedEventArgs e)
        {
            var btn = sender as AppBarToggleButton;
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
                    break;
                case PlayerDisplayMode.FullWindow:
                    _fullWindowPlayModeButton.IsChecked = true;
                    _fullScreenPlayModeButton.IsChecked = false;
                    _compactOverlayPlayModeButton.IsChecked = false;
                    break;
                case PlayerDisplayMode.FullScreen:
                    _fullWindowPlayModeButton.IsChecked = false;
                    _fullScreenPlayModeButton.IsChecked = true;
                    _compactOverlayPlayModeButton.IsChecked = false;
                    break;
                case PlayerDisplayMode.CompactOverlay:
                    _fullWindowPlayModeButton.IsChecked = false;
                    _fullScreenPlayModeButton.IsChecked = false;
                    _compactOverlayPlayModeButton.IsChecked = true;
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
            _danmakuControl.ClearAll();
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

        private async void OnPlaybackStateChangedAsync(MediaPlaybackSession sender, object args)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                if (sender.PlaybackState == MediaPlaybackState.Buffering)
                {
                    _danmakuControl.PauseDanmaku();
                }
                else if (sender.PlaybackState == MediaPlaybackState.Paused && sender.Position < sender.NaturalDuration)
                {
                    _danmakuControl.PauseDanmaku();
                }
                else if (sender.PlaybackState == MediaPlaybackState.Playing)
                {
                    _danmakuControl.ResumeDanmaku();
                }
            });
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
                    color = item.Color.ToString().ToColor(),
                    fromSite = DanmakuSite.Bilibili,
                    location = location,
                    pool = item.Pool.ToString(),
                    rowID = item.IdStr,
                    sendID = item.MidHash,
                    size = item.Fontsize,
                    weight = item.Weight,
                    text = item.Content,
                    sendTime = item.Ctime.ToString(),
                    time = item.Progress / 1000d,
                    time_s = item.Progress / 1000,
                };

                list.Add(newDm);
            }

            var group = list.GroupBy(p => p.time_s).ToDictionary(x => x.Key, x => x.ToList());
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
            if (this.ActualWidth == 0 || this.ActualHeight == 0 || _danmakuControl == null)
            {
                return;
            }

            var baseWidth = 800d;
            var baseHeight = 600d;
            var scale = Math.Min(this.ActualWidth / baseWidth, ActualHeight / baseHeight);
            scale *= DanmakuViewModel.DanmakuZoom;
            if (scale > 1)
            {
                scale = 1;
            }
            else if (scale < 0.4)
            {
                scale = 0.4;
            }

            _danmakuControl.DanmakuSizeZoom = scale;
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
            if (ViewModel.BiliPlayer == null || _danmakuControl == null)
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
                            data = data.Where(p => p.weight >= shieldLevel).ToList();
                        }

                        var list = DanmakuViewModel.DanmakuConfig.ReportFilterContent.ToList();
                    }

                    await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                    {
                        foreach (var item in data)
                        {
                            _danmakuControl.AddDanmu(item, false);
                        }
                    });
                }
            }
            catch (Exception)
            {
            }
        }
    }
}
