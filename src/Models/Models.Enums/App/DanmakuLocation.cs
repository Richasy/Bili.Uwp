// Copyright (c) Richasy. All rights reserved.

namespace Richasy.Bili.Models.Enums.App
{
    /// <summary>
    /// 弹幕位置.
    /// </summary>
    public enum DanmakuLocation
    {
        /// <summary>
        /// 滚动弹幕Model1-3.
        /// </summary>
        Scroll = 1,

        /// <summary>
        /// 顶部弹幕Model5.
        /// </summary>
        Top = 5,

        /// <summary>
        /// 底部弹幕Model4.
        /// </summary>
        Bottom = 4,

        /// <summary>
        /// 反向滚动弹幕.
        /// </summary>
        ReverseRolling = 6,

        /// <summary>
        /// 定位弹幕Model7.
        /// </summary>
        Position = 7,

        /// <summary>
        /// 其它暂未支持的类型.
        /// </summary>
        Other = 9,
    }
}
