// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using NSDanmaku.Controls;
using NSDanmaku.Model;
using Richasy.Bili.ViewModels.Uwp;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

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

        private const string DefaultPlayModeButtonName = "DefaultModeButton";
        private const string FullWindowPlayModeButtonName = "FullWindowModeButton";
        private const string FullScreenPlayModeButtonName = "FullScreenModeButton";
        private const string CompactOverlayPlayModeButtonName = "CompactOverlayModeButton";

        private readonly Dictionary<int, List<DanmakuModel>> _danmakuDictionary;

        private DispatcherTimer _danmakuTimer;
        private Danmaku _danmakuControl;
        private AppBarToggleButton _defaultPlayModeButton;
        private AppBarToggleButton _fullWindowPlayModeButton;
        private AppBarToggleButton _fullScreenPlayModeButton;
        private AppBarToggleButton _compactOverlayPlayModeButton;
        private int _segmentIndex;

        /// <summary>
        /// 视图模型.
        /// </summary>
        public PlayerViewModel ViewModel
        {
            get { return (PlayerViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }
    }
}
