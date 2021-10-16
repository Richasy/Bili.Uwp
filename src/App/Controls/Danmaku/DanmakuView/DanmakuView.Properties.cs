// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using Richasy.Bili.Models.Enums.App;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace Richasy.Bili.App.Controls
{
    /// <summary>
    /// 弹幕视图.
    /// </summary>
    public sealed partial class DanmakuView
    {
        /// <summary>
        /// <see cref="DanmakuSizeZoom"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty DanmakuSizeZoomProperty =
            DependencyProperty.Register(nameof(DanmakuSizeZoom), typeof(double), typeof(DanmakuView), new PropertyMetadata(1.0, OnDanmakuSizeZoomChanged));

        /// <summary>
        /// <see cref="DanmakuDuration"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty DanmakuDurationProperty =
            DependencyProperty.Register(nameof(DanmakuDuration), typeof(int), typeof(DanmakuView), new PropertyMetadata(10, OnDanmakuDurationChanged));

        /// <summary>
        /// <see cref="DanmakuBold"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty DanmakuBoldProperty =
            DependencyProperty.Register(nameof(DanmakuBold), typeof(bool), typeof(DanmakuView), new PropertyMetadata(false));

        /// <summary>
        /// <see cref="DanmakuFontFamily"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty DanmakuFontFamilyProperty =
            DependencyProperty.Register(nameof(DanmakuFontFamily), typeof(string), typeof(DanmakuView), new PropertyMetadata(default));

        /// <summary>
        /// <see cref="DanmakuStyle"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty DanmakuStyleProperty =
          DependencyProperty.Register(nameof(DanmakuStyle), typeof(DanmakuStyle), typeof(DanmakuView), new PropertyMetadata(DanmakuStyle.Stroke));

        /// <summary>
        /// <see cref="DanmakuArea"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty DanmakuAreaProperty =
            DependencyProperty.Register(nameof(DanmakuArea), typeof(double), typeof(DanmakuView), new PropertyMetadata(0d, OnDanmakuAreaChanged));

        private const string RootGridName = "RootGrid";
        private const string CanvasName = "Canvas";
        private const string ScrollContainerName = "ScrollContainer";
        private const string TopContainerName = "TopContainer";
        private const string BottomContainerName = "BottomContainer";

        private readonly List<Storyboard> _topBottomStoryList = new List<Storyboard>();
        private readonly List<Storyboard> _scrollStoryList = new List<Storyboard>();
        private readonly List<Storyboard> _positionStoryList = new List<Storyboard>();

        private Grid _rootGrid;
        private Canvas _canvas;
        private Grid _topContainer;
        private Grid _bottomContainer;
        private Grid _scrollContainer;

        private bool _isApplyTemplate;

        /// <summary>
        /// 字体大小缩放，电脑推荐默认1.0，手机推荐0.5.
        /// </summary>
        public double DanmakuSizeZoom
        {
            get { return (double)GetValue(DanmakuSizeZoomProperty); }
            set { SetValue(DanmakuSizeZoomProperty, value); }
        }

        /// <summary>
        /// 滚动弹幕动画持续时间,单位:秒,越小弹幕移动速度越快.
        /// </summary>
        public int DanmakuDuration
        {
            get { return (int)GetValue(DanmakuDurationProperty); }
            set { SetValue(DanmakuDurationProperty, value); }
        }

        /// <summary>
        /// 弹幕是否加粗.
        /// </summary>
        public bool DanmakuBold
        {
            get { return (bool)GetValue(DanmakuBoldProperty); }
            set { SetValue(DanmakuBoldProperty, value); }
        }

        /// <summary>
        /// 弹幕字体名称.
        /// </summary>
        public string DanmakuFontFamily
        {
            get { return (string)GetValue(DanmakuFontFamilyProperty); }
            set { SetValue(DanmakuFontFamilyProperty, value); }
        }

        /// <summary>
        /// 弹幕样式.
        /// </summary>
        public DanmakuStyle DanmakuStyle
        {
            get { return (DanmakuStyle)GetValue(DanmakuStyleProperty); }
            set { SetValue(DanmakuStyleProperty, value); }
        }

        /// <summary>
        /// 弹幕显示区域，取值0.1-1.0.
        /// </summary>
        public double DanmakuArea
        {
            get { return (double)GetValue(DanmakuAreaProperty); }
            set { SetValue(DanmakuAreaProperty, value); }
        }
    }
}
