// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using System.Threading.Tasks;
using Bili.Models.Data.Article;
using Bili.Models.Data.Pgc;
using Bili.Models.Data.Video;

namespace Bili.Lib.Interfaces
{
    /// <summary>
    /// 收藏夹关联服务提供工具的接口定义.
    /// </summary>
    public interface IFavoriteProvider
    {
        /// <summary>
        /// 获取用户的收藏夹列表（限于播放视频时）.
        /// </summary>
        /// <param name="userId">用户Id.</param>
        /// <param name="videoId">待查询的视频Id.</param>
        /// <returns>收藏夹列表以及包含该视频的收藏夹 Id 集合.</returns>
        Task<(VideoFavoriteSet, IEnumerable<string>)> GetCurrentPlayerFavoriteListAsync(string userId, string videoId);

        /// <summary>
        /// 获取用户的视频收藏分组列表.
        /// </summary>
        /// <param name="userId">用户Id.</param>
        /// <returns>响应结果.</returns>
        Task<VideoFavoriteView> GetVideoFavoriteViewAsync(string userId);

        /// <summary>
        /// 获取视频收藏夹详情.
        /// </summary>
        /// <param name="folderId">收藏夹Id.</param>
        /// <returns>视频收藏夹响应.</returns>
        Task<VideoFavoriteFolderDetail> GetVideoFavoriteFolderDetailAsync(string folderId);

        /// <summary>
        /// 获取指定用户的视频收藏夹分组详情.
        /// </summary>
        /// <param name="userId">用户Id.</param>
        /// <param name="isCreated">是否是该用户创建的收藏夹.</param>
        /// <returns>视频收藏夹列表响应.</returns>
        /// <remarks>
        /// 对于视频收藏夹分组来说，只有两种，一种是用户创建的，一种是用户收集的.
        /// </remarks>
        Task<VideoFavoriteSet> GetVideoFavoriteFolderListAsync(string userId, bool isCreated);

        /// <summary>
        /// 获取追番列表.
        /// </summary>
        /// <param name="status">状态.</param>
        /// <returns>追番列表响应.</returns>
        Task<SeasonSet> GetFavoriteAnimeListAsync(int status);

        /// <summary>
        /// 获取追剧列表.
        /// </summary>
        /// <param name="status">状态.</param>
        /// <returns>追剧列表响应.</returns>
        Task<SeasonSet> GetFavoriteCinemaListAsync(int status);

        /// <summary>
        /// 获取收藏文章列表.
        /// </summary>
        /// <returns>收藏文章列表响应.</returns>
        Task<ArticleSet> GetFavortieArticleListAsync();

        /// <summary>
        /// 更新收藏的PGC内容状态.
        /// </summary>
        /// <param name="seasonId">PGC剧集Id.</param>
        /// <param name="status">状态代码.</param>
        /// <returns>是否更新成功.</returns>
        Task<bool> UpdateFavoritePgcStatusAsync(string seasonId, int status);

        /// <summary>
        /// 取消关注收藏夹.
        /// </summary>
        /// <param name="favoriteId">收藏夹Id.</param>
        /// <param name="isMe">是否是登录用户创建的收藏夹.</param>
        /// <returns>结果.</returns>
        Task<bool> RemoveFavoriteFolderAsync(string favoriteId, bool isMe);

        /// <summary>
        /// 取消视频收藏.
        /// </summary>
        /// <param name="favoriteId">收藏夹Id.</param>
        /// <param name="videoId">视频Id.</param>
        /// <returns>结果.</returns>
        Task<bool> RemoveFavoriteVideoAsync(string favoriteId, string videoId);

        /// <summary>
        /// 取消番剧/影视收藏.
        /// </summary>
        /// <param name="seasonId">剧集Id.</param>
        /// <returns>结果.</returns>
        Task<bool> RemoveFavoritePgcAsync(string seasonId);

        /// <summary>
        /// 取消文章收藏.
        /// </summary>
        /// <param name="articleId">文章Id.</param>
        /// <returns>结果.</returns>
        Task<bool> RemoveFavoriteArticleAsync(string articleId);

        /// <summary>
        /// 重置视频收藏夹详情的请求状态.
        /// </summary>
        void ResetVideoFolderDetailStatus();

        /// <summary>
        /// 重置动漫收藏请求状态.
        /// </summary>
        void ResetAnimeStatus();

        /// <summary>
        /// 重置影视收藏请求状态.
        /// </summary>
        void ResetCinemaStatus();

        /// <summary>
        /// 重置文章收藏请求状态.
        /// </summary>
        void ResetArticleStatus();
    }
}
