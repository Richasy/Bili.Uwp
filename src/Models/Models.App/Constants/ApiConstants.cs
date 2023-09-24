// Copyright (c) Richasy. All rights reserved.

namespace Bili.Models.App.Constants
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
        public const string _passSnmBase = "https://passport.snm0516.aisee.tv";
        public const string _bangumiBase = "https://bangumi.bilibili.com";
        public const string _grpcBase = "https://grpc.biliapi.net";

        public const string CookieGetDomain = "https://bilibili.com";
        public const string CookieSetDomain = "bilibili.com";

        public static class Passport
        {
            /// <summary>
            /// 字符串加密.
            /// </summary>
            public const string PasswordEncrypt = _passBase + "/api/oauth2/getKey";

            /// <summary>
            /// 登录.
            /// </summary>
            public const string Login = _passBase + "/x/passport-login/oauth2/login";

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
            public const string QRCode = _passSnmBase + "/x/passport-tv-login/qrcode/auth_code";

            /// <summary>
            /// 登录二维码轮询状态.
            /// </summary>
            public const string QRCodeCheck = _passSnmBase + "/x/passport-tv-login/qrcode/poll";

            /// <summary>
            /// cookie转访问令牌.
            /// </summary>
            public const string LoginAppThird = _passBase + "/login/app/third";

            /// <summary>
            /// cookie转访问令牌.
            /// </summary>
            public const string LoginAppThirdApi = "http://link.acg.tv/forum.php";
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

            public const string SpaceVideoSearch = _grpcBase + "/bilibili.app.interface.v1.Space/SearchArchive";

            /// <summary>
            /// 用户空间中视频的增量请求.
            /// </summary>
            public const string VideoCursor = _appBase + "/x/v2/space/archive/cursor";

            /// <summary>
            /// 获取与用户间的关系（关注与否）.
            /// </summary>
            public const string Relation = _apiBase + "/x/relation";

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
            /// 获取我的关注分组.
            /// </summary>
            public const string MyFollowingTags = _apiBase + "/x/relation/tags";

            /// <summary>
            /// 获取我的关注分组详情.
            /// </summary>
            public const string MyFollowingTagDetail = _apiBase + "/x/relation/tag";

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
            /// 获取用户收集的视频收藏夹分类的增量信息.
            /// </summary>
            public const string CollectedVideoFavoriteFolderDelta = _apiBase + "/x/v3/fav/folder/collected/list";

            /// <summary>
            /// 获取用户创建的视频收藏夹分类的增量信息.
            /// </summary>
            public const string CreatedVideoFavoriteFolderDelta = _apiBase + "/x/v3/fav/folder/created/list";

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

            /// <summary>
            /// 取消关注收藏夹.
            /// </summary>
            public const string UnFavoriteFolder = _apiBase + "/x/v3/fav/folder/unfav";

            /// <summary>
            /// 取消关注视频.
            /// </summary>
            public const string UnFavoriteVideo = _apiBase + "/x/v3/fav/resource/batch-del";

            /// <summary>
            /// 取消关注番剧或影视.
            /// </summary>
            public const string UnFavoritePgc = _apiBase + "/pgc/app/follow/del";

            /// <summary>
            /// 取消关注文章.
            /// </summary>
            public const string UnFavoriteArticle = _apiBase + "/x/article/favorites/del";

            /// <summary>
            /// 添加视频收藏夹.
            /// </summary>
            public const string AddFavoriteFolder = _apiBase + "/x/v3/fav/folder/add";

            /// <summary>
            /// 删除视频收藏夹.
            /// </summary>
            public const string DeleteFavoriteFolder = _apiBase + "/x/v3/fav/folder/del";

            /// <summary>
            /// 获取未读消息.
            /// </summary>
            public const string MessageUnread = _apiBase + "/x/msgfeed/unread";

            /// <summary>
            /// 获取点赞消息.
            /// </summary>
            public const string MessageLike = _apiBase + "/x/msgfeed/like";

            /// <summary>
            /// 获取@我的消息.
            /// </summary>
            public const string MessageAt = _apiBase + "/x/msgfeed/at";

            /// <summary>
            /// 获取回复我的消息.
            /// </summary>
            public const string MessageReply = _apiBase + "/x/msgfeed/reply";

            /// <summary>
            /// 更新PGC收藏状态.
            /// </summary>
            public const string UpdatePgcStatus = _apiBase + "/pgc/app/follow/status/update";
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
            /// 直播分区.
            /// </summary>
            public const string LiveArea = _liveBase + "/xlive/app-interface/v2/index/getAreaList";

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

            /// <summary>
            /// 分区详情.
            /// </summary>
            public const string AreaDetail = _liveBase + "/xlive/app-interface/v2/second/getList";

            /// <summary>
            /// 移动应用上的播放信息.
            /// </summary>
            public const string AppPlayInformation = _liveBase + "/xlive/app-room/v2/index/getRoomPlayInfo";
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

            /// <summary>
            /// 文章内容.
            /// </summary>
            public const string ArticleContent = "https://www.bilibili.com/read/app/";
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

            /// <summary>
            /// 剧集详情.
            /// </summary>
            /// <param name="proxy">代理服务器地址.</param>
            /// <returns>API地址.</returns>
            public static string SeasonDetail(string proxy = "")
            {
                var prefix = string.IsNullOrEmpty(proxy)
                    ? _apiBase
                    : proxy;
                return prefix.TrimEnd('/') + "/pgc/view/v2/app/season";
            }

            /// <summary>
            /// 剧集播放信息.
            /// </summary>
            /// <param name="proxy">代理服务器地址.</param>
            /// <returns>API地址.</returns>
            public static string PlayInformation(string proxy = "")
            {
                var prefix = string.IsNullOrEmpty(proxy)
                    ? _apiBase
                    : proxy;
                return prefix.TrimEnd('/') + "/pgc/player/web/playurl";
            }
        }

        public static class Video
        {
            /// <summary>
            /// 视频详情.
            /// </summary>
            public const string Detail = _grpcBase + "/bilibili.app.view.v1.View/View";

            /// <summary>
            /// 在线观看人数.
            /// </summary>
            public const string OnlineViewerCount = _grpcBase + "/bilibili.app.playeronline.v1.PlayerOnline/PlayerOnline";

            /// <summary>
            /// 视频播放信息.
            /// </summary>
            public const string PlayInformation = _apiBase + "/x/player/playurl";

            /// <summary>
            /// 视频播放信息.
            /// </summary>
            public const string PlayUrl = _grpcBase + "/bilibili.app.playurl.v1.PlayURL/PlayView";

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
            public const string ModifyFavorite = _apiBase + "/x/v3/fav/resource/batch-deal";

            /// <summary>
            /// 一键三连.
            /// </summary>
            public const string Triple = _appBase + "/x/v2/view/like/triple";

            /// <summary>
            /// 发送弹幕.
            /// </summary>
            public const string SendDanmaku = _apiBase + "/x/v2/dm/post";

            /// <summary>
            /// 获取视频字幕.
            /// </summary>
            public const string Subtitle = _apiBase + "/x/player.so";

            /// <summary>
            /// 获取互动视频选项.
            /// </summary>
            public const string InteractionEdge = _apiBase + "/x/stein/edgeinfo_v2";

            /// <summary>
            /// 获取视频参数.
            /// </summary>
            public const string Stat = _apiBase + "/x/web-interface/archive/stat";
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
            /// 直播搜索.
            /// </summary>
            public const string LiveModuleSearch = _appBase + "/x/v2/search/live";

            /// <summary>
            /// 搜索建议.
            /// </summary>
            public const string Suggestion = _grpcBase + "/bilibili.app.interface.v1.Search/Suggest3";

            /// <summary>
            /// 子模块搜索，包括PGC，用户和文章.
            /// </summary>
            /// <param name="proxy">代理服务器地址.</param>
            /// <returns>API地址.</returns>
            public static string SubModuleSearch(string proxy = "")
            {
                var prefix = string.IsNullOrEmpty(proxy)
                    ? _appBase
                    : proxy;
                return prefix.TrimEnd('/') + "/x/v2/search/type";
            }
        }

        public static class Community
        {
            /// <summary>
            /// 评论列表.
            /// </summary>
            public const string ReplyMainList = _grpcBase + "/bilibili.main.community.reply.v1.Reply/MainList";

            /// <summary>
            /// 单层评论详情.
            /// </summary>
            public const string ReplyDetailList = _grpcBase + "/bilibili.main.community.reply.v1.Reply/DetailList";

            /// <summary>
            /// 点赞评论.
            /// </summary>
            public const string LikeReply = _apiBase + "/x/v2/reply/action";

            /// <summary>
            /// 添加评论.
            /// </summary>
            public const string AddReply = _apiBase + "/x/v2/reply/add";

            /// <summary>
            /// 动态标签.
            /// </summary>
            public const string DynamicTabs = _grpcBase + "/bilibili.app.dynamic.v2.Dynamic/DynTab";

            /// <summary>
            /// 综合动态列表.
            /// </summary>
            public const string DynamicAll = _grpcBase + "/bilibili.app.dynamic.v2.Dynamic/DynAll";

            /// <summary>
            /// 视频动态列表.
            /// </summary>
            public const string DynamicVideo = _grpcBase + "/bilibili.app.dynamic.v2.Dynamic/DynVideo";

            /// <summary>
            /// 未登录时的动态推荐.
            /// </summary>
            public const string DynamicWhenUnlogin = _grpcBase + "/bilibili.app.dynamic.v2.Dynamic/DynUnLoginRcmd";

            /// <summary>
            /// 点赞/取消点赞动态.
            /// </summary>
            public const string LikeDynamic = _grpcBase + "/bilibili.app.dynamic.v2.Dynamic/DynThumb";
        }
    }
#pragma warning restore SA1401 // Fields should be private
#pragma warning restore SA1600 // Elements should be documented
}
