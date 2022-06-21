// Copyright (c) Richasy. All rights reserved.

using Bili.Models.BiliBili;
using Bili.Models.Data.Player;

namespace Bili.Adapter.Interfaces
{
    /// <summary>
    /// 播放器数据适配器.
    /// </summary>
    public interface IPlayerAdapter
    {
        /// <summary>
        /// 将视频分段条目 <see cref="DashItem"/> 转换成分片信息.
        /// </summary>
        /// <param name="item">视频分段条目.</param>
        /// <returns><see cref="SegmentInformation"/>.</returns>
        SegmentInformation ConvertToSegmentInformation(DashItem item);

        /// <summary>
        /// 将视频格式 <see cref="VideoFormat"/> 转换成格式信息.
        /// </summary>
        /// <param name="format">视频格式.</param>
        /// <returns><see cref="FormatInformation"/>.</returns>
        FormatInformation ConvertToFormatInformation(VideoFormat format);

        /// <summary>
        /// 将播放器信息 <see cref="PlayerInformation"/> 转换成媒体播放信息.
        /// </summary>
        /// <param name="information">播放器信息.</param>
        /// <returns><see cref="MediaInformation"/>.</returns>
        MediaInformation ConvertToMediaInformation(PlayerInformation information);

        /// <summary>
        /// 将字幕索引条目 <see cref="SubtitleIndexItem"/> 转换成字幕元数据.
        /// </summary>
        /// <param name="item">索引条目.</param>
        /// <returns><see cref="SubtitleMeta"/>.</returns>
        SubtitleMeta ConvertToSubtitleMeta(SubtitleIndexItem item);

        /// <summary>
        /// 将字幕条目 <see cref="SubtitleItem"/> 转换成字幕信息.
        /// </summary>
        /// <param name="item">字幕条目.</param>
        /// <returns><see cref="SubtitleInformation"/>.</returns>
        SubtitleInformation ConvertToSubtitleInformation(SubtitleItem item);
    }
}
