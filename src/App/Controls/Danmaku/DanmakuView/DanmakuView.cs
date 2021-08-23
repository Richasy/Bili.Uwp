// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Richasy.Bili.Models.Enums.App;
using Richasy.Shadow.Uwp;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace Richasy.Bili.App.Controls
{
    /// <summary>
    /// 弹幕控件.
    /// </summary>
    public sealed partial class DanmakuView : Control
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Danmaku"/> class.
        /// </summary>
        public DanmakuView()
        {
            DefaultStyleKey = typeof(DanmakuView);
            DanmakuArea = 1.0;
            DanmakuBold = false;
            DanmakuFontFamily = string.Empty;
        }

        /// <inheritdoc/>
        protected override void OnApplyTemplate()
        {
            _rootGrid = GetTemplateChild(RootGridName) as Grid;
            _scrollContainer = GetTemplateChild(ScrollContainerName) as Grid;
            _topContainer = GetTemplateChild(TopContainerName) as Grid;
            _bottomContainer = GetTemplateChild(BottomContainerName) as Grid;
            _canvas = GetTemplateChild(CanvasName) as Canvas;
            _isApplyTemplate = true;
        }

        /// <inheritdoc/>
        protected override Size MeasureOverride(Size availableSize)
        {
            SetRows(availableSize.Height);
            return base.MeasureOverride(availableSize);
        }

        private static void OnDanmakuSizeZoomChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var value = Convert.ToDouble(e.NewValue);
            if (value > 3)
            {
                value = 3;
            }

            if (value < 0.1)
            {
                value = 0.1;
            }

            ((DanmakuView)d).SetDanmakuSizeZoom(value);
        }

        private static void OnDanmakuDurationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var value = Convert.ToInt32(e.NewValue);
            if (value <= 0)
            {
                value = 1;
                ((DanmakuView)d).DanmakuDuration = value;
            }
        }

        private static void OnDanmakuAreaChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var value = Convert.ToDouble(e.NewValue);
            if (value <= 0)
            {
                value = 0.1;
            }

            if (value > 1)
            {
                value = 1;
            }

            ((DanmakuView)d).DanmakuArea = value;
        }

        private async void AddDanmakuInternalAsync(DanmakuModel m, bool isOwn)
        {
            if (!_isApplyTemplate)
            {
                return;
            }

            var danmaku = await CreateNewDanmuControlAsync(m);

            if (isOwn)
            {
                danmaku.BorderBrush = new SolidColorBrush(m.Color);
                danmaku.BorderThickness = new Thickness(1);
                danmaku.Padding = new Thickness(8, 4, 8, 4);
                danmaku.CornerRadius = new CornerRadius(4);
            }

            var rowNumber = -1;
            Grid container = null;
            List<Storyboard> locationStoryList = null;

            switch (m.Location)
            {
                case DanmakuLocation.Top:
                    rowNumber = GetTopAvailableRow();
                    container = _topContainer;
                    locationStoryList = _topBottomStoryList;
                    break;
                case DanmakuLocation.Bottom:
                    rowNumber = GetBottomAvailableRow();
                    container = _bottomContainer;
                    locationStoryList = _topBottomStoryList;
                    break;
                default:
                    rowNumber = GetScrollAvailableRow(danmaku);
                    container = _scrollContainer;
                    locationStoryList = _scrollStoryList;
                    break;
            }

            if (rowNumber == -1)
            {
                danmaku = null;
                return;
            }

            Grid.SetRow(danmaku, rowNumber);
            danmaku.HorizontalAlignment = m.Location == DanmakuLocation.Scroll ?
                HorizontalAlignment.Left : HorizontalAlignment.Center;
            danmaku.VerticalAlignment = m.Location == DanmakuLocation.Scroll ?
                VerticalAlignment.Center : VerticalAlignment.Top;
            container.Children.Add(danmaku);
            container.UpdateLayout();

            var moveTransform = new TranslateTransform();
            moveTransform.X = _rootGrid.ActualWidth;
            danmaku.RenderTransform = moveTransform;

            // 创建动画
            var duration = new Duration(TimeSpan.FromSeconds(DanmakuDuration));
            var danmakuAnimationX = new DoubleAnimation();
            danmakuAnimationX.Duration = duration;

            // 创建故事版
            var moveStoryboard = new Storyboard();
            moveStoryboard.Duration = duration;

            if (m.Location == DanmakuLocation.Scroll)
            {
                danmakuAnimationX.To = -danmaku.ActualWidth;
            }

            moveStoryboard.Children.Add(danmakuAnimationX);
            Storyboard.SetTarget(danmakuAnimationX, moveTransform);

            // 故事版加入动画
            Storyboard.SetTargetProperty(danmakuAnimationX, "X");
            locationStoryList.Add(moveStoryboard);

            moveStoryboard.Completed += new EventHandler<object>((senders, obj) =>
            {
                var danmakuContent = danmaku.Children.FirstOrDefault();
                if (danmakuContent is TextBlock txt)
                {
                    var shadow = Shadows.GetAttachedShadow(txt);
                    if (shadow != null)
                    {
                        var shadowContext = shadow.GetElementContext(txt);
                        shadowContext?.ClearAndDisposeResources();
                        shadow.DisconnectElement(txt);
                    }
                }

                container.Children.Remove(danmaku);
                danmaku.Children.Clear();
                danmaku = null;
                locationStoryList.Remove(moveStoryboard);
                moveStoryboard.Stop();
                moveStoryboard = null;
            });

            moveStoryboard.Begin();
        }

        private void SetDanmakuSizeZoom(double value)
        {
            if (!_isApplyTemplate)
            {
                return;
            }

            SetRows(this.ActualHeight);
            foreach (var item in _scrollContainer.Children)
            {
                var grid = item as Grid;
                var m = grid.Tag as DanmakuModel;
                foreach (var tb in grid.Children)
                {
                    if (tb is TextBlock)
                    {
                        (tb as TextBlock).FontSize = Convert.ToInt32(m.Size) * DanmakuSizeZoom;
                    }
                }
            }

            foreach (var item in _topContainer.Children)
            {
                var grid = item as Grid;
                var m = grid.Tag as DanmakuModel;
                foreach (var tb in grid.Children)
                {
                    if (tb is TextBlock)
                    {
                        (tb as TextBlock).FontSize = Convert.ToInt32(m.Size) * DanmakuSizeZoom;
                    }
                }
            }

            foreach (var item in _bottomContainer.Children)
            {
                var grid = item as Grid;
                var m = grid.Tag as DanmakuModel;
                foreach (var tb in grid.Children)
                {
                    if (tb is TextBlock)
                    {
                        (tb as TextBlock).FontSize = Convert.ToInt32(m.Size) * DanmakuSizeZoom;
                    }
                }
            }
        }

        private void SetRows(double height)
        {
            if (!_isApplyTemplate)
            {
                return;
            }

            var txt = new TextBlock()
            {
                Text = "测试test",
                FontSize = 25 * DanmakuSizeZoom,
            };
            txt.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            var rowHieght = txt.DesiredSize.Height;

            // 将全部行去除
            _topContainer.RowDefinitions.Clear();
            _bottomContainer.RowDefinitions.Clear();
            _scrollContainer.RowDefinitions.Clear();

            if (double.IsInfinity(height))
            {
                return;
            }

            var num = Convert.ToInt32(height / rowHieght);
            for (var i = 0; i < num; i++)
            {
                _bottomContainer.RowDefinitions.Add(new RowDefinition());
                _topContainer.RowDefinitions.Add(new RowDefinition());
                _scrollContainer.RowDefinitions.Add(new RowDefinition());
            }
        }

        private int GetTopAvailableRow()
        {
            if (!_isApplyTemplate)
            {
                return 0;
            }

            var max = _topContainer.RowDefinitions.Count / 2;

            for (var i = 0; i < max; i++)
            {
                var row = _topContainer.Children.FirstOrDefault(x => Grid.GetRow(x as Grid) == i);
                if (row != null)
                {
                    continue;
                }
                else
                {
                    return i;
                }
            }

            return -1;
        }

        private int GetBottomAvailableRow()
        {
            var max = _bottomContainer.RowDefinitions.Count / 2;
            for (var i = 1; i <= max; i++)
            {
                var rowNum = _bottomContainer.RowDefinitions.Count - i;
                var row = _bottomContainer.Children.FirstOrDefault(x => Grid.GetRow(x as Grid) == rowNum);
                if (row != null)
                {
                    continue;
                }
                else
                {
                    return rowNum;
                }
            }

            return -1;
        }

        private int GetScrollAvailableRow(Grid item)
        {
            var width = _scrollContainer.ActualWidth;

            // 计算弹幕尺寸
            item.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            var newWidth = item.DesiredSize.Width;
            if (newWidth <= 0)
            {
                return -1;
            }

            var max = _scrollContainer.RowDefinitions.Count * DanmakuArea;

            for (var i = 0; i < max; i++)
            {
                // 1、检查当前行是否存在弹幕
                var lastItem = _scrollContainer.Children.LastOrDefault(x => Grid.GetRow(x as Grid) == i);
                if (lastItem == null)
                {
                    return i;
                }

                var lastWidth = (lastItem as Grid).ActualWidth;
                var lastX = (lastItem.RenderTransform as TranslateTransform).X;

                // 2、前弹幕必须已经完全从右侧移动完毕
                if (lastX > width - lastWidth)
                {
                    continue;
                }

                // 3、后弹幕速度小于等于前弹幕速度
                var lastSpeed = (lastWidth + width) / DanmakuDuration;
                var newSpeed = (newWidth + width) / DanmakuDuration;
                if (newSpeed <= lastSpeed)
                {
                    return i;
                }

                // 4、弹幕移动期间不会重叠
                var runDistance = width - lastX;
                var t1 = (runDistance - newWidth) / (newSpeed - lastSpeed);
                var t2 = lastX / lastSpeed;
                if (t1 > t2)
                {
                    return i;
                }
            }

            return -1;
        }

        private async Task<Grid> CreateNewDanmuControlAsync(DanmakuModel m)
        {
            var builder = new DanmakuBuilder()
                .WithSizeZoom(DanmakuSizeZoom)
                .WithBold(DanmakuBold)
                .WithFontFamily(DanmakuFontFamily)
                .WithDanmakuModel(m);
            switch (DanmakuStyle)
            {
                case DanmakuStyle.NoStroke:
                    return builder.CreateNoStrokeDanmaku();
                case DanmakuStyle.Shadow:
                    return builder.CreateShadowDanmaku();
                default:
                    return await builder.CreateStrokeDanmakuAsync();
            }
        }
    }
}
