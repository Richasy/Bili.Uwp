// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bili.Models.App.Args;
using Bili.Models.Enums;

namespace Bili.Lib.Interfaces
{
    /// <summary>
    /// 授权验证模块.
    /// </summary>
    public interface IAuthorizeProvider
    {
        /// <summary>
        /// 当授权状态改变时发生.
        /// </summary>
        event EventHandler<AuthorizeStateChangedEventArgs> StateChanged;

        /// <summary>
        /// 当前的授权状态.
        /// </summary>
        AuthorizeState State { get; }

        /// <summary>
        /// 当前已登录的用户Id.
        /// </summary>
        string CurrentUserId { get; }

        /// <summary>
        /// 获取包含授权码的查询字符串.
        /// </summary>
        /// <param name="queryParameters">请求所需的查询参数.</param>
        /// <param name="clientType">请求需要模拟的客户端类型.</param>
        /// <param name="needToken">是否需要令牌.</param>
        /// <param name="forceNoToken">是否强制不需要令牌.</param>
        /// <returns>包含授权验证的查询字符串.</returns>
        Task<string> GenerateAuthorizedQueryStringAsync(Dictionary<string, string> queryParameters, RequestClientType clientType, bool needToken = true, bool forceNoToken = false);

        /// <summary>
        /// 获取包含授权码的查询字典.
        /// </summary>
        /// <param name="queryParameters">请求所需的查询参数.</param>
        /// <param name="clientType">请求需要模拟的客户端类型.</param>
        /// <param name="needToken">是否需要访问令牌.</param>
        /// <param name="forceNoToken">是否强制不需要令牌.</param>
        /// <returns>包含授权验证码的查询字典.</returns>
        Task<Dictionary<string, string>> GenerateAuthorizedQueryDictionaryAsync(Dictionary<string, string> queryParameters, RequestClientType clientType, bool needToken = true, bool forceNoToken = false);

        /// <summary>
        /// 获取包含授权码的查询字典，无token且先加盐.
        /// </summary>
        /// <param name="queryParameters">请求所需的查询参数.</param>
        /// <param name="clientType">请求需要模拟的客户端类型.</param>
        /// <returns>包含授权验证码的查询字典.</returns>
        string GenerateAuthorizedQueryStringFirstSign(
            Dictionary<string, string> queryParameters,
            RequestClientType clientType);

        /// <summary>
        /// 获取Cookie字符串.
        /// </summary>
        /// <returns>Cookie字符串.</returns>
        string GetCookieString();

        /// <summary>
        /// 获取当前登录用户的访问令牌.
        /// </summary>
        /// <returns>账户授权的令牌.</returns>
        Task<string> GetTokenAsync();

        /// <summary>
        /// 用户登录.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        Task<bool> TrySignInAsync();

        /// <summary>
        /// 用户退出.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        Task SignOutAsync();

        /// <summary>
        /// 当前的访问令牌是否有效.
        /// </summary>
        /// <param name="isNetworkVerify">是否需要联网验证.</param>
        /// <returns>有效为<c>true</c>，无效为<c>false</c>.</returns>
        Task<bool> IsTokenValidAsync(bool isNetworkVerify = false);
    }
}
