// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Richasy.Bili.Lib.Interfaces;
using Richasy.Bili.Models.App.Args;
using Richasy.Bili.Models.App.Constants;
using Richasy.Bili.Models.Enums;
using Richasy.Bili.Toolkit.Interfaces;

namespace Richasy.Bili.Lib.Uwp
{
    /// <summary>
    /// 授权模块.
    /// </summary>
    public partial class AuthorizeProvider : IAuthorizeProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizeProvider"/> class.
        /// </summary>
        /// <param name="md5Toolkit">MD5工具.</param>
        public AuthorizeProvider(IMD5Toolkit md5Toolkit)
        {
            _md5Toolkit = md5Toolkit;
        }

        /// <inheritdoc/>
        public event EventHandler<AuthorizeStateChangedEventArgs> StateChanged;

        /// <inheritdoc/>
        public AuthorizeState State
        {
            get => _state;
            protected set
            {
                var oldState = _state;
                var newState = value;
                if (oldState != newState)
                {
                    _state = newState;
                    StateChanged?.Invoke(this, new AuthorizeStateChangedEventArgs(oldState, newState));
                }
            }
        }

        /// <inheritdoc/>
        public async Task<string> GenerateAuthorizedQueryStringAsync(Dictionary<string, object> queryParameters)
        {
            var query = string.Empty;
            if (!string.IsNullOrEmpty(_accessToken))
            {
                queryParameters.Add(ServiceConstants.Query.AccessToken, _accessToken);
            }
            else
            {
                await GetTokenAsync();
            }

            var queryList = queryParameters.Select(p => $"{p.Key}={p.Value}").ToList();
            queryList.Sort();

            var apiKey = queryParameters[ServiceConstants.Query.AppKey].ToString();
            var apiSecret = string.Empty;
            if (apiKey == ServiceConstants.Keys.IOSKey)
            {
                apiSecret = ServiceConstants.Keys.IOSSecret;
            }
            else if (apiKey == ServiceConstants.Keys.AndroidKey)
            {
                apiSecret = ServiceConstants.Keys.AndroidSecret;
            }
            else
            {
                apiSecret = ServiceConstants.Keys.WebSecret;
            }

            query = string.Join('&', queryList);
            var signQuery = query + $"&{apiSecret}";
            var sign = _md5Toolkit.GetMd5String(signQuery).ToLower();
            return query + $"&sign={sign}";
        }

        /// <inheritdoc/>
        public Task<string> GetTokenAsync(bool silentOnly = false) => throw new NotImplementedException();

        /// <inheritdoc/>
        public Task SignInAsync() => throw new NotImplementedException();

        /// <inheritdoc/>
        public Task SignOutAsync() => throw new NotImplementedException();

        /// <inheritdoc/>
        public Task<bool> TrySilentSignInAsync() => throw new NotImplementedException();
    }
}
