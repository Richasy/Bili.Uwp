// Copyright (c) Richasy. All rights reserved.

using Bili.Uwp.App.Controls.Danmaku;
using Bili.Models.Enums.App;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Shapes;

namespace Bili.Uwp.App.Controls.Player
{
    /// <summary>
    /// 媒体播放器.
    /// </summary>
    public sealed partial class BiliMediaPlayer
    {
        private const string MediaPlayerElementName = "MediaPlayerElement";
        private const string InteractionControlName = "InteractionControl";
        private const string MediaTransportControlsName = "MediaTransportControls";
        private const string TempMessageContaienrName = "TempMessageContainer";
        private const string TempMessageBlockName = "TempMessageBlock";
        private const string SubtitleBlockName = "SubtitleBlock";
        private const string DanmakuViewName = "DanmakuView";

        private readonly DispatcherTimer _unitTimer;

        private MediaPlayerElement _mediaPlayerElement;
        private BiliMediaTransportControls _mediaTransportControls;
        private Rectangle _interactionControl;
        private GestureRecognizer _gestureRecognizer;
        private Grid _tempMessageContainer;
        private TextBlock _tempMessageBlock;
        private TextBlock _subtitleBlock;
        private DanmakuView _danmakuView;

        private double _cursorStayTime;
        private double _tempMessageStayTime;
        private double _transportStayTime;
        private double _nextVideoStayTime;
        private double _progressTipStayTime;

        private double _manipulationDeltaX = 0d;
        private double _manipulationDeltaY = 0d;
        private double _manipulationProgress = 0d;
        private double _manipulationVolume = 0d;
        private double _manipulationUnitLength = 0d;
        private bool _manipulationBeforeIsPlay = false;
        private PlayerManipulationType _manipulationType = PlayerManipulationType.None;

        private bool _isTouch = false;
        private bool _isHolding = false;
        private bool _isCursorInPlayer = false;
        private bool _isForceHiddenTransportControls;
    }
}
