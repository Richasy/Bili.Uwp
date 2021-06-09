// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Richasy.Bili.Lib.Interfaces;
using Richasy.Bili.Locator.Uwp;
using Richasy.Bili.Models.BiliBili;
using Richasy.Bili.Models.Enums;
using Richasy.Bili.Toolkit.Interfaces;
using Windows.Security.Cryptography.Core;
using static Richasy.Bili.Models.App.Constants.ServiceConstants;

namespace Richasy.Bili.Lib.Uwp
{
    /// <summary>
    /// 授权模块的属性集及扩展.
    /// </summary>
    public partial class AuthorizeProvider
    {
        private readonly IMD5Toolkit _md5Toolkit;
        private readonly ISettingsToolkit _settingsToolkit;
        private AuthorizeState _state;
        private string _accessToken;

        private async Task<AuthorizeResult> ShowAccountManagementPaneAndGetResultAsync()
        {
            var webAccountProviderTaskCompletionSource = new TaskCompletionSource<AuthorizeResult>();

            return await webAccountProviderTaskCompletionSource.Task;
        }

        internal async Task<AuthorizeResult> InternalLoginAsync(string userName, string password, string captcha)
        {
            var queryParameters = new Dictionary<string, object>
            {
                { Query.UserName, userName },
                { Query.Password, EncryptedPasswordAsync(password) },
            };

            if (!string.IsNullOrEmpty(captcha))
            {
                queryParameters.Add(Query.Captcha, captcha);
            }

            var httpProvider = ServiceLocator.Instance.GetService<IHttpProvider>();
            var request = await httpProvider.GetRequestMessageAsync(HttpMethod.Post, Api.Passport.Login, queryParameters);
            var response = await httpProvider.SendAsync(request);
            var result = await httpProvider.ParseAsync<ServerResponse<AuthorizeResult>>(response);
            return result.Data;
        }

        internal async Task<string> EncryptedPasswordAsync(string password)
        {
            string base64String;
            try
            {
                var httpProvider = ServiceLocator.Instance.GetService<IHttpProvider>();
                var param = await GenerateAuthorizedQueryStringAsync(null, RequestClientType.Android);
                var request = new HttpRequestMessage(HttpMethod.Post, Api.Passport.PasswordEncrypt);
                request.Content = new StringContent(param, System.Text.Encoding.UTF8, Headers.FormUrlEncodedContentType);
                var response = await httpProvider.SendAsync(request);
                var responseContent = await response.Content.ReadAsStringAsync();
                var jobj = JObject.Parse(responseContent);
                var str = jobj["data"]["hash"].ToString();
                var str1 = jobj["data"]["key"].ToString();
                var str2 = string.Concat(str, password);
                var str3 = Regex.Match(str1, "BEGIN PUBLIC KEY-----(?<key>[\\s\\S]+)-----END PUBLIC KEY").Groups["key"].Value.Trim();
                var numArray = Convert.FromBase64String(str3);
                var asymmetricKeyAlgorithmProvider = AsymmetricKeyAlgorithmProvider.OpenAlgorithm(AsymmetricAlgorithmNames.RsaPkcs1);
                var cryptographicKey = asymmetricKeyAlgorithmProvider.ImportPublicKey(WindowsRuntimeBufferExtensions.AsBuffer(numArray), 0);
                var buffer = CryptographicEngine.Encrypt(cryptographicKey, WindowsRuntimeBufferExtensions.AsBuffer(System.Text.Encoding.UTF8.GetBytes(str2)), null);
                base64String = Convert.ToBase64String(WindowsRuntimeBufferExtensions.ToArray(buffer));
            }
            catch (Exception)
            {
                base64String = password;
            }

            return base64String;
        }

        private long GetNowSeconds()
        {
            return DateTimeOffset.Now.ToLocalTime().ToUnixTimeSeconds();
        }

        private long GetNowMilliSeconds()
        {
            return DateTimeOffset.Now.ToLocalTime().ToUnixTimeMilliseconds();
        }
    }
}
