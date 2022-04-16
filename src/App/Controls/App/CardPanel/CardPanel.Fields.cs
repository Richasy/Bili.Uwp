﻿// Copyright (c) Richasy. All rights reserved.

using System;
using Windows.UI.Xaml.Controls;

namespace Richasy.Bili.App.Controls
{
    /// <summary>
    /// 卡片面板的字段.
    /// </summary>
    public partial class CardPanel
    {
        private const float PointerOverOffsetY = -4f;

        private static readonly TimeSpan OffsetDuration = TimeSpan.FromMilliseconds(250);

        private Grid _rootContainer;
        private long _pointerOverToken;
        private long _pressedToken;
    }
}
