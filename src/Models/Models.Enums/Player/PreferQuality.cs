// Copyright (c) Richasy. All rights reserved.

namespace Bili.Models.Enums.Player
{
    /// <summary>
    /// 偏好的画质类型.
    /// </summary>
    public enum PreferQuality
    {
        /// <summary>
        /// 自动，会根据上一次播放的清晰度进行选择.
        /// </summary>
        Auto,

        /// <summary>
        /// 优先播放 1080P 的片源，如果没有，则播放次一级的片源.
        /// </summary>
        HDFirst,

        /// <summary>
        /// 画质优先，会优先选取最高清晰度的片源.
        /// </summary>
        HighQuality,
    }
}
