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
        public static string DefaultAcceptString = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9";
        public static string BuildNumber = "5520400";

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
        }

        public static class Messages
        {
            public const string NotFound = "没有找到你所需要的资源";
            public const string UnexpectedExceptionOnSend = "在发送请求时出现了异常";
            public const string RequestTimedOut = "请求超时";
            public const string OverallTimeoutCannotBeSet = "全局超时未能在第一次请求后设置";
            public const string UnexpectedExceptionResponse = "在获取响应时出现了异常";
        }

        public static class Headers
        {
            public const string Bearer = "Bearer";
            public const string FormUrlEncodedContentType = "application/x-www-form-urlencoded";
            public const string JsonContentType = "application/json";
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
        }
#pragma warning restore SA1401 // Fields should be private
#pragma warning restore SA1600 // Elements should be documented
    }
}
