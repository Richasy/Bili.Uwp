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
            public static string AndroidKey = "4409e2ce8ffd12b8";
            public static string AndroidSecret = "59b43e04ad6965f34319062b478f83dd";
            public static string IOSKey = "4ebafd7c4951b366";
            public static string IOSSecret = "8cb98205e9b2ad3669aad0fce12a4c13";
            public static string WebKey = "84956560bc028eb7";
            public static string WebSecret = "94aba54af9065f71de72f5508f1cd42e";
        }

        public static class Query
        {
            public static string AppKey = "appKey";
            public static string Build = "build";
            public static string MobileApp = "mobi_app";
            public static string Platform = "platform";
            public static string TimeStamp = "ts";
            public static string AccessToken = "access_key";
        }

        public static class Messages
        {
            public static string NotFound = "没有找到你所需要的资源";
            public static string AuthenticationProviderMissing = "Authentication provider is required before sending a request.";
            public static string BaseUrlMissing = "Base URL cannot be null or empty.";
            public static string InvalidTypeForDateConverter = "DateConverter can only serialize objects of type Date.";
            public static string LocationHeaderNotSetOnRedirect = "Location header not present in redirection response.";
            public static string OverallTimeoutCannotBeSet = "Overall timeout cannot be set after the first request is sent.";
            public static string RequestTimedOut = "The request timed out.";
            public static string RequestUrlMissing = "Request URL is required to send a request.";
            public static string TooManyRedirectsFormatString = "More than {0} redirects encountered while sending the request.";
            public static string TooManyRetriesFormatString = "More than {0} retries encountered while sending the request.";
            public static string UnableToCreateInstanceOfTypeFormatString = "Unable to create an instance of type {0}.";
            public static string UnableToDeserializeDate = "Unable to deserialize the returned Date.";
            public static string UnexpectedExceptionOnSend = "An error occurred sending the request.";
            public static string UnexpectedExceptionResponse = "Unexpected exception returned from the service.";
            public static string MaximumValueExceeded = "{0} exceeds the maximum value of {1}.";
            public static string NullParameter = "{0} parameter cannot be null.";
            public static string UnableToDeserializexContent = "Unable to deserialize content.";
            public static string InvalidDependsOnRequestId = "Corresponding batch request id not found for the specified dependsOn relation.";
            public static string ExpiredUploadSession = "Upload session expired. Upload cannot resume";
            public static string NoResponseForUpload = "No Response Received for upload.";
            public static string InvalidProxyArgument = "Proxy cannot be set more once. Proxy can only be set on the proxy or defaultHttpHandler argument and not both.";
        }

        public static class Headers
        {
            public const string Bearer = "Bearer";
            public const string FormUrlEncodedContentType = "application/x-www-form-urlencoded";
            public const string JsonContentType = "application/json";
        }
#pragma warning restore SA1401 // Fields should be private
#pragma warning restore SA1600 // Elements should be documented
    }
}
