// Copyright (c) Richasy. All rights reserved.

using Bili.Models.Enums;

namespace Bili.Models.Data.Local
{
    /// <summary>
    /// 当前正在播放内容的快照.
    /// </summary>
    public class PlaySnapshot
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlaySnapshot"/> class.
        /// </summary>
        public PlaySnapshot()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PlaySnapshot"/> class.
        /// </summary>
        /// <param name="vid">视频ID.</param>
        /// <param name="sid">剧集ID.</param>
        /// <param name="type">视频类型.</param>
        public PlaySnapshot(string vid, string sid, VideoType type)
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
        public string SeasonId { get; set; }

        /// <summary>
        /// 视频类型.
        /// </summary>
        public VideoType VideoType { get; set; }

        /// <summary>
        /// 是否为关联视频.
        /// </summary>
        public bool IsRelated { get; set; }

        /// <summary>
        /// 显示模式.
        /// </summary>
        public PlayerDisplayMode DisplayMode { get; set; }

        /// <summary>
        /// 标题.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 是否需要借助 BiliPlus 服务获取视频对应的剧集信息.
        /// </summary>
        public bool NeedBiliPlus { get; set; }
    }
}
