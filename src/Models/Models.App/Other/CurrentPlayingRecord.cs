// Copyright (c) Richasy. All rights reserved.

using Richasy.Bili.Models.Enums;

namespace Richasy.Bili.Models.App.Other
{
    /// <summary>
    /// 当前正在播放内容的快照.
    /// </summary>
    public class CurrentPlayingRecord
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CurrentPlayingRecord"/> class.
        /// </summary>
        public CurrentPlayingRecord()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CurrentPlayingRecord"/> class.
        /// </summary>
        /// <param name="vid">视频ID.</param>
        /// <param name="sid">剧集ID.</param>
        /// <param name="type">视频类型.</param>
        public CurrentPlayingRecord(string vid, int sid, VideoType type)
        {
            VideoId = vid;
            SeasonId = sid;
            VideoType = type;
        }

        /// <summary>
        /// 视频Id.
        /// </summary>
        public string VideoId { get; set; }

        /// <summary>
        /// 剧集Id.
        /// </summary>
        public int SeasonId { get; set; }

        /// <summary>
        /// 视频类型.
        /// </summary>
        public VideoType VideoType { get; set; }

        /// <summary>
        /// 是否为关联视频.
        /// </summary>
        public bool IsRelated { get; set; }
    }
}
