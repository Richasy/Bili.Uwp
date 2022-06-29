// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using Atelier39;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Bili.App.Controls.Danmaku
{
    /// <summary>
    /// 弹幕视图.
    /// </summary>
    public sealed partial class DanmakuView
    {
        /// <summary>
        /// <see cref="ProgressPosition"/> 的依赖属性.
        /// </summary>
        public static readonly DependencyProperty ProgressPositionProperty =
            DependencyProperty.Register(nameof(ProgressPosition), typeof(double), typeof(DanmakuView), new PropertyMetadata(0d, new PropertyChangedCallback(OnProgressPositionChanged)));

        private const string RootGridName = "RootGrid";

        private Grid _rootGrid;
        private CanvasAnimatedControl _canvas;
        private DanmakuFrostMaster _danmakuController;
        private List<DanmakuItem> _cachedDanmakus;
        private uint _currentTs;
        private bool _isInitialized;

        /// <summary>
        /// 播放进度位置.
        /// </summary>
        public double ProgressPosition
        {
            get { return (double)GetValue(ProgressPositionProperty); }
            set { SetValue(ProgressPositionProperty, value); }
        }
    }
}
