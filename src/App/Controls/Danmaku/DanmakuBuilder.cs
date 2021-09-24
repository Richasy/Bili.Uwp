// Copyright (c) GodLeaveMe. All rights reserved.

using System;
using System.Threading.Tasks;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using Microsoft.Graphics.Canvas.Text;
using Microsoft.Toolkit.Uwp.UI;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

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
        public async Task<Grid> CreateStrokeDanmakuAsync()
        {
            if (_model == null)
            {
                throw new ArgumentNullException("未传入弹幕模型.");
            }

            var size = _model.Size * _sizeZoom;
            var container = new Grid();
            var device = CanvasDevice.GetSharedDevice();
            var format = new CanvasTextFormat() { FontSize = (float)size, WordWrapping = CanvasWordWrapping.NoWrap, FontFamily = _fontFamily };
            if (_isBold)
            {
                format.FontWeight = FontWeights.Bold;
            }

            var holderBlock = new TextBlock()
            {
                Text = _model.Text,
                FontSize = size,
                FontFamily = new FontFamily(_fontFamily),
            };
            holderBlock.Measure(new Windows.Foundation.Size(double.PositiveInfinity, double.PositiveInfinity));

            var renderTarget = new CanvasRenderTarget(device, (float)holderBlock.DesiredSize.Width, (float)holderBlock.DesiredSize.Height, 96);
            var layout = new CanvasTextLayout(device, _model.Text, format, (float)holderBlock.DesiredSize.Width, (float)holderBlock.DesiredSize.Height);
            var textGeo = CanvasGeometry.CreateText(layout);

            using (var session = renderTarget.CreateDrawingSession())
            {
                session.Clear(Colors.Transparent);
                var borderColor = _model.Color.R <= 80 ? Color.FromArgb(125, 255, 255, 255) : Color.FromArgb(125, 0, 0, 0);
                session.DrawGeometry(textGeo, borderColor, 1f, new CanvasStrokeStyle() { DashStyle = CanvasDashStyle.Solid });
                session.FillGeometry(textGeo, _model.Color);
            }

            using (var stream = new InMemoryRandomAccessStream())
            {
                var image = new Image();
                var bitmap = new BitmapImage();
                await renderTarget.SaveAsync(stream, CanvasBitmapFileFormat.Png, 1.0f);
                await bitmap.SetSourceAsync(stream);
                image.Source = bitmap;
                image.Stretch = Stretch.None;
                container.Children.Add(image);
            }

            container.Tag = _model;
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

        /// <summary>
        /// 创建阴影弹幕.
        /// </summary>
        /// <returns>弹幕容器.</returns>
        public Grid CreateShadowDanmaku()
        {
            if (_model == null)
            {
                throw new ArgumentNullException("未传入弹幕模型.");
            }

            // 创建基础控件
            var tx = new TextBlock();
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

            var grid = new Grid();
            var hostGrid = new Grid();

            var shadow = new AttachedDropShadow();
            shadow.BlurRadius = 2;
            shadow.Opacity = 0.8;
            shadow.Offset = "1,1,0";
            shadow.Color = _model.Color.R <= 80 ? Colors.White : Colors.Black;
            grid.Children.Add(hostGrid);
            grid.Children.Add(tx);
            grid.Loaded += (s, e) =>
            {
                shadow.CastTo = hostGrid;
            };

            Effects.SetShadow(tx, shadow);
            grid.Tag = _model;
            return grid;
        }
    }
}
