// Copyright (c) Richasy. All rights reserved.

using System.Net.Http;
using System.Threading.Tasks;
using Richasy.Bili.Lib.Interfaces;
using Richasy.Bili.Models.BiliBili;
using static Richasy.Bili.Models.App.Constants.ServiceConstants;

namespace Richasy.Bili.Lib.Uwp
{
    /// <summary>
    /// 提供已登录用户的数据操作.
    /// </summary>
    public partial class AccountProvider : IAccountProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AccountProvider"/> class.
        /// </summary>
        /// <param name="httpProvider">网络操作工具.</param>
        public AccountProvider(IHttpProvider httpProvider)
        {
            _httpProvider = httpProvider;
        }

        /// <inheritdoc/>
        public async Task<MyInfo> GetMyInformationAsync()
        {
            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Get, Api.Account.MyInfo, type: Models.Enums.RequestClientType.IOS);
            var response = await _httpProvider.SendAsync(request);
            var result = await _httpProvider.ParseAsync<ServerResponse<MyInfo>>(response);
            _mid = result.Data.Mid;
            return result.Data;
        }
    }
}
