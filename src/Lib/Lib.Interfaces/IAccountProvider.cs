// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using System.Threading.Tasks;
using Bili.Models.BiliBili;
using Bili.Models.Data.Community;
using Bili.Models.Data.User;
using Bili.Models.Data.Video;
using Bili.Models.Enums.App;
using Bili.Models.Enums.Bili;
using Bili.Models.Enums.Community;

namespace Bili.Lib.Interfaces
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
        Task<AccountInformation> GetMyInformationAsync();

        /// <summary>
        /// 获取我的基本数据.
        /// </summary>
        /// <returns>个人数据.</returns>
        Task<UserCommunityInformation> GetMyCommunityInformationAsync();

        /// <summary>
        /// 获取用户主页信息.
        /// </summary>
        /// <param name="userId">用户Id.</param>
        /// <returns><see cref="UserSpaceResponse"/>.</returns>
        Task<UserSpaceView> GetUserSpaceInformationAsync(string userId);

        /// <summary>
        /// 获取用户空间的视频集.
        /// </summary>
        /// <param name="userId">用户Id.</param>
        /// <returns>视频集.</returns>
        Task<VideoSet> GetUserSpaceVideoSetAsync(string userId);

        /// <summary>
        /// 搜索用户空间的视频.
        /// </summary>
        /// <param name="userId">用户Id.</param>
        /// <param name="keyword">关键词.</param>
        /// <returns>视频集.</returns>
        Task<VideoSet> SearchUserSpaceVideoAsync(string userId, string keyword);

        /// <summary>
        /// 关注/取消关注用户.
        /// </summary>
        /// <param name="userId">用户Id.</param>
        /// <param name="isFollow">是否关注.</param>
        /// <returns>关注是否成功.</returns>
        Task<bool> ModifyUserRelationAsync(string userId, bool isFollow);

        /// <summary>
        /// 获取我的历史记录信息.
        /// </summary>
        /// <param name="tabSign">标签信息，默认是 <c>archive</c>，表示视频.</param>
        /// <returns>历史记录.</returns>
        Task<VideoHistoryView> GetMyHistorySetAsync(string tabSign = "archive");

        /// <summary>
        /// 删除指定的历史记录条目.
        /// </summary>
        /// <param name="itemId">条目Id.</param>
        /// <param name="tabSign">标签信息，默认是 <c>archive</c>，表示视频..</param>
        /// <returns>删除是否成功.</returns>
        Task<bool> RemoveHistoryItemAsync(string itemId, string tabSign = "archive");

        /// <summary>
        /// 清空历史记录.
        /// </summary>
        /// <param name="tabSign">标签信息，默认是 <c>archive</c>，表示视频..</param>
        /// <returns>清空是否成功.</returns>
        Task<bool> ClearHistoryAsync(string tabSign = "archive");

        /// <summary>
        /// 获取指定用户的粉丝或关注列表.
        /// </summary>
        /// <param name="userId">用户 Id.</param>
        /// <param name="type">关系类型.</param>
        /// <returns>粉丝或关注列表.</returns>
        Task<RelationView> GetUserFansOrFollowsAsync(string userId, RelationType type);

        /// <summary>
        /// 获取我的关注分组.
        /// </summary>
        /// <returns>分组列表.</returns>
        Task<IEnumerable<FollowGroup>> GetMyFollowingGroupsAsync();

        /// <summary>
        /// 获取我的关注分组详情.
        /// </summary>
        /// <param name="tagId">分组Id.</param>
        /// <returns>用户列表.</returns>
        Task<IEnumerable<AccountInformation>> GetMyFollowingGroupDetailAsync(string tagId);

        /// <summary>
        /// 获取稍后再看列表.
        /// </summary>
        /// <returns>稍后再看视频列表.</returns>
        Task<VideoSet> GetViewLaterListAsync();

        /// <summary>
        /// 清空稍后再看列表.
        /// </summary>
        /// <returns>清除结果.</returns>
        Task<bool> ClearViewLaterAsync();

        /// <summary>
        /// 将视频添加到稍后再看.
        /// </summary>
        /// <param name="videoId">视频Id.</param>
        /// <returns>添加的结果.</returns>
        Task<bool> AddVideoToViewLaterAsync(string videoId);

        /// <summary>
        /// 将视频从稍后再看中移除.
        /// </summary>
        /// <param name="videoIds">需要移除的视频Id列表.</param>
        /// <returns>移除结果.</returns>
        Task<bool> RemoveVideoFromViewLaterAsync(params string[] videoIds);

        /// <summary>
        /// 获取未读消息.
        /// </summary>
        /// <returns>未读消息.</returns>
        Task<UnreadInformation> GetUnreadMessageAsync();

        /// <summary>
        /// 获取指定类型的消息列表.
        /// </summary>
        /// <param name="type">消息类型.</param>
        /// <returns>指定类型的消息响应结果.</returns>
        Task<MessageView> GetMyMessagesAsync(MessageType type);

        /// <summary>
        /// 查询我与某用户之间的关系（是否关注）.
        /// </summary>
        /// <param name="targetUserId">目标用户Id.</param>
        /// <returns>是否关注.</returns>
        Task<UserRelationStatus> GetRelationAsync(string targetUserId);

        /// <summary>
        /// 清除消息的请求状态.
        /// </summary>
        void ClearMessageStatus();

        /// <summary>
        /// 重置稍后再看的请求状态.
        /// </summary>
        void ResetViewLaterStatus();

        /// <summary>
        /// 重置历史记录请求状态.
        /// </summary>
        void ResetHistoryStatus();

        /// <summary>
        /// 重置关系请求状态.
        /// </summary>
        /// <param name="type">关系类型.</param>
        void ResetRelationStatus(RelationType type);

        /// <summary>
        /// 重置空间视频请求状态.
        /// </summary>
        void ResetSpaceVideoStatus();

        /// <summary>
        /// 重置空间搜索请求状态.
        /// </summary>
        void ResetSpaceSearchStatus();

        /// <summary>
        /// 重置我的关注请求状态.
        /// </summary>
        /// <param name="groupId">分组 Id.</param>
        void ResetMyFollowStatus(string groupId);

        /// <summary>
        /// 清除我的关注请求状态.
        /// </summary>
        void ClearMyFollowStatus();
    }
}
