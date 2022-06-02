// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bili.Models.App.Args;
using Bili.Models.BiliBili;
using Bili.Models.Data.Community;
using Bili.Models.Data.User;
using Bili.Models.Enums.App;

namespace Bili.Controller.Uwp
{
    /// <summary>
    /// 控制器的账户部分.
    /// </summary>
    public partial class BiliController
    {
        private AccountInformation _accountInformation;

        /// <summary>
        /// 已登录用户的账户数据.
        /// </summary>
        public AccountInformation AccountInformation
        {
            get => _accountInformation;
            set
            {
                _accountInformation = value;
                AccountChanged?.Invoke(this, _accountInformation);
            }
        }

        /// <summary>
        /// 获取我的资料.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        public async Task RequestMyProfileAsync()
        {
            if (IsNetworkAvailable && await _authorizeProvider.IsTokenValidAsync())
            {
                try
                {
                    var profile = await _accountProvider.GetMyInformationAsync();
                    AccountInformation = profile;
                }
                catch (Exception ex)
                {
                    _loggerModule.LogError(ex);
                }
            }
        }

        /// <summary>
        /// 获取我的用户数据.
        /// </summary>
        /// <returns>用户数据.</returns>
        public async Task<UserCommunityInformation> GetMyCommunityInformationAsync()
        {
            ThrowWhenNetworkUnavaliable();

            try
            {
                var data = await _accountProvider.GetMyCommunityInformationAsync();
                return data;
            }
            catch (Exception ex)
            {
                _loggerModule.LogError(ex, true);
                throw;
            }
        }

        /// <summary>
        /// 获取我的关注分组.
        /// </summary>
        /// <returns>关注分组.</returns>
        public Task<List<RelatedTag>> GetMyFollowingTagsAsync()
            => _accountProvider.GetMyFollowingTagsAsync();

        /// <summary>
        /// 获取我的关注分组.
        /// </summary>
        /// <param name="tagId">分组Id.</param>
        /// <param name="pageNumber">页码.</param>
        /// <returns>关注分组.</returns>
        public async Task GetMyFollowingTagDetailAsync(int tagId, int pageNumber)
        {
            ThrowWhenNetworkUnavaliable();

            try
            {
                var result = await _accountProvider.GetMyFollowingTagDetailAsync(_accountProvider.UserId, tagId, pageNumber);
            }
            catch (Exception ex)
            {
                _loggerModule.LogError(ex, pageNumber > 1);
                if (pageNumber <= 1)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// 清除稍后再看列表.
        /// </summary>
        /// <returns>清除结果.</returns>
        public async Task<bool> ClearViewLaterAsync()
        {
            ThrowWhenNetworkUnavaliable();

            try
            {
                var result = await _accountProvider.ClearViewLaterAsync();
                return result;
            }
            catch (Exception ex)
            {
                _loggerModule.LogError(ex, true);
                return false;
            }
        }

        /// <summary>
        /// 添加视频至稍后再看列表.
        /// </summary>
        /// <param name="videoId">视频Id.</param>
        /// <returns>清除结果.</returns>
        public async Task<bool> AddVideoToViewLaterAsync(int videoId)
        {
            ThrowWhenNetworkUnavaliable();

            try
            {
                var result = await _accountProvider.AddVideoToViewLaterAsync(videoId.ToString());
                return result;
            }
            catch (Exception ex)
            {
                _loggerModule.LogError(ex, true);
                return false;
            }
        }

        /// <summary>
        /// 获取收藏夹列表.
        /// </summary>
        /// <param name="userId">用户Id.</param>
        /// <param name="videoId">视频Id.</param>
        /// <returns>收藏夹列表.</returns>
        public async Task<List<FavoriteMeta>> GetFavoriteListAsync(int userId, int videoId)
        {
            ThrowWhenNetworkUnavaliable();

            try
            {
                var list = await _accountProvider.GetFavoriteListAsync(userId, videoId);
                return list.List;
            }
            catch (Exception ex)
            {
                _loggerModule.LogError(ex);
                throw;
            }
        }

        /// <summary>
        /// 获取视频收藏夹概览信息.
        /// </summary>
        /// <param name="userId">用户Id.</param>
        /// <returns>概览信息.</returns>
        public async Task<VideoFavoriteGalleryResponse> GetVideoFavoriteGalleryAsync(int userId)
        {
            ThrowWhenNetworkUnavaliable();

            try
            {
                var response = await _accountProvider.GetFavoriteVideoGalleryAsync(userId);
                return response;
            }
            catch (Exception ex)
            {
                _loggerModule.LogError(ex);
                throw;
            }
        }

        /// <summary>
        /// 获取分类下的收藏夹列表.
        /// </summary>
        /// <param name="userId">用户Id.</param>
        /// <param name="pageNumber">页码.</param>
        /// <param name="isCreated">是否是该用户创建的收藏夹.</param>
        /// <returns>列表信息.</returns>
        public async Task<FavoriteMediaList> GetFavoriteFolderListAsync(int userId, int pageNumber, bool isCreated = false)
        {
            ThrowWhenNetworkUnavaliable();

            try
            {
                var response = await _accountProvider.GetFavoriteFolderListAsync(userId, pageNumber, isCreated);
                return response;
            }
            catch (Exception ex)
            {
                _loggerModule.LogError(ex);
                throw;
            }
        }

        /// <summary>
        /// 获取收藏夹的视频列表.
        /// </summary>
        /// <param name="favoriteId">收藏夹Id.</param>
        /// <param name="pageNumber">页码.</param>
        /// <returns>收藏夹视频列表响应.</returns>
        public async Task<VideoFavoriteListResponse> GetVideoFavoriteListAsync(int favoriteId, int pageNumber)
        {
            ThrowWhenNetworkUnavaliable();

            try
            {
                var response = await _accountProvider.GetFavoriteVideoListAsync(favoriteId, pageNumber);
                return response;
            }
            catch (Exception ex)
            {
                _loggerModule.LogError(ex, pageNumber > 1);
                if (pageNumber <= 1)
                {
                    throw;
                }
            }

            return null;
        }

        /// <summary>
        /// 请求PGC收藏夹信息.
        /// </summary>
        /// <param name="pageNumber">页码.</param>
        /// <param name="type">收藏夹类型.</param>
        /// <param name="status">状态.</param>
        /// <returns><see cref="Task"/>.</returns>
        public async Task RequestPgcFavoriteListAsync(int pageNumber, FavoriteType type, int status)
        {
            ThrowWhenNetworkUnavaliable();
            PgcFavoriteListResponse response = null;

            try
            {
                if (type == FavoriteType.Anime)
                {
                    response = await _accountProvider.GetFavoriteAnimeListAsync(pageNumber, status);
                }
                else if (type == FavoriteType.Cinema)
                {
                    response = await _accountProvider.GetFavoriteCinemaListAsync(pageNumber, status);
                }

                var args = new FavoritePgcIterationEventArgs(response, pageNumber, type);
                PgcFavoriteIteration?.Invoke(this, args);
            }
            catch (Exception ex)
            {
                _loggerModule.LogError(ex, pageNumber > 1);
                if (pageNumber <= 1)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// 请求文章收藏夹信息.
        /// </summary>
        /// <param name="pageNumber">页码.</param>
        /// <returns><see cref="Task"/>.</returns>
        public async Task RequestArticleFavoriteListAsync(int pageNumber)
        {
            ThrowWhenNetworkUnavaliable();

            try
            {
                var response = await _accountProvider.GetFavortieArticleListAsync(pageNumber);
                var args = new FavoriteArticleIterationEventArgs(response, pageNumber);
                ArticleFavoriteIteration?.Invoke(this, args);
            }
            catch (Exception ex)
            {
                _loggerModule.LogError(ex, pageNumber > 1);
                if (pageNumber <= 1)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// 移除收藏夹.
        /// </summary>
        /// <param name="favoriteId">收藏夹Id.</param>
        /// <param name="isMe">是否为自己创建的收藏夹.</param>
        /// <returns>结果.</returns>
        public Task<bool> RemoveFavoriteFolderAsync(int favoriteId, bool isMe)
            => _accountProvider.RemoveFavoriteFolderAsync(favoriteId, isMe);

        /// <summary>
        /// 取消收藏视频.
        /// </summary>
        /// <param name="favoriteId">收藏夹Id.</param>
        /// <param name="videoId">视频Id.</param>
        /// <returns>结果.</returns>
        public Task<bool> RemoveFavoriteVideoAsync(int favoriteId, int videoId)
            => _accountProvider.RemoveFavoriteVideoAsync(favoriteId, videoId);

        /// <summary>
        /// 取消收藏番剧/影视.
        /// </summary>
        /// <param name="seasonId">剧集Id.</param>
        /// <returns>结果.</returns>
        public Task<bool> RemoveFavoritePgcAsync(int seasonId)
            => _accountProvider.RemoveFavoritePgcAsync(seasonId);

        /// <summary>
        /// 取消收藏文章.
        /// </summary>
        /// <param name="articleId">文章Id.</param>
        /// <returns>结果.</returns>
        public Task<bool> RemoveFavoriteArticleAsync(int articleId)
            => _accountProvider.RemoveFavoriteArticleAsync(articleId);

        /// <summary>
        /// 更新收藏的PGC内容状态.
        /// </summary>
        /// <param name="seasonId">PGC剧集Id.</param>
        /// <param name="status">状态代码.</param>
        /// <returns>是否更新成功.</returns>
        public Task<bool> UpdateFavoritePgcStatusAsync(int seasonId, int status)
            => _accountProvider.UpdateFavoritePgcStatusAsync(seasonId, status);
    }
}
