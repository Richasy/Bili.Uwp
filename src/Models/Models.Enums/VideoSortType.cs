// Copyright (c) Richasy. All rights reserved.

namespace Richasy.Bili.Models.Enums
{
    /// <summary>
    /// 视频排序方式.
    /// </summary>
    public enum VideoSortType
    {
        /// <summary>
        /// 默认排序.
        /// </summary>
        Default,

        /// <summary>
        /// 最新视频优先.
        /// </summary>
        Newest,

        /// <summary>
        /// 最多播放优先.
        /// </summary>
        Play,

        /// <summary>
        /// 最多回复优先.
        /// </summary>
        Reply,

        /// <summary>
        /// 最多弹幕优先.
        /// </summary>
        Danmaku,

        /// <summary>
        /// 最多收藏优先.
        /// </summary>
        Favorite,
    }
}
