// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using Richasy.Bili.ViewModels.Uwp;
using Richasy.Bili.ViewModels.Uwp.Common;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Shapes;

namespace Richasy.Bili.App.Controls
{
    /// <summary>
    /// 哔哩播放器的媒体传输控件.
    /// </summary>
    public partial class BiliPlayerTransportControls
    {
        /// <summary>
        /// <see cref="ViewModel"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel), typeof(PlayerViewModel), typeof(BiliPlayerTransportControls), new PropertyMetadata(PlayerViewModel.Instance));

        /// <summary>
        /// <see cref="DanmakuViewModel"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty DanmakuViewModelProperty =
            DependencyProperty.Register(nameof(DanmakuViewModel), typeof(DanmakuViewModel), typeof(BiliPlayerTransportControls), new PropertyMetadata(DanmakuViewModel.Instance));

        /// <summary>
        /// <see cref="SettingViewModel"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty SettingViewModelProperty =
            DependencyProperty.Register(nameof(SettingViewModel), typeof(SettingViewModel), typeof(BiliPlayerTransportControls), new PropertyMetadata(SettingViewModel.Instance));

        private const string DanmakuViewName = "DanmakuView";
        private const string FullWindowPlayModeButtonName = "FullWindowModeButton";
        private const string FullScreenPlayModeButtonName = "FullScreenModeButton";
        private const string CompactOverlayPlayModeButtonName = "CompactOverlayModeButton";
        private const string InteractionControlName = "InteractionControl";
        private const string ControlPanelName = "ControlPanel_ControlPanelVisibilityStates_Border";
        private const string FormatListViewName = "FormatListView";
        private const string LiveQualityListViewName = "LiveQualityListView";
        private const string LivePlayLineListViewName = "LivePlayLineListView";
        private const string BackButtonName = "BackButton";
        private const string BackSkipButtonName = "BackSkipButton";
        private const string ForwardSkipButtonName = "ForwardSkipButton";
        private const string PlayPauseButtonName = "PlayPauseButton";

        private readonly Dictionary<int, List<DanmakuModel>> _danmakuDictionary;

        private DispatcherTimer _danmakuTimer;
        private DispatcherTimer _cursorTimer;
        private DanmakuView _danmakuView;
        private ToggleButton _fullWindowPlayModeButton;
        private ToggleButton _fullScreenPlayModeButton;
        private ToggleButton _compactOverlayPlayModeButton;
        private Rectangle _interactionControl;
        private Border _controlPanel;
        private ListView _formatListView;
        private ListView _liveQualityListView;
        private ListView _livePlayLineListView;
        private Button _backButton;
        private Button _backSkipButton;
        private Button _forwardSkipButton;
        private Button _playPauseButton;
        private int _segmentIndex;
        private double _cursorStayTime;

        /// <summary>
        /// 视图模型.
        /// </summary>
        public PlayerViewModel ViewModel
        {
            get { return (PlayerViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        /// <summary>
        /// 弹幕视图模型.
        /// </summary>
        public DanmakuViewModel DanmakuViewModel
        {
            get { return (DanmakuViewModel)GetValue(DanmakuViewModelProperty); }
            set { SetValue(DanmakuViewModelProperty, value); }
        }

        /// <summary>
        /// 设置视图模型.
        /// </summary>
        public SettingViewModel SettingViewModel
        {
            get { return (SettingViewModel)GetValue(SettingViewModelProperty); }
            set { SetValue(SettingViewModelProperty, value); }
        }
    }
}
