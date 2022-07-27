// Copyright (c) Richasy. All rights reserved.

namespace Bili.Models.Enums
{
    /// <summary>
    /// 播放器状态.
    /// </summary>
    public enum PlayerStatus
    {
        /// <summary>
        /// 未加载.
        /// </summary>
        NotLoad,

        /// <summary>
        /// 播放中.
        /// </summary>
        Playing,

        /// <summary>
        /// 暂停.
        /// </summary>
        Pause,

        /// <summary>
        /// 已打开媒体流.
        /// </summary>
        Opened,

        /// <summary>
        /// 缓冲中.
        /// </summary>
        Buffering,

        /// <summary>
        /// 播放结束.
        /// </summary>
        End,

        /// <summary>
        /// 播放错误.
        /// </summary>
        Failed,
    }
}
