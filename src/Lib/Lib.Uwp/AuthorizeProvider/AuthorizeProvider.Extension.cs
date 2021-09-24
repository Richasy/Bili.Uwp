// Copyright (c) GodLeaveMe. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Richasy.Bili.Lib.Interfaces;
using Richasy.Bili.Locator.Uwp;
using Richasy.Bili.Models.App.Other;
using Richasy.Bili.Models.BiliBili;
using Richasy.Bili.Models.Enums;
using Richasy.Bili.Toolkit.Interfaces;
using Windows.Security.Cryptography.Core;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;
using ZXing;
using ZXing.Common;
using static Richasy.Bili.Models.App.Constants.ApiConstants;
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
        private readonly string _guid;
        private AuthorizeState _state;
        private TokenInfo _tokenInfo;
        private DateTimeOffset _lastAuthorizeTime;
        private string _internalQRAuthCode;
        private DispatcherTimer _qrTimer;
        private CancellationTokenSource _qrPollCancellationTokenSource;

        internal async Task<AuthorizeResult> InternalLoginAsync(string userName, string password, int geeType = 10)
        {
            var encryptedPwd = await EncryptedPasswordAsync(password);
            var queryParameters = new Dictionary<string, string>
            {
                { Query.UserName, Uri.EscapeDataString(userName) },
                { Query.Password, Uri.EscapeDataString(encryptedPwd) },
                { Query.GeeType, geeType.ToString() },
            };

            var httpProvider = ServiceLocator.Instance.GetService<IHttpProvider>();
            var query = await GenerateAuthorizedQueryDictionaryAsync(queryParameters, RequestClientType.Android);
            query[Query.UserName] = userName;
            query[Query.Password] = encryptedPwd;
            var request = new HttpRequestMessage(HttpMethod.Post, Passport.Login);
            request.Content = new FormUrlEncodedContent(query);
            var response = await httpProvider.SendAsync(request);
            var result = await httpProvider.ParseAsync<ServerResponse<AuthorizeResult>>(response);
            await SSOInitAsync();
            return result.Data;
        }

        internal async Task<TokenInfo> InternalRefreshTokenAsync()
        {
            try
            {
                if (!string.IsNullOrEmpty(_tokenInfo?.RefreshToken))
                {
                    var queryParameters = new Dictionary<string, string>
                    {
                        { Query.AccessToken, _tokenInfo.AccessToken },
                        { Query.RefreshToken, _tokenInfo.RefreshToken },
                    };

                    var httpProvider = ServiceLocator.Instance.GetService<IHttpProvider>();
                    var request = await httpProvider.GetRequestMessageAsync(HttpMethod.Post, Passport.RefreshToken, queryParameters);
                    var response = await httpProvider.SendAsync(request);
                    var result = await httpProvider.ParseAsync<ServerResponse<TokenInfo>>(response);
                    await SSOInitAsync();

                    return result.Data;
                }
            }
            catch
            {
            }

            return null;
        }

        internal string GenerateSign(Dictionary<string, string> queryParameters)
        {
            var queryList = queryParameters.Select(p => $"{p.Key}={p.Value}").ToList();
            queryList.Sort();

            var apiKey = queryParameters[Query.AppKey].ToString();
            var apiSecret = string.Empty;

            switch (apiKey)
            {
                case Keys.IOSKey:
                    apiSecret = Keys.IOSSecret;
                    break;
                case Keys.AndroidKey:
                    apiSecret = Keys.AndroidSecret;
                    break;
                default:
                    apiSecret = Keys.WebSecret;
                    break;
            }

            var query = string.Join('&', queryList);
            var signQuery = query + apiSecret;
            var sign = _md5Toolkit.GetMd5String(signQuery).ToLower();
            return sign;
        }

        internal async Task<string> EncryptedPasswordAsync(string password)
        {
            string base64String;
            try
            {
                var httpProvider = ServiceLocator.Instance.GetService<IHttpProvider>();
                var param = await GenerateAuthorizedQueryDictionaryAsync(null, RequestClientType.Android);
                var request = new HttpRequestMessage(HttpMethod.Post, Passport.PasswordEncrypt);
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

        internal async Task<WriteableBitmap> GetQRImageAsync()
        {
            try
            {
                StopQRLoginListener();
                var queryParameters = new Dictionary<string, string>
                {
                    { Query.LocalId, _guid },
                };
                var httpProvider = ServiceLocator.Instance.GetService<IHttpProvider>();
                var request = await httpProvider.GetRequestMessageAsync(HttpMethod.Post, Passport.QRCode, queryParameters);
                var response = await httpProvider.SendAsync(request);
                var result = await httpProvider.ParseAsync<ServerResponse<QRInfo>>(response);

                _internalQRAuthCode = result.Data.AuthCode;
                var barcodeWriter = new BarcodeWriter();
                barcodeWriter.Format = BarcodeFormat.QR_CODE;
                barcodeWriter.Options = new EncodingOptions()
                {
                    Margin = 1,
                    Height = 200,
                    Width = 200,
                };

                var img = barcodeWriter.Write(result.Data.Url);
                return img;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 开始本地轮询二维码状态.
        /// </summary>
        internal void StartQRLoginListener()
        {
            if (_qrTimer == null)
            {
                _qrTimer = new DispatcherTimer();
                _qrTimer.Interval = TimeSpan.FromSeconds(3);
                _qrTimer.Tick += OnQRTimerTickAsync;
            }

            _qrTimer?.Start();
        }

        internal void StopQRLoginListener()
        {
            _qrTimer?.Stop();
            _qrTimer = null;
            CleanQRCodeCancellationToken();
        }

        private async void OnQRTimerTickAsync(object sender, object e)
        {
            if (await IsTokenValidAsync())
            {
                StopQRLoginListener();
                return;
            }

            CleanQRCodeCancellationToken();
            _qrPollCancellationTokenSource = new CancellationTokenSource();
            var queryParameters = new Dictionary<string, string>
            {
                { Query.AuthCode, _internalQRAuthCode },
                { Query.LocalId, _guid },
            };

            try
            {
                var httpProvider = ServiceLocator.Instance.GetService<IHttpProvider>();
                var request = await httpProvider.GetRequestMessageAsync(HttpMethod.Post, Passport.QRCodeCheck, queryParameters);
                var response = await httpProvider.SendAsync(request, _qrPollCancellationTokenSource.Token);
                var result = await httpProvider.ParseAsync<ServerResponse<TokenInfo>>(response);

                QRCodeStatusChanged?.Invoke(this, new Tuple<QRCodeStatus, TokenInfo>(QRCodeStatus.Success, result.Data));
            }
            catch (ServiceException se)
            {
                if (se.InnerException is TaskCanceledException)
                {
                    return;
                }

                if (se.Error != null)
                {
                    QRCodeStatus qrStatus = default;
                    if (se.Error.Code == 86039)
                    {
                        qrStatus = QRCodeStatus.NotConfirm;
                    }
                    else if (se.Error.Code == 86038 || se.Error.Code == -3)
                    {
                        qrStatus = QRCodeStatus.Expiried;
                    }
                    else
                    {
                        qrStatus = QRCodeStatus.Failed;
                    }

                    QRCodeStatusChanged?.Invoke(this, new Tuple<QRCodeStatus, TokenInfo>(qrStatus, null));
                }
            }
        }

        private void CleanQRCodeCancellationToken()
        {
            if (_qrPollCancellationTokenSource != null)
            {
                if (_qrPollCancellationTokenSource.Token.CanBeCanceled)
                {
                    _qrPollCancellationTokenSource.Cancel();
                }

                _qrPollCancellationTokenSource.Dispose();
                _qrPollCancellationTokenSource = null;
            }
        }

        private async Task SSOInitAsync()
        {
            var url = Passport.SSO;
            var httpProvider = ServiceLocator.Instance.GetService<IHttpProvider>();
            var request = await httpProvider.GetRequestMessageAsync(HttpMethod.Get, url);
            await httpProvider.SendAsync(request);
        }

        private async Task<bool> NetworkVerifyTokenAsync()
        {
            if (!string.IsNullOrEmpty(_tokenInfo?.AccessToken))
            {
                var queryParameters = new Dictionary<string, string>
                {
                    { Query.AccessToken, _tokenInfo.AccessToken },
                };

                try
                {
                    var httpProvider = ServiceLocator.Instance.GetService<IHttpProvider>();
                    var request = await httpProvider.GetRequestMessageAsync(HttpMethod.Get, Passport.CheckToken, queryParameters);
                    _ = await httpProvider.SendAsync(request);
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }

            return false;
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

        private void SaveAuthorizeResult(TokenInfo result)
        {
            if (result != null)
            {
                var saveTime = DateTimeOffset.Now;
                var compositeValue = new ApplicationDataCompositeValue
                {
                    [Settings.AccessTokenKey] = result.AccessToken,
                    [Settings.RefreshTokenKey] = result.RefreshToken,
                    [Settings.UserIdKey] = result.Mid,
                    [Settings.ExpiresInKey] = result.ExpiresIn,
                    [Settings.LastSaveAuthTimeKey] = saveTime.ToUnixTimeSeconds(),
                };

                ApplicationData.Current.LocalSettings.Values[Settings.AuthResultKey] = compositeValue;
                _lastAuthorizeTime = saveTime;
                _tokenInfo = result;
                State = AuthorizeState.SignedIn;
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
    }
}
