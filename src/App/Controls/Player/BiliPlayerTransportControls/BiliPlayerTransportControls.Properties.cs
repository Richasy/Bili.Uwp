// Copyright (c) Richasy. All rights reserved.

using Bili.App.Controls.Danmaku;
using Bili.Models.Enums.App;
using Bili.ViewModels.Uwp;
using Bili.ViewModels.Uwp.Common;
using Bili.ViewModels.Uwp.Core;
using Bili.ViewModels.Uwp.Home;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Shapes;

namespace Bili.App.Controls
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
            DependencyProperty.Register(nameof(DanmakuViewModel), typeof(DanmakuModuleViewModel), typeof(BiliPlayerTransportControls), new PropertyMetadata(default));

        /// <summary>
        /// <see cref="SettingViewModel"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty SettingViewModelProperty =
            DependencyProperty.Register(nameof(SettingViewModel), typeof(SettingsPageViewModel), typeof(BiliPlayerTransportControls), new PropertyMetadata(default));

        private const string RootGridName = "RootGrid";
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
        private const string DanmakuBarVisibilityButtonName = "ToggleDanmakuBarVisibilityButton";
        private const string SubtitleBlockName = "SubtitleBlock";
        private const string HomeButtonName = "HomeButton";
        private const string BackToDefaultButtonName = "BackToDefaultButton";
        private const string PreviousViewInformerName = "PreviousViewInformer";
        private const string LiveRefreshButtonName = "LiveRefreshButton";
        private const string TempMessageContaienrName = "TempMessageContainer";
        private const string TempMessageBlockName = "TempMessageBlock";
        private const string PreviousEpisodeButtonName = "PreviousEpisodeButton";
        private const string NextEpisodeButtonName = "NextEpisodeButton";
        private const string ScreenshotButtonName = "ScreenshotButton";
        private const string PlaybackRateNodeComboBoxName = "PlaybackRateNodeComboBox";
        private const string IncreasePlayRateButtonName = "IncreasePlayRateButton";
        private const string DecreasePlayRateButtonName = "DecreasePlayRateButton";
        private const string IncreaseVolumeButtonName = "IncreaseVolumeButton";
        private const string DecreaseVolumeButtonName = "DecreaseVolumeButton";
        private const string NextVideoInformerName = "NextVideoInformer";
        private const string VisibilityStateGroupName = "ControlPanelVisibilityStates";
        private const string FormatButtonName = "FormatButton";
        private const string LiveQualityButtonName = "LiveQualityButton";
        private const string MiniViewButtonName = "MiniViewButton";

        private DispatcherTimer _danmakuTimer;
        private DispatcherTimer _cursorTimer;
        private DispatcherTimer _normalTimer;
        private DispatcherTimer _focusTimer;
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
        private Button _danmakuBarVisibilityButton;
        private Button _homeButton;
        private Button _backToDefaultButton;
        private Button _liveRefreshButton;
        private Button _previousEpisodeButton;
        private Button _nextEpisodeButton;
        private Button _screenshotButton;
        private TextBlock _subtitleBlock;
        private Grid _tempMessageContainer;
        private TextBlock _tempMessageBlock;
        private ComboBox _playbackRateNodeComboBox;
        private Button _increasePlayRateButton;
        private Button _decreasePlayRateButton;
        private Button _increaseVolumeButton;
        private Button _decreaseVolumeButton;
        private PlayerTip _previousViewInformer;
        private PlayerTip _nextVidepInformer;
        private Grid _rootGrid;
        private VisualStateGroup _visibilityStateGroup;
        private Button _formatButton;
        private Button _liveQualityButton;
        private GestureRecognizer _gestureRecognizer;
        private Button _miniViewButton;

        private int _segmentIndex;
        private double _cursorStayTime;
        private double _historyMessageHoldSeconds;
        private double _tempMessageHoldSeconds;
        private double _nextVideoHoldSeconds;

        private double _manipulationDeltaX = 0d;
        private double _manipulationDeltaY = 0d;
        private double _manipulationProgress = 0d;
        private double _manipulationVolume = 0d;
        private double _manipulationUnitLength = 0d;
        private bool _manipulationBeforeIsPlay = false;
        private PlayerManipulationType _manipulationType = PlayerManipulationType.None;

        private bool _isTouch = false;
        private bool _isHolding = false;

        /// <summary>
        /// 实例.
        /// </summary>
        public static BiliPlayerTransportControls Instance { get; private set; }

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
        public DanmakuModuleViewModel DanmakuViewModel
        {
            get { return (DanmakuModuleViewModel)GetValue(DanmakuViewModelProperty); }
            set { SetValue(DanmakuViewModelProperty, value); }
        }

        /// <summary>
        /// 设置视图模型.
        /// </summary>
        public SettingsPageViewModel SettingViewModel
        {
            get { return (SettingsPageViewModel)GetValue(SettingViewModelProperty); }
            set { SetValue(SettingViewModelProperty, value); }
        }
    }
}
