// Copyright (c) Richasy. All rights reserved.

using System.Threading.Tasks;
using Richasy.Bili.Models.App.Other;

namespace Richasy.Bili.Lib.Interfaces
{
    /// <summary>
    /// 应用更新操作.
    /// </summary>
    public interface IUpdateProvider
    {
        /// <summary>
        /// 获取Github最新的发布版本.
        /// </summary>
        /// <returns>最新发布版本.</returns>
        Task<GithubReleaseResponse> GetGithubLatestReleaseAsync();
    }
}
