// Copyright (c) Richasy. All rights reserved.

using System;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using Microsoft.Graphics.Canvas.Text;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Richasy.Bili.App.Controls
{
    /// <summary>
    /// 弹幕组件构建器.
    /// </summary>
    public class DanmakuBuilder
    {
        private double _sizeZoom = 1d;
        private bool _isBold = false;
        private string _fontFamily = "Segoe UI";
        private DanmakuModel _model = null;

        /// <summary>
        /// 设置缩放比例.
        /// </summary>
        /// <param name="sizeZoom">缩放比例.</param>
        /// <returns>构造器.</returns>
        public DanmakuBuilder WithSizeZoom(double sizeZoom)
        {
            _sizeZoom = sizeZoom;
            return this;
        }

        /// <summary>
        /// 设置字体是否加粗.
        /// </summary>
        /// <param name="isBold">是否加粗.</param>
        /// <returns>构造器.</returns>
        public DanmakuBuilder WithBold(bool isBold)
        {
            _isBold = isBold;
            return this;
        }

        /// <summary>
        /// 设置字体.
        /// </summary>
        /// <param name="fontFamily">字体名.</param>
        /// <returns>构造器.</returns>
        public DanmakuBuilder WithFontFamily(string fontFamily)
        {
            _fontFamily = fontFamily;
            return this;
        }

        /// <summary>
        /// 设置弹幕模型.
        /// </summary>
        /// <param name="model">模型.</param>
        /// <returns>构造器.</returns>
        public DanmakuBuilder WithDanmakuModel(DanmakuModel model)
        {
            _model = model;
            return this;
        }

        /// <summary>
        /// 创建重叠弹幕.
        /// </summary>
        /// <returns>弹幕容器.</returns>
        public Grid CreateOverlapDanamku()
        {
            if (_model == null)
            {
                throw new ArgumentNullException("未传入弹幕模型.");
            }

            // 创建基础控件
            var tx = new TextBlock();
            var tx2 = new TextBlock();
            var grid = new Grid();

            tx2.Text = _model.Text;
            tx.Text = _model.Text;

            if (_isBold)
            {
                tx.FontWeight = FontWeights.Bold;
                tx2.FontWeight = FontWeights.Bold;
            }

            if (!string.IsNullOrEmpty(_fontFamily))
            {
                tx.FontFamily = new FontFamily(_fontFamily);
                tx2.FontFamily = new FontFamily(_fontFamily);
            }

            tx2.Foreground = _model.Foreground;
            tx.Foreground = _model.Foreground;

            // 弹幕大小
            var size = _model.Size * _sizeZoom;

            tx2.FontSize = size;
            tx.FontSize = size;

            tx2.Margin = new Thickness(1);

            // Grid包含弹幕文本信息
            grid.Children.Add(tx2);
            grid.Children.Add(tx);
            grid.Tag = _model;
            return grid;
        }

        /// <summary>
        /// 创建描边弹幕.
        /// </summary>
        /// <returns>弹幕容器.</returns>
        public Grid CreateStrokeDanmaku()
        {
            if (_model == null)
            {
                throw new ArgumentNullException("未传入弹幕模型.");
            }

            var size = _model.Size * _sizeZoom;
            var canvasControl = new CanvasControl();
            var container = new Grid();
            container.Children.Add(canvasControl);
            CanvasDrawingSession session = null;
            CanvasTextFormat format = null;
            CanvasTextLayout layout = null;
            canvasControl.Draw += (control, args) =>
            {
                session = args.DrawingSession;
                format = new CanvasTextFormat() { FontSize = (float)size, WordWrapping = CanvasWordWrapping.NoWrap };
                layout = new CanvasTextLayout(session, _model.Text, format, 0f, 0f);
                control.Width = layout.DrawBounds.Width;
                control.Height = layout.DrawBounds.Height;
                var textGeo = CanvasGeometry.CreateText(layout);
                session.DrawGeometry(textGeo, _model.Color.R < 60 ? Colors.White : Colors.Black);
                session.DrawTextLayout(layout, 0f, 0f, _model.Color);
            };
            container.Unloaded += (s, e) =>
            {
                if (session != null)
                {
                    session.Dispose();
                }

                if (format != null)
                {
                    format.Dispose();
                }

                if (layout != null)
                {
                    layout.Dispose();
                }
            };

            return container;
        }

        /// <summary>
        /// 创建无边框弹幕.
        /// </summary>
        /// <returns>弹幕容器.</returns>
        public Grid CreateNoStrokeDanmaku()
        {
            if (_model == null)
            {
                throw new ArgumentNullException("未传入弹幕模型.");
            }

            // 创建基础控件
            var tx = new TextBlock();
            var grid = new Grid();

            tx.Text = _model.Text;
            if (_isBold)
            {
                tx.FontWeight = FontWeights.Bold;
            }

            if (string.IsNullOrEmpty(_fontFamily))
            {
                tx.FontFamily = new FontFamily(_fontFamily);
            }

            tx.Foreground = _model.Foreground;
            var size = _model.Size * _sizeZoom;
            tx.FontSize = size;
            grid.Children.Add(tx);
            grid.Tag = _model;
            return grid;
        }
    }
}
