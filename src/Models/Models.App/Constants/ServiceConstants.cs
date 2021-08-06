// Copyright (c) Richasy. All rights reserved.

namespace Richasy.Bili.Models.App.Constants
{
    /// <summary>
    /// 服务相关的常量.
    /// </summary>
    public static class ServiceConstants
    {
#pragma warning disable SA1600 // Elements should be documented
#pragma warning disable SA1401 // Fields should be private
        public const string DefaultAcceptString = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9";
        public const string DefaultUserAgentString = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/69.0.3497.100 Safari/537.36";
        public const string BuildNumber = "5520400";
        public const string Av = "av";
        public const string Bangumi = "bangumi";
        public const string Pgc = "pgc";

        /// <summary>
        /// 番剧分区Id.
        /// </summary>
        public const int BangumiPartitionId = 152;

        /// <summary>
        /// 国创分区Id.
        /// </summary>
        public const int DomesticPartitionId = 167;

        public const string BangumiOperation = "bangumi-operation";
        public const string DomesticOperation = "gc-operation";
        public const string MovieOperation = "movie-operation";
        public const string TvOperation = "tv-operation";
        public const string DocumentaryOperation = "documentary-operation";

        public static class Keys
        {
            public const string AndroidKey = "4409e2ce8ffd12b8";
            public const string AndroidSecret = "59b43e04ad6965f34319062b478f83dd";
            public const string IOSKey = "4ebafd7c4951b366";
            public const string IOSSecret = "8cb98205e9b2ad3669aad0fce12a4c13";
            public const string WebKey = "84956560bc028eb7";
            public const string WebSecret = "94aba54af9065f71de72f5508f1cd42e";
        }

        public static class Query
        {
            public const string AppKey = "appkey";
            public const string Build = "build";
            public const string MobileApp = "mobi_app";
            public const string Platform = "platform";
            public const string TimeStamp = "ts";
            public const string AccessKey = "access_key";
            public const string Password = "password";
            public const string UserName = "username";
            public const string Captcha = "captcha";
            public const string AccessToken = "access_token";
            public const string RefreshToken = "refresh_token";
            public const string Sign = "sign";
            public const string GeeType = "gee_type";
            public const string LocalId = "local_id";
            public const string AuthCode = "auth_code";
            public const string PartitionId = "rid";
            public const string CreateTime = "ctime";
            public const string Order = "order";
            public const string Pull = "pull";
            public const string PageNumber = "pn";
            public const string PageSize = "ps";
            public const string Type = "type";
            public const string Idx = "idx";
            public const string Flush = "flush";
            public const string Device = "device";
            public const string DeviceName = "device_name";
            public const string Column = "column";
            public const string Page = "page";
            public const string RelationPage = "relation_page";
            public const string Scale = "scale";
            public const string LoginEvent = "login_event";
            public const string CategoryId = "cid";
            public const string MyId = "mid";
            public const string Sort = "sort";
            public const string ParentTab = "parent_tab_name";
            public const string IsHideRecommendTab = "hide_rcmd_tab";
            public const string Fnval = "fnval";
            public const string Fnver = "fnver";
            public const string Fourk = "fourk";
            public const string Qn = "qn";
            public const string TabId = "tab_id";
            public const string TeenagersMode = "teenagers_mode";
            public const string Cursor = "cursor";
            public const string Name = "name";
            public const string Aid = "aid";
            public const string AVid = "avid";
            public const string Cid = "cid";
            public const string OType = "otype";
            public const string Progress = "progress";
        }

        public static class Sort
        {
            public const string Newest = "senddate";
            public const string Play = "view";
            public const string Reply = "reply";
            public const string Danmaku = "danmaku";
            public const string Favorite = "favorite";
        }

        public static class Messages
        {
            public const string NotFound = "没有找到你所需要的资源";
            public const string NoData = "请求失败，没有数据返回";
            public const string UnexpectedExceptionOnSend = "在发送请求时出现了异常";
            public const string RequestTimedOut = "请求超时";
            public const string OverallTimeoutCannotBeSet = "全局超时未能在第一次请求后设置";
            public const string UnexpectedExceptionResponse = "在获取响应时出现了异常";
        }

        public static class Headers
        {
            public const string Bearer = "Bearer";
            public const string Identify = "identify_v1";
            public const string FormUrlEncodedContentType = "application/x-www-form-urlencoded";
            public const string JsonContentType = "application/json";
            public const string GRPCContentType = "application/grpc";
            public const string UserAgent = "User-Agent";
            public const string Referer = "Referer";
            public const string AppKey = "APP-KEY";
            public const string BiliMeta = "x-bili-metadata-bin";
            public const string Authorization = "authorization";
            public const string BiliDevice = "x-bili-device-bin";
            public const string BiliNetwork = "x-bili-network-bin";
            public const string BiliRestriction = "x-bili-restriction-bin";
            public const string BiliLocale = "x-bili-locale-bin";
            public const string BiliFawkes = "x-bili-fawkes-req-bin";
            public const string GRPCAcceptEncodingKey = "grpc-accept-encoding";
            public const string GRPCAcceptEncodingValue = "identity,deflate,gzip";
            public const string GRPCTimeOutKey = "grpc-timeout";
            public const string GRPCTimeOutValue = "20100m";
            public const string Envoriment = "env";
            public const string TransferEncodingKey = "Transfer-Encoding";
            public const string TransferEncodingValue = "chunked";
            public const string TEKey = "TE";
            public const string TEValue = "trailers";
        }

        public static class Settings
        {
            public const string AccessTokenKey = "accessToken";
            public const string RefreshTokenKey = "refreshToken";
            public const string UserIdKey = "userId";
            public const string ExpiresInKey = "expiresIn";
            public const string AuthResultKey = "authorizeResult";
            public const string LastSaveAuthTimeKey = "lastSaveAuthorizeResultTime";
        }

        public static class Api
        {
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
                public const string LiveFeed = _liveBase + "/xlive/app-interface/v2/index/feed";
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
                public const string Detail = _apiBase + "/pgc/page";
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
            }
        }
#pragma warning restore SA1401 // Fields should be private
#pragma warning restore SA1600 // Elements should be documented
    }
}
