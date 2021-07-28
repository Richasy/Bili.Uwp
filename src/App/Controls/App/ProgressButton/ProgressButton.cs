// Copyright (c) Richasy. All rights reserved.

using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Shapes;

namespace Richasy.Bili.App.Controls
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

        private readonly double _defaultHoldingJudgeTime = 100d;

        private BubbleView _bubbleView;
        private Ellipse _host;
        private Microsoft.UI.Xaml.Controls.ProgressRing _progressRing;

        private long _pressedToken;

        private DispatcherTimer _timer;
        private bool _isHolding = false;
        private double _holdingTime = 0d;
        private double _currentProgressValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProgressButton"/> class.
        /// </summary>
        public ProgressButton()
        {
            this.DefaultStyleKey = typeof(ProgressButton);
            this.Loading += OnLoading;
            this.Unloaded += OnUnloaded;
        }

        /// <summary>
        /// Gets or sets button diameter.
        /// </summary>
        public double Diameter
        {
            get { return (double)this.GetValue(DiameterProperty); }
            set { this.SetValue(DiameterProperty, value); }
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
        /// 开始进度动画.
        /// </summary>
        public void BeginProgressAnimation()
        {
            VisualStateManager.GoToState(this, "HoldingState", false);
        }

        /// <summary>
        /// 终止进度动画.
        /// </summary>
        public void StopProgressAnimation()
        {
            VisualStateManager.GoToState(this, "NonState", false);
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
            if (_timer != null)
            {
                _timer.Stop();
                _timer = null;
            }

            _isHolding = false;
            this.UnregisterPropertyChangedCallback(IsPressedProperty, _pressedToken);
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
            _pressedToken = this.RegisterPropertyChangedCallback(IsPressedProperty, new DependencyPropertyChangedCallback(OnIsPressedChanged));
        }

        private void OnTimerTick(object sender, object e)
        {
            _holdingTime += 10;
            if (_holdingTime > _defaultHoldingJudgeTime && !_isHolding)
            {
                _isHolding = true;
                BeginProgressAnimation();
            }
        }

        private void OnIsPressedChanged(DependencyObject sender, DependencyProperty dp)
        {
            if (IsPressed)
            {
                if (Convert.ToBoolean(IsChecked))
                {
                    return;
                }

                _isHolding = false;
                _holdingTime = 0d;
                _timer.Start();
            }
            else
            {
                // Stop holding.
                _timer.Stop();
                this.IsChecked = _isHolding && _currentProgressValue > 99.9d;
                StopProgressAnimation();
            }
        }

        private void OnValueChanged(DependencyObject sender, DependencyProperty dp)
        {
            _currentProgressValue = _progressRing.Value;
            if (_currentProgressValue >= 99.9)
            {
                // Raise event.
                _bubbleView.ShowBubbles();
            }
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
