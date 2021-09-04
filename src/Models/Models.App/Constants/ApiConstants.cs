// Copyright (c) Richasy. All rights reserved.

namespace Richasy.Bili.Models.App.Constants
{
    /// <summary>
    /// API常量.
    /// </summary>
    public static class ApiConstants
    {
#pragma warning disable SA1600 // Elements should be documented
#pragma warning disable SA1401 // Fields should be private
        public const string _apiBase = "https://api.bilibili.com";
        public const string _appBase = "https://app.bilibili.com";
        public const string _vcBase = "https://api.vc.bilibili.com";
        public const string _liveBase = "https://api.live.bilibili.com";
        public const string _passBase = "https://passport.bilibili.com";
        public const string _bangumiBase = "https://bangumi.bilibili.com";
        public const string _grpcBase = "https://grpc.biliapi.net";

        public static class Passport
        {
            /// <summary>
            /// 字符串加密.
            /// </summary>
            public const string PasswordEncrypt = _passBase + "/api/oauth2/getKey";

            /// <summary>
            /// 登录.
            /// </summary>
            public const string Login = _passBase + "/api/v3/oauth2/login";

            /// <summary>
            /// 刷新令牌信息.
            /// </summary>
            public const string RefreshToken = _passBase + "/api/oauth2/refreshToken";

            /// <summary>
            /// 验证令牌是否有效.
            /// </summary>
            public const string CheckToken = _passBase + "/api/oauth2/info";

            /// <summary>
            /// SSO.
            /// </summary>
            public const string SSO = _passBase + "/api/login/sso";

            /// <summary>
            /// 获取登录二维码.
            /// </summary>
            public const string QRCode = _passBase + "/x/passport-tv-login/qrcode/auth_code";

            /// <summary>
            /// 登录二维码轮询状态.
            /// </summary>
            public const string QRCodeCheck = _passBase + "/x/passport-tv-login/qrcode/poll";
        }

        public static class Account
        {
            /// <summary>
            /// 我的信息.
            /// </summary>
            public const string MyInfo = _appBase + "/x/v2/account/myinfo";

            /// <summary>
            /// 个人主页数据信息.
            /// </summary>
            public const string Mine = _appBase + "/x/v2/account/mine";

            /// <summary>
            /// 用户空间.
            /// </summary>
            public const string Space = _appBase + "/x/v2/space";

            /// <summary>
            /// 用户空间中视频的增量请求.
            /// </summary>
            public const string VideoCursor = _appBase + "/x/v2/space/archive/cursor";

            /// <summary>
            /// 修改与用户间的关系（关注/取消关注）.
            /// </summary>
            public const string ModifyRelation = _apiBase + "/x/relation/modify";

            /// <summary>
            /// 历史记录标签页.
            /// </summary>
            public const string HistoryTabs = _grpcBase + "/bilibili.app.interface.v1.History/HistoryTabV2";

            /// <summary>
            /// 历史记录指针.
            /// </summary>
            public const string HistoryCursor = _grpcBase + "/bilibili.app.interface.v1.History/CursorV2";

            /// <summary>
            /// 删除单条历史记录.
            /// </summary>
            public const string DeleteHistoryItem = _grpcBase + "/bilibili.app.interface.v1.History/Delete";

            /// <summary>
            /// 清空历史记录.
            /// </summary>
            public const string ClearHistory = _grpcBase + "/bilibili.app.interface.v1.History/Clear";

            /// <summary>
            /// 获取粉丝列表.
            /// </summary>
            public const string Fans = _apiBase + "/x/relation/followers";

            /// <summary>
            /// 获取关注列表.
            /// </summary>
            public const string Follows = _apiBase + "/x/relation/followings";

            /// <summary>
            /// 获取稍后再看列表.
            /// </summary>
            public const string ViewLaterList = _apiBase + "/x/v2/history/toview";

            /// <summary>
            /// 添加视频到稍后再看.
            /// </summary>
            public const string ViewLaterAdd = _apiBase + "/x/v2/history/toview/add";

            /// <summary>
            /// 删除稍后再看的视频.
            /// </summary>
            public const string ViewLaterDelete = _apiBase + "/x/v2/history/toview/del";

            /// <summary>
            /// 清空稍后再看的视频.
            /// </summary>
            public const string ViewLaterClear = _apiBase + "/x/v2/history/toview/clear";

            /// <summary>
            /// 获取全部收藏夹列表.
            /// </summary>
            public const string FavoriteList = _apiBase + "/x/v3/fav/folder/created/list-all";

            /// <summary>
            /// 获取视频收藏夹概览.
            /// </summary>
            public const string VideoFavoriteGallery = _apiBase + "/x/v3/fav/folder/space/v2";

            /// <summary>
            /// 获取视频收藏夹增量信息.
            /// </summary>
            public const string VideoFavoriteDelta = _apiBase + "/x/v3/fav/resource/list";

            /// <summary>
            /// 获取动漫收藏信息.
            /// </summary>
            public const string AnimeFavorite = _apiBase + "/pgc/app/follow/v2/bangumi";

            /// <summary>
            /// 电影电视剧收藏信息.
            /// </summary>
            public const string CinemaFavorite = _apiBase + "/pgc/app/follow/v2/cinema";

            /// <summary>
            /// 专栏文章收藏信息.
            /// </summary>
            public const string ArticleFavorite = _appBase + "/x/v2/favorite/article";
        }

        public static class Partition
        {
            /// <summary>
            /// 分区索引（包含子分区数据）.
            /// </summary>
            public const string PartitionIndex = _appBase + "/x/v2/region/index";

            /// <summary>
            /// 推荐子分区.
            /// </summary>
            public const string SubPartitionRecommend = _appBase + "/x/v2/region/dynamic";

            /// <summary>
            /// 推荐子分区的增量加载.
            /// </summary>
            public const string SubPartitionRecommendOffset = _appBase + "/x/v2/region/dynamic/list";

            /// <summary>
            /// 常规子分区.
            /// </summary>
            public const string SubPartitionNormal = _appBase + "/x/v2/region/dynamic/child";

            /// <summary>
            /// 常规子分区的增量加载.
            /// </summary>
            public const string SubPartitionNormalOffset = _appBase + "/x/v2/region/dynamic/child/list";

            /// <summary>
            /// 子分区排序增量加载.
            /// </summary>
            public const string SubPartitionOrderOffset = _appBase + "/x/v2/region/show/child/list";
        }

        public static class Home
        {
            /// <summary>
            /// 推荐视频.
            /// </summary>
            public const string Recommend = _appBase + "/x/v2/feed/index";

            /// <summary>
            /// 热门 - gRPC.
            /// </summary>
            public const string PopularGRPC = _grpcBase + "/bilibili.app.show.v1.Popular/Index";

            /// <summary>
            /// 排行榜 - Web.
            /// </summary>
            public const string Ranking = _apiBase + "/x/web-interface/ranking/v2";

            /// <summary>
            /// 排行榜 - gRPC.
            /// </summary>
            public const string RankingGRPC = _grpcBase + "/bilibili.app.show.v1.Rank/RankRegion";
        }

        public static class Live
        {
            /// <summary>
            /// 直播源（首页内容）.
            /// </summary>
            public const string LiveFeed = _liveBase + "/xlive/app-interface/v2/index/feed";

            /// <summary>
            /// 直播间详情.
            /// </summary>
            public const string RoomDetail = _liveBase + "/xlive/app-room/v1/index/getInfoByRoom";

            /// <summary>
            /// 直播播放信息.
            /// </summary>
            public const string PlayInformation = _liveBase + "/xlive/web-room/v1/index/getRoomPlayInfo";

            /// <summary>
            /// 聊天套接字地址.
            /// </summary>
            public const string ChatSocket = "wss://broadcastlv.chat.bilibili.com/sub";

            /// <summary>
            /// 进入直播间.
            /// </summary>
            public const string EnterRoom = _liveBase + "/xlive/app-room/v1/index/roomEntryAction";

            /// <summary>
            /// 发送消息.
            /// </summary>
            public const string SendMessage = _liveBase + "/api/sendmsg";
        }

        public static class Article
        {
            /// <summary>
            /// 专栏分区.
            /// </summary>
            public const string Categories = _apiBase + "/x/article/categories";

            /// <summary>
            /// 专栏首页，推荐内容.
            /// </summary>
            public const string Recommend = _apiBase + "/x/article/recommends/plus";

            /// <summary>
            /// 各个分区下的文章列表.
            /// </summary>
            public const string ArticleList = _apiBase + "/x/article/recommends";
        }

        public static class Pgc
        {
            /// <summary>
            /// 顶部标签.
            /// </summary>
            public const string Tab = _apiBase + "/pgc/page/tab";

            /// <summary>
            /// 页面详情.
            /// </summary>
            public const string PageDetail = _apiBase + "/pgc/page";

            /// <summary>
            /// 剧集详情.
            /// </summary>
            public const string SeasonDetail = _apiBase + "/pgc/view/v2/app/season";

            /// <summary>
            /// 剧集播放信息.
            /// </summary>
            public const string PlayInformation = _apiBase + "/pgc/player/web/playurl";

            /// <summary>
            /// 分集交互信息.
            /// </summary>
            public const string EpisodeInteraction = _apiBase + "/pgc/season/episode/community";

            /// <summary>
            /// 追番/追剧.
            /// </summary>
            public const string Follow = _apiBase + "/pgc/app/follow/add";

            /// <summary>
            /// 取消追番/追剧.
            /// </summary>
            public const string Unfollow = _apiBase + "/pgc/app/follow/del";

            /// <summary>
            /// PGC索引条件.
            /// </summary>
            public const string IndexCondition = _apiBase + "/pgc/season/index/condition";

            /// <summary>
            /// PGC索引筛选结果.
            /// </summary>
            public const string IndexResult = _apiBase + "/pgc/season/index/result";

            /// <summary>
            /// 时间表.
            /// </summary>
            public const string TimeLine = _apiBase + "/pgc/app/timeline";

            /// <summary>
            /// 播放列表.
            /// </summary>
            public const string PlayList = _apiBase + "/pgc/web/playlist";
        }

        public static class Video
        {
            /// <summary>
            /// 视频详情.
            /// </summary>
            public const string Detail = _appBase + "/bilibili.app.view.v1.View/View";

            /// <summary>
            /// 在线观看人数.
            /// </summary>
            public const string OnlineViewerCount = _appBase + "/x/v2/view/video/online";

            /// <summary>
            /// 视频播放信息.
            /// </summary>
            public const string PlayInformation = _apiBase + "/x/player/playurl";

            /// <summary>
            /// 视频播放信息.
            /// </summary>
            public const string PlayConfig = _appBase + "/bilibili.app.playurl.v1.PlayURL/PlayConf";

            /// <summary>
            /// 弹幕元数据.
            /// </summary>
            public const string DanmakuMetaData = _grpcBase + "/bilibili.community.service.dm.v1.DM/DmView";

            /// <summary>
            /// 分段弹幕.
            /// </summary>
            public const string SegmentDanmaku = _grpcBase + "/bilibili.community.service.dm.v1.DM/DmSegMobile";

            /// <summary>
            /// 历史记录.
            /// </summary>
            public const string ProgressReport = _apiBase + "/x/v2/history/report";

            /// <summary>
            /// 点赞视频.
            /// </summary>
            public const string Like = _appBase + "/x/v2/view/like";

            /// <summary>
            /// 给视频投币.
            /// </summary>
            public const string Coin = _appBase + "/x/v2/view/coin/add";

            /// <summary>
            /// 添加或删除视频收藏.
            /// </summary>
            public const string ModifyFavorite = _apiBase + "/x/v3/fav/resource/deal";

            /// <summary>
            /// 一键三连.
            /// </summary>
            public const string Triple = _appBase + "/x/v2/view/like/triple";
        }

        public static class Search
        {
            /// <summary>
            /// 搜索推荐.
            /// </summary>
            public const string Square = _appBase + "/x/v2/search/square";

            /// <summary>
            /// 综合搜索.
            /// </summary>
            public const string ComprehensiveSearch = _appBase + "/x/v2/search";

            /// <summary>
            /// 子模块搜索，包括PGC，用户和文章.
            /// </summary>
            public const string SubModuleSearch = _appBase + "/x/v2/search/type";

            /// <summary>
            /// 直播搜索.
            /// </summary>
            public const string LiveModuleSearch = _appBase + "/x/v2/search/live";
        }
    }
#pragma warning restore SA1401 // Fields should be private
#pragma warning restore SA1600 // Elements should be documented
}
