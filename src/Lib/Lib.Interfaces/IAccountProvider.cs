// Copyright (c) Richasy. All rights reserved.

using System.Threading.Tasks;
using Bilibili.App.Interfaces.V1;
using Richasy.Bili.Models.BiliBili;

namespace Richasy.Bili.Lib.Interfaces
{
    /// <summary>
    /// 提供账户相关的操作和功能.
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

        /// <summary>
        /// 获取用户主页信息.
        /// </summary>
        /// <param name="userId">用户Id.</param>
        /// <returns><see cref="UserSpaceResponse"/>.</returns>
        Task<UserSpaceResponse> GetUserSpaceInformationAsync(int userId);

        /// <summary>
        /// 获取用户空间的视频集.
        /// </summary>
        /// <param name="userId">用户Id.</param>
        /// <param name="offsetId">偏移值Id.</param>
        /// <returns>视频集.</returns>
        Task<UserSpaceVideoSet> GetUserSpaceVideoSetAsync(int userId, string offsetId);

        /// <summary>
        /// 关注/取消关注用户.
        /// </summary>
        /// <param name="userId">用户Id.</param>
        /// <param name="isFollow">是否关注.</param>
        /// <returns>关注是否成功.</returns>
        Task<bool> ModifyUserRelationAsync(int userId, bool isFollow);

        /// <summary>
        /// 获取我的历史记录标签页信息.
        /// </summary>
        /// <returns><see cref="HistoryTabReply"/>.</returns>
        Task<HistoryTabReply> GetMyHistoryTabsAsync();

        /// <summary>
        /// 获取我的历史记录信息.
        /// </summary>
        /// <param name="tabSign">标签信息.</param>
        /// <param name="cursor">偏移值.</param>
        /// <returns><see cref="CursorV2Reply"/>.</returns>
        Task<CursorV2Reply> GetMyHistorySetAsync(string tabSign, Cursor cursor);
    }
}
