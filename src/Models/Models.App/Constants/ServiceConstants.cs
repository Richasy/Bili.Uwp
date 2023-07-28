// Copyright (c) Richasy. All rights reserved.

namespace Bili.Models.App.Constants
{
    /// <summary>
    /// 服务相关的常量.
    /// </summary>
    public static class ServiceConstants
    {
#pragma warning disable SA1600 // Elements should be documented
#pragma warning disable SA1401 // Fields should be private
        public const string DefaultAcceptString = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9";
        public const string DefaultUserAgentString = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/92.0.4515.107 Safari/537.36 Edg/92.0.902.62";
        public const string BuildNumber = "5520400";
        public const string Av = "av";
        public const string Bangumi = "bangumi";
        public const string Pgc = "pgc";

        public const string Season = "season";
        public const string Positive = "positive";
        public const string Section = "section";

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
            public const string IOSKey = "27eb53fc9058f8c3";
            public const string IOSSecret = "c2ed53a74eeefe3cf99fbd01d8c9c375";
            public const string WebKey = "84956560bc028eb7";
            public const string WebSecret = "94aba54af9065f71de72f5508f1cd42e";
            public const string LoginKey = "4409e2ce8ffd12b8";
            public const string LoginSecret = "59b43e04ad6965f34319062b478f83dd";
        }

        public static class Query
        {
            public const string AppKey = "appkey";
            public const string Build = "build";
            public const string MobileApp = "mobi_app";
            public const string Platform = "platform";
            public const string PlatformSlim = "plat";
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
            public const string OrderSort = "order_sort";
            public const string Pull = "pull";
            public const string PageNumber = "pn";
            public const string PageSizeSlim = "ps";
            public const string Type = "type";
            public const string SubType = "sub_type";
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
            public const string RealTime = "realtime";
            public const string Like = "like";
            public const string Multiply = "multiply";
            public const string AlsoLike = "select_like";
            public const string AddFavoriteIds = "add_media_ids";
            public const string DeleteFavoriteIds = "del_media_ids";
            public const string AutoPlay = "autoplay";
            public const string IsShowAllSeries = "is_show_all_series";
            public const string SeasonId = "season_id";
            public const string EpisodeId = "ep_id";
            public const string EpisodeIdSlim = "epid";
            public const string SeasonIdSlim = "sid";
            public const string Module = "module";
            public const string SeasonType = "season_type";
            public const string RoomId = "room_id";
            public const string From = "from";
            public const string Limit = "limit";
            public const string Keyword = "keyword";
            public const string Recommend = "recommend";
            public const string Duration = "duration";
            public const string HighLight = "highlight";
            public const string IsOrgQuery = "is_org_query";
            public const string FullCategoryId = "category_id";
            public const string UserType = "user_type";
            public const string PlayUrl = "play_url";
            public const string VMid = "vmid";
            public const string Fid = "fid";
            public const string ActionSlim = "act";
            public const string ActionFull = "action";
            public const string ReSrc = "re_src";
            public const string ActionKey = "actionKey";
            public const string MessageSlim = "msg";
            public const string MessageFull = "message";
            public const string Rnd = "rnd";
            public const string Mode = "mode";
            public const string Pool = "pool";
            public const string Color = "color";
            public const string FontSize = "fontsize";
            public const string PlayTime = "playTime";
            public const string TagId = "tagid";
            public const string UpId = "up_mid";
            public const string Area = "area";
            public const string CopyRight = "copyright";
            public const string IsFinish = "is_finish";
            public const string PageSizeFull = "pagesize";
            public const string PageSizeUnderline = "page_size";
            public const string SeasonMonth = "season_month";
            public const string SeasonStatus = "season_status";
            public const string SeasonVersion = "season_version";
            public const string SpokenLanguageType = "spoken_language_type";
            public const string StyleId = "style_id";
            public const string Year = "year";
            public const string IndexType = "index_type";
            public const string FilterType = "filter_type";
            public const string Id = "id";
            public const string MediaId = "media_id";
            public const string MediaIds = "media_ids";
            public const string Status = "status";
            public const string Resources = "resources";
            public const string Oid = "oid";
            public const string ReplyId = "rpid";
            public const string Parent = "parent";
            public const string Root = "root";
            public const string LikeTime = "like_time";
            public const string AtTime = "at_time";
            public const string ReplyTime = "reply_time";
            public const string Protocol = "protocol";
            public const string Codec = "codec";
            public const string PType = "ptype";
            public const string Format = "format";
            public const string GraphVersion = "graph_version";
            public const string EdgeId = "edge_id";
            public const string LoginSessionId = "login_session_id";
            public const string GeeSeccode = "gee_seccode";
            public const string GeeValidate = "gee_validate";
            public const string GeeChallenge = "gee_challenge";
            public const string RecaptchaToken = "recaptcha_token";
            public const string AreaId = "area_id";
            public const string CateId = "cate_id";
            public const string ParentAreaId = "parent_area_id";
            public const string SortType = "sort_type";
            public const string NoPlayUrl = "no_playurl";
            public const string Dolby = "dolby";
            public const string Http = "http";
            public const string OnlyAudio = "only_audio";
            public const string OnlyVideo = "only_video";
            public const string NeedHdr = "need_hdr";
            public const string Mask = "mask";
            public const string PlayType = "play_type";
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
            public const string Buvid = "buvid";
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

        public static class Search
        {
            public const string OrderType = "orderType";
            public const string OrderSort = "orderSort";
            public const string Duration = "duration";
            public const string PartitionId = "partitionId";
            public const string TotalRank = "totalrank";
            public const string UserType = "userType";
        }
#pragma warning restore SA1401 // Fields should be private
#pragma warning restore SA1600 // Elements should be documented
    }
}
