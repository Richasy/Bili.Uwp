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
        /// 获取我的基本数据.
        /// </summary>
        /// <returns>个人数据.</returns>
        Task<Mine> GetMyDataAsync();

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
        /// 搜索用户空间的视频.
        /// </summary>
        /// <param name="userId">用户Id.</param>
        /// <param name="keyword">关键词.</param>
        /// <param name="pageNumber">页码.</param>
        /// <param name="pageSize">每页容量.</param>
        /// <returns><see cref="SearchArchiveReply"/>.</returns>
        Task<SearchArchiveReply> SearchUserSpaceVideoAsync(int userId, string keyword, int pageNumber, int pageSize = 20);

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

        /// <summary>
        /// 删除指定的历史记录条目.
        /// </summary>
        /// <param name="tabSign">标签信息.</param>
        /// <param name="itemId">条目Id.</param>
        /// <returns>删除是否成功.</returns>
        Task<bool> RemoveHistoryItemAsync(string tabSign, long itemId);

        /// <summary>
        /// 清空历史记录.
        /// </summary>
        /// <param name="tabSign">标签信息.</param>
        /// <returns>清空是否成功.</returns>
        Task<bool> ClearHistoryAsync(string tabSign);

        /// <summary>
        /// 获取指定用户的粉丝列表.
        /// </summary>
        /// <param name="userId">指定用户的用户Id.</param>
        /// <param name="page">页码 (每页上限50个).</param>
        /// <returns>粉丝响应结果.</returns>
        Task<RelatedUserResponse> GetFansAsync(int userId, int page);

        /// <summary>
        /// 获取指定用户的关注列表.
        /// </summary>
        /// <param name="userId">指定用户的用户Id.</param>
        /// <param name="page">页码（每页上限50个）.</param>
        /// <returns>关注列表.</returns>
        Task<RelatedUserResponse> GetFollowsAsync(int userId, int page);

        /// <summary>
        /// 获取稍后再看列表.
        /// </summary>
        /// <param name="page">页码.</param>
        /// <returns>稍后再看视频列表.</returns>
        Task<ViewLaterResponse> GetViewLaterListAsync(int page);

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
        Task<bool> AddVideoToViewLaterAsync(int videoId);

        /// <summary>
        /// 将视频从稍后再看中移除.
        /// </summary>
        /// <param name="videoIds">需要移除的视频Id列表.</param>
        /// <returns>移除结果.</returns>
        Task<bool> RemoveVideoFromViewLaterAsync(params int[] videoIds);

        /// <summary>
        /// 获取用户的收藏夹列表（限于播放视频时）.
        /// </summary>
        /// <param name="userId">用户Id.</param>
        /// <param name="videoId">待查询的视频Id.</param>
        /// <returns><see cref="FavoriteListResponse"/>.</returns>
        Task<FavoriteListResponse> GetFavoriteListAsync(int userId, int videoId = 0);

        /// <summary>
        /// 获取用户的视频收藏概览.
        /// </summary>
        /// <param name="userId">用户Id.</param>
        /// <returns>响应结果.</returns>
        Task<VideoFavoriteGalleryResponse> GetFavoriteVideoGalleryAsync(int userId);

        /// <summary>
        /// 获取收藏夹视频列表.
        /// </summary>
        /// <param name="favoriteId">收藏夹Id.</param>
        /// <param name="pageNumber">页码.</param>
        /// <returns>视频收藏夹响应.</returns>
        Task<VideoFavoriteListResponse> GetFavoriteVideoListAsync(int favoriteId, int pageNumber);

        /// <summary>
        /// 获取视频收藏夹列表.
        /// </summary>
        /// <param name="userId">用户Id.</param>
        /// <param name="pageNumber">页码.</param>
        /// <param name="isCreated">是否是该用户创建的收藏夹.</param>
        /// <returns>视频收藏夹列表响应.</returns>
        Task<FavoriteMediaList> GetFavoriteFolderListAsync(int userId, int pageNumber, bool isCreated);

        /// <summary>
        /// 获取追番列表.
        /// </summary>
        /// <param name="pageNumber">页码.</param>
        /// <param name="status">状态.</param>
        /// <returns>追番列表响应.</returns>
        Task<PgcFavoriteListResponse> GetFavoriteAnimeListAsync(int pageNumber, int status);

        /// <summary>
        /// 获取追剧列表.
        /// </summary>
        /// <param name="pageNumber">页码.</param>
        /// <param name="status">状态.</param>
        /// <returns>追剧列表响应.</returns>
        Task<PgcFavoriteListResponse> GetFavoriteCinemaListAsync(int pageNumber, int status);

        /// <summary>
        /// 获取收藏文章列表.
        /// </summary>
        /// <param name="pageNumber">页码.</param>
        /// <returns>收藏文章列表响应.</returns>
        Task<ArticleFavoriteListResponse> GetFavortieArticleListAsync(int pageNumber);

        /// <summary>
        /// 更新收藏的PGC内容状态.
        /// </summary>
        /// <param name="seasonId">PGC剧集Id.</param>
        /// <param name="status">状态代码.</param>
        /// <returns>是否更新成功.</returns>
        Task<bool> UpdateFavoritePgcStatusAsync(int seasonId, int status);

        /// <summary>
        /// 取消关注收藏夹.
        /// </summary>
        /// <param name="favoriteId">收藏夹Id.</param>
        /// <param name="isMe">是否是登录用户创建的收藏夹.</param>
        /// <returns>结果.</returns>
        Task<bool> RemoveFavoriteFolderAsync(int favoriteId, bool isMe);

        /// <summary>
        /// 取消视频收藏.
        /// </summary>
        /// <param name="favoriteId">收藏夹Id.</param>
        /// <param name="videoId">视频Id.</param>
        /// <returns>结果.</returns>
        Task<bool> RemoveFavoriteVideoAsync(int favoriteId, int videoId);

        /// <summary>
        /// 取消番剧/影视收藏.
        /// </summary>
        /// <param name="seasonId">剧集Id.</param>
        /// <returns>结果.</returns>
        Task<bool> RemoveFavoritePgcAsync(int seasonId);

        /// <summary>
        /// 取消文章收藏.
        /// </summary>
        /// <param name="articleId">文章Id.</param>
        /// <returns>结果.</returns>
        Task<bool> RemoveFavoriteArticleAsync(int articleId);

        /// <summary>
        /// 获取未读消息.
        /// </summary>
        /// <returns>未读消息.</returns>
        Task<UnreadMessage> GetUnreadMessageAsync();

        /// <summary>
        /// 获取点赞消息列表.
        /// </summary>
        /// <param name="id">偏移值Id.</param>
        /// <param name="likeTime">点赞的时间，偏移的标识符.</param>
        /// <returns>点赞消息响应结果.</returns>
        Task<LikeMessageResponse> GetLikeMessagesAsync(long id, long likeTime);

        /// <summary>
        /// 获取@我的消息列表.
        /// </summary>
        /// <param name="id">偏移值Id.</param>
        /// <param name="atTime">At的时间，偏移的标识符.</param>
        /// <returns>@我的消息响应结果.</returns>
        Task<AtMessageResponse> GetAtMessagesAsync(long id, long atTime);

        /// <summary>
        /// 获取回复我的消息列表.
        /// </summary>
        /// <param name="id">偏移值Id.</param>
        /// <param name="replyTime">回复的时间，偏移的标识符.</param>
        /// <returns>回复我的消息响应结果.</returns>
        Task<ReplyMessageResponse> GetReplyMessagesAsync(long id, long replyTime);

        /// <summary>
        /// 查询我与某用户之间的关系（是否关注）.
        /// </summary>
        /// <param name="targetUserId">目标用户Id.</param>
        /// <returns>是否关注.</returns>
        Task<UserRelationResponse> GetRelationAsync(int targetUserId);
    }
}
