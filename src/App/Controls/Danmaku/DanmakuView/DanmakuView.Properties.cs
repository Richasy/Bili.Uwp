// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using Atelier39;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Bili.App.Controls.Danmaku
{
    /// <summary>
    /// 弹幕视图.
    /// </summary>
    public sealed partial class DanmakuView
    {
        private const string RootGridName = "RootGrid";
        private const string AnimatedCanvasName = "AnimatedCanvas";

        private Grid _rootGrid;
        private CanvasAnimatedControl _canvas;
        private DanmakuFrostMaster _danmakuController;
        private List<DanmakuItem> _cachedDanmakus;
        private uint _currentTs;
    }
}
