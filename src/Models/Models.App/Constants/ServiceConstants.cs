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
            public const string AccessToken = "access_key";
            public const string Password = "password";
            public const string UserName = "username";
            public const string Captcha = "captcha";
        }

        public static class Messages
        {
            public const string NotFound = "没有找到你所需要的资源";
            public const string AuthenticationProviderMissing = "Authentication provider is required before sending a request.";
            public const string BaseUrlMissing = "Base URL cannot be null or empty.";
            public const string InvalidTypeForDateConverter = "DateConverter can only serialize objects of type Date.";
            public const string LocationHeaderNotSetOnRedirect = "Location header not present in redirection response.";
            public const string OverallTimeoutCannotBeSet = "Overall timeout cannot be set after the first request is sent.";
            public const string RequestTimedOut = "The request timed out.";
            public const string RequestUrlMissing = "Request URL is required to send a request.";
            public const string TooManyRedirectsFormatString = "More than {0} redirects encountered while sending the request.";
            public const string TooManyRetriesFormatString = "More than {0} retries encountered while sending the request.";
            public const string UnableToCreateInstanceOfTypeFormatString = "Unable to create an instance of type {0}.";
            public const string UnableToDeserializeDate = "Unable to deserialize the returned Date.";
            public const string UnexpectedExceptionOnSend = "An error occurred sending the request.";
            public const string UnexpectedExceptionResponse = "Unexpected exception returned from the service.";
            public const string MaximumValueExceeded = "{0} exceeds the maximum value of {1}.";
            public const string NullParameter = "{0} parameter cannot be null.";
            public const string UnableToDeserializexContent = "Unable to deserialize content.";
            public const string InvalidDependsOnRequestId = "Corresponding batch request id not found for the specified dependsOn relation.";
            public const string ExpiredUploadSession = "Upload session expired. Upload cannot resume";
            public const string NoResponseForUpload = "No Response Received for upload.";
            public const string InvalidProxyArgument = "Proxy cannot be set more once. Proxy can only be set on the proxy or defaultHttpHandler argument and not both.";
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
                /// 获取验证码.
                /// </summary>
                public const string Captcha = _passBase + "/captcha";
            }
        }
#pragma warning restore SA1401 // Fields should be private
#pragma warning restore SA1600 // Elements should be documented
    }
}
