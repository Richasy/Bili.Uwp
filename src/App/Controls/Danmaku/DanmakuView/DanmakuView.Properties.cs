// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using Atelier39;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Bili.App.Controls
{
    /// <summary>
    /// 弹幕视图.
    /// </summary>
    public sealed partial class DanmakuView
    {
        /// <summary>
        /// <see cref="DanmakuSize"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty DanmakuSizeProperty =
            DependencyProperty.Register(nameof(DanmakuSize), typeof(double), typeof(DanmakuView), new PropertyMetadata(1.5, OnDanmakuSizeChanged));

        /// <summary>
        /// <see cref="DanmakuDuration"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty DanmakuDurationProperty =
            DependencyProperty.Register(nameof(DanmakuDuration), typeof(double), typeof(DanmakuView), new PropertyMetadata(1, OnDanmakuDurationChanged));

        /// <summary>
        /// <see cref="DanmakuBold"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty DanmakuBoldProperty =
            DependencyProperty.Register(nameof(DanmakuBold), typeof(bool), typeof(DanmakuView), new PropertyMetadata(false, OnDanmakuBoldChanged));

        /// <summary>
        /// <see cref="DanmakuFontFamily"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty DanmakuFontFamilyProperty =
            DependencyProperty.Register(nameof(DanmakuFontFamily), typeof(string), typeof(DanmakuView), new PropertyMetadata(default, OnDanmakuFontFamilyChanged));

        /// <summary>
        /// <see cref="DanmakuArea"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty DanmakuAreaProperty =
            DependencyProperty.Register(nameof(DanmakuArea), typeof(double), typeof(DanmakuView), new PropertyMetadata(default, OnDanmakuAreaChanged));

        /// <summary>
        /// <see cref="IsDanmakuLimit"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty IsDanmakuLimitProperty =
            DependencyProperty.Register(nameof(IsDanmakuLimit), typeof(bool), typeof(DanmakuView), new PropertyMetadata(default, OnIsDanmakuLimitChanged));

        private const string RootGridName = "RootGrid";

        private Grid _rootGrid;
        private CanvasAnimatedControl _canvas;
        private DanmakuFrostMaster _danmakuController;
        private List<DanmakuItem> _cachedDanmakus;
        private uint _currentTs;

        private bool _isApplyTemplate;

        /// <summary>
        /// 字体大小缩放，电脑推荐默认1.0，手机推荐0.5.
        /// </summary>
        public double DanmakuSize
        {
            get { return (double)GetValue(DanmakuSizeProperty); }
            set { SetValue(DanmakuSizeProperty, value); }
        }

        /// <summary>
        /// 滚动弹幕动画持续时间,单位:秒,越小弹幕移动速度越快.
        /// </summary>
        public double DanmakuDuration
        {
            get { return (double)GetValue(DanmakuDurationProperty); }
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
        /// 弹幕显示区域，取值0.1-1.0.
        /// </summary>
        public double DanmakuArea
        {
            get { return (double)GetValue(DanmakuAreaProperty); }
            set { SetValue(DanmakuAreaProperty, value); }
        }

        /// <summary>
        /// 自动限制同屏弹幕数.
        /// </summary>
        public bool IsDanmakuLimit
        {
            get { return (bool)GetValue(IsDanmakuLimitProperty); }
            set { SetValue(IsDanmakuLimitProperty, value); }
        }
    }
}
