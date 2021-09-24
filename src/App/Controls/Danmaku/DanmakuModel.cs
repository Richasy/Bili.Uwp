// Copyright (c) GodLeaveMe. All rights reserved.

using Richasy.Bili.Models.Enums.App;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace Richasy.Bili.App.Controls
{
    /// <summary>
    /// 弹幕模型.
    /// </summary>
    public class DanmakuModel
    {
        /// <summary>
        /// 文本.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// 弹幕大小.
        /// </summary>
        public double Size { get; set; }

        /// <summary>
        /// 弹幕颜色.
        /// </summary>
        public Color Color { get; set; }

        /// <summary>
        /// 弹幕出现时间.
        /// </summary>
        public int Time { get; set; }

        /// <summary>
        /// 弹幕发送时间.
        /// </summary>
        public string SendTime { get; set; }

        /// <summary>
        /// 弹幕池.
        /// </summary>
        public string Pool { get; set; }

        /// <summary>
        /// 弹幕发送人ID.
        /// </summary>
        public string SendId { get; set; }

        /// <summary>
        /// 弹幕ID.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 弹幕出现位置.
        /// </summary>
        public DanmakuLocation Location
        {
            get; set;
        }

        /// <summary>
        /// 来源.
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// 弹幕字重.
        /// </summary>
        public int Weight { get; set; }

        /// <summary>
        /// 前景色.
        /// </summary>
        public SolidColorBrush Foreground
        {
            get
            {
                return new SolidColorBrush(Color);
            }
        }
    }
}
