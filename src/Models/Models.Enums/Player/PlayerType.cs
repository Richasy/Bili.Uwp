// Copyright (c) Richasy. All rights reserved.

namespace Bili.Models.Enums.Player
{
    /// <summary>
    /// 播放器类型.
    /// </summary>
    public enum PlayerType
    {
        /// <summary>
        /// 原生播放解码，使用 AdaptiveMediaSource.
        /// </summary>
        Native,

        /// <summary>
        /// FFmpeg解码，借助 FFmpegInteropX.
        /// </summary>
        FFmpeg,

        /// <summary>
        /// 使用 VLC 播放器播放.
        /// </summary>
        Vlc,
    }
}
