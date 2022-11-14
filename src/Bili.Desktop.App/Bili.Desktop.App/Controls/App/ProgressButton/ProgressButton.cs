// Copyright (c) Richasy. All rights reserved.

using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Shapes;

namespace Bili.Desktop.App.Controls
{
    /// <summary>
    /// 带进度环和完成动画的按钮.
    /// </summary>
    public sealed class ProgressButton : ToggleButton
    {
        /// <summary>
        /// <see cref="Diameter"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty DiameterProperty =
            DependencyProperty.Register(
                nameof(Diameter),
                typeof(double),
                typeof(ProgressButton),
                new PropertyMetadata(default, new PropertyChangedCallback(OnDiameterChanged)));

        /// <summary>
        /// <see cref="Progress"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty ProgressProperty =
            DependencyProperty.Register(
                nameof(Progress),
                typeof(double),
                typeof(ProgressButton),
                new PropertyMetadata(0d));

        /// <summary>
        /// <see cref="Description"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty DescriptionProperty =
            DependencyProperty.Register(
                nameof(Description),
                typeof(string),
                typeof(ProgressButton),
                new PropertyMetadata(string.Empty));

        /// <summary>
        /// <see cref="Spacing"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty SpacingProperty =
            DependencyProperty.Register(
                nameof(Spacing),
                typeof(double),
                typeof(ProgressButton),
                new PropertyMetadata(0d));

        private readonly double _defaultHoldingJudgeTime = 100d;

        private BubbleView _bubbleView;
        private Ellipse _host;
        private Microsoft.UI.Xaml.Controls.ProgressRing _progressRing;

        private long _pressedToken;
        private long _isCheckedToken;

        private DispatcherTimer _timer;
        private bool _isHolding = false;
        private double _holdingTime = 0d;
        private double _currentProgressValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProgressButton"/> class.
        /// </summary>
        public ProgressButton()
        {
            DefaultStyleKey = typeof(ProgressButton);
            Loading += OnLoading;
            Unloaded += OnUnloaded;
        }

        /// <summary>
        /// 长按开始.
        /// </summary>
        public event EventHandler HoldingStart;

        /// <summary>
        /// 长按中止（此时没有完成动画）.
        /// </summary>
        public event EventHandler HoldingSuspend;

        /// <summary>
        /// 长按完成.
        /// </summary>
        public event EventHandler HoldingCompleted;

        /// <summary>
        /// 直径.
        /// </summary>
        public double Diameter
        {
            get { return (double)GetValue(DiameterProperty); }
            set { SetValue(DiameterProperty, value); }
        }

        /// <summary>
        /// 进度（小于等于100）.
        /// </summary>
        public double Progress
        {
            get { return (double)GetValue(ProgressProperty); }
            set { SetValue(ProgressProperty, value); }
        }

        /// <summary>
        /// 描述文本.
        /// </summary>
        public string Description
        {
            get { return (string)GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }

        /// <summary>
        /// 按钮与说明文本的间隔.
        /// </summary>
        public double Spacing
        {
            get { return (double)GetValue(SpacingProperty); }
            set { SetValue(SpacingProperty, value); }
        }

        /// <summary>
        /// 显示气泡.
        /// </summary>
        public void ShowBubbles()
        {
            _bubbleView.ShowBubbles();
        }

        /// <inheritdoc/>
        protected override void OnApplyTemplate()
        {
            _host = GetTemplateChild("BackgroundHost") as Ellipse;
            _bubbleView = GetTemplateChild("BubbleView") as BubbleView;
            _progressRing = GetTemplateChild("ProgressRing") as Microsoft.UI.Xaml.Controls.ProgressRing;
            _progressRing.RegisterPropertyChangedCallback(Microsoft.UI.Xaml.Controls.ProgressRing.ValueProperty, new DependencyPropertyChangedCallback(OnValueChanged));
            Initialize();
        }

        private static void OnDiameterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as ProgressButton;
            if (e.NewValue is double v && v > 0)
            {
                instance.Initialize();
                instance.CornerRadius = new CornerRadius(Math.Ceiling((v + 12) / 2.0));
            }
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            _timer?.Stop();

            _isHolding = false;
            UnregisterPropertyChangedCallback(IsPressedProperty, _pressedToken);
            UnregisterPropertyChangedCallback(IsCheckedProperty, _isCheckedToken);
        }

        private void OnLoading(FrameworkElement sender, object args)
        {
            if (_timer == null)
            {
                _timer = new DispatcherTimer();
                _timer.Interval = TimeSpan.FromMilliseconds(10);
                _timer.Tick += OnTimerTick;
            }

            _isHolding = false;
            _pressedToken = RegisterPropertyChangedCallback(IsPressedProperty, new DependencyPropertyChangedCallback(OnIsPressedChanged));
            _isCheckedToken = RegisterPropertyChangedCallback(IsCheckedProperty, new DependencyPropertyChangedCallback(OnIsCheckedChanged));
        }

        private void OnTimerTick(object sender, object e)
        {
            _holdingTime += 10;
            if (_holdingTime > _defaultHoldingJudgeTime && !_isHolding)
            {
                _isHolding = true;
                HoldingStart?.Invoke(this, EventArgs.Empty);
                BeginProgressAnimation();
            }
        }

        private void OnIsPressedChanged(DependencyObject sender, DependencyProperty dp)
        {
            if (!IsHoldingEnabled)
            {
                return;
            }

            if (IsPressed)
            {
                _isHolding = false;
                _holdingTime = 0d;
                _timer.Start();
            }
            else
            {
                if (_currentProgressValue < 99.9 && _currentProgressValue > 0)
                {
                    HoldingSuspend?.Invoke(this, EventArgs.Empty);
                }

                StopProgressAnimation();
            }
        }

        private void OnIsCheckedChanged(DependencyObject sender, DependencyProperty dp)
        {
            if (_isHolding)
            {
                IsChecked = _currentProgressValue >= 99.9;
                _isHolding = false;
            }
        }

        private void OnValueChanged(DependencyObject sender, DependencyProperty dp)
        {
            _currentProgressValue = _progressRing.Value;
            if (_currentProgressValue >= 99.9)
            {
                _bubbleView.ShowBubbles();
                IsChecked = true;
                HoldingCompleted?.Invoke(this, EventArgs.Empty);
            }
            else if (!IsPressed)
            {
                StopProgressAnimation();
            }
        }

        /// <summary>
        /// 开始进度动画.
        /// </summary>
        private void BeginProgressAnimation()
        {
            VisualStateManager.GoToState(this, "HoldingState", false);
        }

        /// <summary>
        /// 终止进度动画.
        /// </summary>
        private void StopProgressAnimation()
        {
            if (_currentProgressValue < 99.9)
            {
                _progressRing.Value = 0d;
            }

            _timer?.Stop();
            VisualStateManager.GoToState(this, "NonState", false);
        }

        private void Initialize()
        {
            if (_host != null)
            {
                _host.Width = _host.Height = Diameter;
            }

            if (_progressRing != null)
            {
                _progressRing.Width = _progressRing.Height = Diameter + 8;
            }
        }
    }
}
