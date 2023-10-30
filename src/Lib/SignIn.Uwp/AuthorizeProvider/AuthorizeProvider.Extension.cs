// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Bili.DI.Container;
using Bili.Lib.Interfaces;
using Bili.Models.App.Constants;
using Bili.Models.App.Other;
using Bili.Models.BiliBili;
using Bili.Models.Enums;
using Bili.Toolkit.Interfaces;
using Newtonsoft.Json.Linq;
using QRCoder;
using Windows.Security.Cryptography.Core;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Web.Http.Filters;
using static Bili.Models.App.Constants.ApiConstants;
using static Bili.Models.App.Constants.ServiceConstants;

namespace Bili.SignIn.Uwp
{
    /// <summary>
    /// 授权模块的属性集及扩展.
    /// </summary>
    public partial class AuthorizeProvider
    {
        private readonly IMD5Toolkit _md5Toolkit;
        private readonly ISettingsToolkit _settingsToolkit;
        private readonly string _guid;
        private readonly bool _isXbox;
        private AuthorizeState _state;
        private TokenInfo _tokenInfo;
        private DateTimeOffset _lastAuthorizeTime;
        private string _internalQRAuthCode;
        private DispatcherTimer _qrTimer;
        private CancellationTokenSource _qrPollCancellationTokenSource;

        internal async Task<AuthorizeResult> InternalLoginAsync(string userName, string password, int geeType = 10, Dictionary<string, string> additionalParams = null)
        {
            var encryptedPwd = await EncryptedPasswordAsync(password);
            var queryParameters = new Dictionary<string, string>
            {
                { Query.UserName, Uri.EscapeDataString(userName) },
                { Query.Password, Uri.EscapeDataString(encryptedPwd) },
                { Query.GeeType, geeType.ToString() },
            };

            if (additionalParams != null)
            {
                foreach (var item in additionalParams)
                {
                    if (!queryParameters.ContainsKey(item.Key))
                    {
                        queryParameters.Add(item.Key, item.Value);
                    }
                }
            }

            var httpProvider = Locator.Instance.GetService<IHttpProvider>();
            var query = await GenerateAuthorizedQueryDictionaryAsync(queryParameters, RequestClientType.Login);
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

                    var httpProvider = Locator.Instance.GetService<IHttpProvider>();
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

        internal void GenerateAppKey(Dictionary<string, string> queryParameters, RequestClientType clientType, bool onlyAppKey = false)
        {
            if (clientType == RequestClientType.IOS)
            {
                queryParameters.Add(ServiceConstants.Query.AppKey, ServiceConstants.Keys.IOSKey);
                if (!onlyAppKey)
                {
                    queryParameters.Add(ServiceConstants.Query.MobileApp, "iphone");
                    queryParameters.Add(ServiceConstants.Query.Platform, "ios");
                    queryParameters.Add(ServiceConstants.Query.TimeStamp, GetNowSeconds().ToString());
                }
            }
            else if (clientType == RequestClientType.Web)
            {
                queryParameters.Add(ServiceConstants.Query.AppKey, ServiceConstants.Keys.WebKey);
                if (!onlyAppKey)
                {
                    queryParameters.Add(ServiceConstants.Query.Platform, "web");
                    queryParameters.Add(ServiceConstants.Query.TimeStamp, GetNowMilliSeconds().ToString());
                }
            }
            else if (clientType == RequestClientType.Login)
            {
                queryParameters.Add(ServiceConstants.Query.AppKey, ServiceConstants.Keys.LoginKey);
                if (!onlyAppKey)
                {
                    queryParameters.Add(ServiceConstants.Query.TimeStamp, GetNowMilliSeconds().ToString());
                }
            }
            else
            {
                queryParameters.Add(ServiceConstants.Query.AppKey, ServiceConstants.Keys.AndroidKey);
                if (!onlyAppKey)
                {
                    queryParameters.Add(ServiceConstants.Query.MobileApp, "android");
                    queryParameters.Add(ServiceConstants.Query.Platform, "android");
                    queryParameters.Add(ServiceConstants.Query.TimeStamp, GetNowSeconds().ToString());
                }
            }
        }

        internal string GenerateSign(Dictionary<string, string> queryParameters, RequestClientType clientType)
        {
            var queryList = queryParameters.Select(p => $"{p.Key}={p.Value}").ToList();
            queryList.Sort();

            var apiSecret = string.Empty;

            switch (clientType)
            {
                case RequestClientType.IOS:
                    apiSecret = Keys.IOSSecret;
                    break;
                case RequestClientType.Android:
                    apiSecret = Keys.AndroidSecret;
                    break;
                case RequestClientType.Login:
                    apiSecret = Keys.LoginSecret;
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
                var httpProvider = Locator.Instance.GetService<IHttpProvider>();
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

        internal async Task<string> GetConfirmUriAsync()
        {
            var url = "https://passport.bilibili.com/login/app/third?appkey=27eb53fc9058f8c3&api=http%3A%2F%2Flink.acg.tv%2Fforum.php&sign=67ec798004373253d60114caaad89a8c";

            try
            {
                using (var httpClient = new Windows.Web.Http.HttpClient())
                {
                    var result = await httpClient.GetStringAsync(new Uri(url));
                    var jobj = JObject.Parse(result);
                    if (Convert.ToInt32(jobj["code"].ToString()) == 0)
                    {
                        return jobj["data"]["confirm_uri"].ToString();
                    }
                }
            }
            catch (Exception)
            {
            }

            return null;
        }

        internal async Task<BitmapImage> GetQRImageAsync()
        {
            try
            {
                StopQRLoginListener();
                var queryParameters = new Dictionary<string, string>
                {
                    { Query.LocalId, _guid },
                };
                var httpProvider = Locator.Instance.GetService<IHttpProvider>();
                var request = await httpProvider.GetRequestMessageAsync(HttpMethod.Post, Passport.QRCode, queryParameters, RequestClientType.Login);
                var response = await httpProvider.SendAsync(request);
                var result = await httpProvider.ParseAsync<ServerResponse<QRInfo>>(response);

                _internalQRAuthCode = result.Data.AuthCode;
                var generator = new QRCodeGenerator();
                var data = generator.CreateQrCode(result.Data.Url, QRCodeGenerator.ECCLevel.Q);
                var code = new BitmapByteQRCode(data);
                var image = code.GetGraphic(20);
                using (var stream = new InMemoryRandomAccessStream())
                {
                    using (var writer = new DataWriter(stream.GetOutputStreamAt(0)))
                    {
                        writer.WriteBytes(image);
                        await writer.StoreAsync();
                    }

                    var bitmap = new BitmapImage();
                    await bitmap.SetSourceAsync(stream);
                    return bitmap;
                }
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
                { "guid", Guid.NewGuid().ToString() },
            };

            try
            {
                var httpProvider = Locator.Instance.GetService<IHttpProvider>();
                var request = await httpProvider.GetRequestMessageAsync(HttpMethod.Post, Passport.QRCodeCheck, queryParameters, RequestClientType.Login);
                var response = await httpProvider.SendAsync(request, _qrPollCancellationTokenSource.Token);
                var result = await httpProvider.ParseAsync<ServerResponse<TokenInfo>>(response);

                // 保存cookie
                SaveCookie(result.Data.CookieInfo);
                SaveAuthorizeResult(result.Data);
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
                    if (se.Error.Code == 86039 || se.Error.Code == 86090)
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
            var httpProvider = Locator.Instance.GetService<IHttpProvider>();
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
                    var httpProvider = Locator.Instance.GetService<IHttpProvider>();
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

            var dialog = new SignInDialog(webAccountProviderTaskCompletionSource, this);
            dialog.IsShowSwitchButton = !_isXbox;
            await dialog.ShowAsync();

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
                    [Settings.RefreshTokenKey] = result.RefreshToken ?? string.Empty,
                    [Settings.UserIdKey] = result.Mid,
                    [Settings.ExpiresInKey] = result.ExpiresIn,
                    [Settings.LastSaveAuthTimeKey] = saveTime.ToUnixTimeSeconds(),
                };

                CurrentUserId = result.Mid.ToString();
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

                CurrentUserId = tokenInfo.Mid.ToString();
                _tokenInfo = tokenInfo;
                _lastAuthorizeTime = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(data[Settings.LastSaveAuthTimeKey]));
                State = AuthorizeState.SignedIn;
            }
            else
            {
                _tokenInfo = null;
                _lastAuthorizeTime = default;
            }
        }

        private void SaveCookie(CookieInfo cookieInfo)
        {
            var domain = ApiConstants.CookieSetDomain;

            if (cookieInfo != null && cookieInfo.Cookies != null)
            {
                var filter = new HttpBaseProtocolFilter();
                foreach (var cookieItem in cookieInfo.Cookies)
                {
                    filter.CookieManager.SetCookie(new Windows.Web.Http.HttpCookie(cookieItem.Name, domain, "/")
                    {
                        HttpOnly = cookieItem.HttpOnly == 1,
                        Secure = cookieItem.Secure == 1,
                        Expires = DateTimeOffset.FromUnixTimeSeconds(cookieItem.Expires),
                        Value = cookieItem.Value,
                    });
                }
            }
        }

        private async Task<string> GetCookieToAccessKeyConfirmUrlAsync()
        {
            try
            {
                var httpProvider = Locator.Instance.GetService<IHttpProvider>();
                var query = new Dictionary<string, string>
                {
                    { "api", ApiConstants.Passport.LoginAppThirdApi },
                };
                var request = await httpProvider.GetRequestMessageAsync(HttpMethod.Get, Passport.LoginAppThird, query, type: RequestClientType.IOS, needCookie: true, needAppKey: true);
                var response = await httpProvider.SendAsync(request);
                var result = await httpProvider.ParseAsync<ServerResponse<LoginAppThird>>(response);
                return result.Data.ConfirmUri;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task<string> GetAccessKeyAsync(string confirmUri)
        {
            try
            {
                var httpProvider = Locator.Instance.GetService<IHttpProvider>();
                var request = await httpProvider.GetRequestMessageAsync(HttpMethod.Get, confirmUri, needCookie: true);
                var response = await httpProvider.SendAsync(request);
                var success = response.Headers.TryGetValues("location", out var locations);
                if (!success)
                {
                    return default;
                }

                var redirectUrl = locations.FirstOrDefault();
                var uri = new Uri(redirectUrl);
                var queries = HttpUtility.ParseQueryString(uri.Query);
                var accessKey = queries.Get("access_key");
                return accessKey;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
