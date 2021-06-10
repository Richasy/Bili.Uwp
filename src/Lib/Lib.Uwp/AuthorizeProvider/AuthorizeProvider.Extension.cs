// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
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
using Windows.Storage;
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
        private TokenInfo _tokenInfo;
        private DateTimeOffset _lastAuthorizeTime;

        internal async Task<AuthorizeResult> InternalLoginAsync(string userName, string password, string captcha)
        {
            var encryptedPwd = await EncryptedPasswordAsync(password);
            var queryParameters = new Dictionary<string, string>
            {
                { Query.UserName, Uri.EscapeDataString(userName) },
                { Query.Password, Uri.EscapeDataString(encryptedPwd) },
            };

            if (!string.IsNullOrEmpty(captcha))
            {
                queryParameters.Add(Query.Captcha, captcha);
            }

            var httpProvider = ServiceLocator.Instance.GetService<IHttpProvider>();
            var query = await GenerateAuthorizedQueryDictionaryAsync(queryParameters, RequestClientType.Android);
            query[Query.UserName] = userName;
            query[Query.Password] = encryptedPwd;
            var request = new HttpRequestMessage(HttpMethod.Post, Api.Passport.Login);
            request.Content = new FormUrlEncodedContent(query);
            var response = await httpProvider.SendAsync(request);
            var result = await httpProvider.ParseAsync<ServerResponse<AuthorizeResult>>(response);
            await SSOInitAsync(result.Data.SSO.FirstOrDefault());
            return result.Data;
        }

        internal async Task<string> EncryptedPasswordAsync(string password)
        {
            string base64String;
            try
            {
                var httpProvider = ServiceLocator.Instance.GetService<IHttpProvider>();
                var param = await GenerateAuthorizedQueryDictionaryAsync(null, RequestClientType.Android);
                var request = new HttpRequestMessage(HttpMethod.Post, Api.Passport.PasswordEncrypt);
                request.Content = new FormUrlEncodedContent(param);
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

        private async Task SSOInitAsync(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                url = Api.Passport.SSO;
            }

            var httpProvider = ServiceLocator.Instance.GetService<IHttpProvider>();
            var request = await httpProvider.GetRequestMessageAsync(HttpMethod.Get, url);
            await httpProvider.SendAsync(request);
        }

        private async Task<AuthorizeResult> ShowAccountManagementPaneAndGetResultAsync()
        {
            var webAccountProviderTaskCompletionSource = new TaskCompletionSource<AuthorizeResult>();
            var loginDialog = new AccountLoginDialog(webAccountProviderTaskCompletionSource);
            await loginDialog.ShowAsync();
            return await webAccountProviderTaskCompletionSource.Task;
        }

        private long GetNowSeconds()
        {
            return DateTimeOffset.Now.ToLocalTime().ToUnixTimeSeconds();
        }

        private long GetNowMilliSeconds()
        {
            return DateTimeOffset.Now.ToLocalTime().ToUnixTimeMilliseconds();
        }

        private void SaveAuthorizeResult(AuthorizeResult result)
        {
            if (result != null)
            {
                var saveTime = DateTimeOffset.Now;
                var compositeValue = new ApplicationDataCompositeValue
                {
                    [Settings.AccessTokenKey] = result.TokenInfo.AccessToken,
                    [Settings.RefreshTokenKey] = result.TokenInfo.RefreshToken,
                    [Settings.UserIdKey] = result.TokenInfo.Mid,
                    [Settings.ExpiresInKey] = result.TokenInfo.ExpiresIn,
                    [Settings.LastSaveAuthTimeKey] = saveTime.ToUnixTimeSeconds(),
                };

                ApplicationData.Current.LocalSettings.Values[Settings.AuthResultKey] = compositeValue;
                _lastAuthorizeTime = saveTime;
            }
        }

        private void RetrieveAuthorizeResult()
        {
            var localSettings = ApplicationData.Current.LocalSettings;
            if (localSettings.Values.ContainsKey(Settings.AuthResultKey))
            {
                var data = (ApplicationDataCompositeValue)localSettings.Values[Settings.AuthResultKey];
                var tokenInfo = new TokenInfo
                {
                    AccessToken = data[Settings.AccessTokenKey].ToString(),
                    RefreshToken = data[Settings.RefreshTokenKey].ToString(),
                    Mid = Convert.ToInt64(data[Settings.UserIdKey]),
                    ExpiresIn = (int)data[Settings.ExpiresInKey],
                };

                _tokenInfo = tokenInfo;
                _lastAuthorizeTime = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(data[Settings.LastSaveAuthTimeKey]));
            }
            else
            {
                _tokenInfo = null;
                _lastAuthorizeTime = default;
            }
        }

        private bool IsTokenValid()
        {
            return _tokenInfo != null &&
                !string.IsNullOrEmpty(_tokenInfo.AccessToken) &&
                _lastAuthorizeTime != null &&
                (DateTimeOffset.Now - _lastAuthorizeTime).TotalSeconds < _tokenInfo.ExpiresIn;
        }
    }
}
