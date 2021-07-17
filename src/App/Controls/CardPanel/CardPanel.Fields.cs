// Copyright (c) Richasy. All rights reserved.

using System;
using Windows.UI.Composition;
using Windows.UI.Xaml.Controls;

namespace Richasy.Bili.App.Controls
{
    /// <summary>
    /// 卡片面板的字段.
    /// </summary>
    public partial class CardPanel
    {
        // Light theme
        private const float LightPointerRestShadowRadius = 8f;
        private const float LightPointerRestShadowOffsetY = 2f;
        private const float LightPointerRestShadowOpacity = 0.02f;

        private const float LightPointerOverShadowRadius = 8f;
        private const float LightPointerOverShadowOffsetY = 4f;
        private const float LightPointerOverShadowOpacity = 0.04f;

        private const float LightPointerPressedShadowRadius = 2f;
        private const float LightPointerPressedShadowOffsetY = 0;
        private const float LightPointerPressedShadowOpacity = 0.03f;

        // Dark theme
        private const float DarkPointerRestShadowRadius = 6f;
        private const float DarkPointerRestShadowOffsetY = 2f;
        private const float DarkPointerRestShadowOpacity = 0.1f;

        private const float DarkPointerOverShadowRadius = 12f;
        private const float DarkPointerOverShadowOffsetY = 8f;
        private const float DarkPointerOverShadowOpacity = 0.18f;

        private const float DarkPointerPressedShadowRadius = 2f;
        private const float DarkPointerPressedShadowOffsetY = 0f;
        private const float DarkPointerPressedShadowOpacity = 0.2f;

        private const float PointerOverOffsetY = -4f;

        private static readonly TimeSpan ShowShadowDuration = TimeSpan.FromMilliseconds(300);
        private static readonly TimeSpan HideShadowDuration = TimeSpan.FromMilliseconds(250);
        private static readonly TimeSpan PressShadowDuration = TimeSpan.FromMilliseconds(250);

        private readonly Compositor _compositor;
        private Grid _rootContainer;
        private bool _loaded;
        private bool _templateApplied;
        private bool _shadowCreated;
        private long _pointerOverToken;
        private long _pressedToken;
        private long _checkedToken;
    }
}
