// Copyright (c) Richasy. All rights reserved.

using System.Threading.Tasks;
using Richasy.Bili.Models.BiliBili;

namespace Richasy.Bili.Lib.Interfaces
{
    /// <summary>
    /// 提供已登录账户相关的操作和功能.
    /// </summary>
    public interface IAccountProvider
    {
        /// <summary>
        /// 已登录的用户Id.
        /// </summary>
        int UserId { get; }

        /// <summary>
        /// 获取已登录用户的个人资料.
        /// </summary>
        /// <returns>个人资料.</returns>
        Task<MyInfo> GetMyInformationAsync();
    }
}
