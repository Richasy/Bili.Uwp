// Copyright (c) Richasy. All rights reserved.

using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Bili.Lib.Interfaces;
using Bili.Models.App.Constants;
using Bili.Models.App.Other;
using Newtonsoft.Json;

namespace Bili.Lib.Uwp
{
    /// <summary>
    /// 应用更新相关.
    /// </summary>
    public class UpdateProvider : IUpdateProvider
    {
        private const string LatestReleaseUrl = "https://api.github.com/repos/Richasy/Bili.Uwp/releases/latest";

        /// <inheritdoc/>
        public async Task<GithubReleaseResponse> GetGithubLatestReleaseAsync()
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add(ServiceConstants.Headers.UserAgent, ServiceConstants.DefaultUserAgentString);
                var request = new HttpRequestMessage(HttpMethod.Get, LatestReleaseUrl);
                var response = await httpClient.SendAsync(request, new CancellationTokenSource(TimeSpan.FromSeconds(5)).Token);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<GithubReleaseResponse>(content);
            }
        }
    }
}
