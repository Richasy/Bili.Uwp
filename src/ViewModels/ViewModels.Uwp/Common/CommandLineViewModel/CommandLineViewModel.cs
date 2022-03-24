// Copyright (c) Richasy. All rights reserved.

using CommandLine;

namespace Richasy.Bili.ViewModels.Uwp
{
    internal class CommandLineViewModel
    {
        /// <summary>
        /// 页面Id.
        /// </summary>
        [Option(shortName: 'n', longName: "navigate", Required = false, HelpText = "页面标识")]
        public string PageId { get; set; }

        /// <summary>
        /// 视频Id.
        /// </summary>
        [Option(shortName: 'v', longName: "video", Required = false, HelpText = "视频Id，支持 AV号 和 BV号")]
        public string VideoId { get; set; }

        /// <summary>
        /// PGC剧集Id.
        /// </summary>
        [Option(shortName: 's', longName: "season", Required = false, HelpText = "PGC剧集Id，指的是整个动漫或电影的Id，而非单集Id，形如 'ss11111'")]
        public string SeasonId { get; set; }

        /// <summary>
        /// PGC单集Id.
        /// </summary>
        [Option(shortName: 'e', longName: "episode", Required = false, HelpText = "PGC单集Id，并非是整个剧集的Id，形如 'ep111111'")]
        public string EpisodeId { get; set; }

        /// <summary>
        /// 直播间Id.
        /// </summary>
        [Option(shortName: 'l', longName: "live", Required = false, HelpText = "直播间Id")]
        public string LiveId { get; set; }

        /// <summary>
        /// 搜索关键词.
        /// </summary>
        [Option(shortName: 'f', longName: "find", Required = false, HelpText = "搜索关键词")]
        public string SearchWord { get; set; }

        /// <summary>
        /// 是否在迷你视窗中播放视频.
        /// </summary>
        [Option(longName: "mini", HelpText = "是否在迷你视窗中播放视频")]
        public bool IsMiniPlay { get; set; }

        /// <summary>
        /// 是否全屏播放视频.
        /// </summary>
        [Option(longName: "screen", HelpText = "是否全屏播放视频")]
        public bool IsFullScreenPlay { get; set; }

        /// <summary>
        /// 是否全窗口播放视频.
        /// </summary>
        [Option(longName: "window", HelpText = "是否全窗口播放视频")]
        public bool IsFullWindowPlay { get; set; }
    }
}
