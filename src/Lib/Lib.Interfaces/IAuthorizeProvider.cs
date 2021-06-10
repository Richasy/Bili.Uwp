// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Richasy.Bili.Models.App.Args;
using Richasy.Bili.Models.Enums;

namespace Richasy.Bili.Lib.Interfaces
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
        /// 获取包含授权码的查询字符串.
        /// </summary>
        /// <param name="queryParameters">请求所需的查询参数.</param>
        /// <param name="clientType">请求需要模拟的客户端类型.</param>
        /// <returns>包含授权验证的查询字符串.</returns>
        Task<string> GenerateAuthorizedQueryStringAsync(Dictionary<string, string> queryParameters, RequestClientType clientType);

        /// <summary>
        /// 获取包含授权码的查询字典.
        /// </summary>
        /// <param name="queryParameters">请求所需的查询参数.</param>
        /// <param name="clientType">请求需要模拟的客户端类型.</param>
        /// <returns>包含授权验证码的查询字典.</returns>
        Task<Dictionary<string, string>> GenerateAuthorizedQueryDictionaryAsync(Dictionary<string, string> queryParameters, RequestClientType clientType);

        /// <summary>
        /// 获取当前登录用户的访问令牌.
        /// </summary>
        /// <param name="silentOnly">是否需要静默请求.</param>
        /// <returns>账户授权的令牌.</returns>
        Task<string> GetTokenAsync(bool silentOnly = false);

        /// <summary>
        /// 用户登录.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        Task SignInAsync();

        /// <summary>
        /// 用户退出.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        Task SignOutAsync();

        /// <summary>
        /// 尝试静默登录.
        /// </summary>
        /// <returns>静默登录的结果.</returns>
        Task<bool> TrySilentSignInAsync();
    }
}
