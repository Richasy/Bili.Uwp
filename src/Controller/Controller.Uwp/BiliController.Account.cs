// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bili.Models.App.Args;
using Bili.Models.BiliBili;
using Bili.Models.Data.Community;
using Bili.Models.Data.User;
using Bili.Models.Enums.App;
using Bilibili.App.Interfaces.V1;

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
        /// 获取历史记录标签页.
        /// </summary>
        /// <returns>历史记录标签页.</returns>
        [Obsolete]
        public async Task<List<CursorTab>> GetHistoryTabsAsync()
        {
            ThrowWhenNetworkUnavaliable();
            var data = await _accountProvider.GetMyHistoryTabsAsync();
            var tabs = data.Tab.ToList();
            tabs.RemoveAll(p => p.Name == "goods" || p.Name == "show");
            return tabs;
        }

        /// <summary>
        /// 请求历史记录（目前仅请求视频）.
        /// </summary>
        /// <param name="cursor">偏移值.</param>
        /// <returns><see cref="Task"/>.</returns>
        public async Task RequestHistorySetAsync(Cursor cursor)
        {
            ThrowWhenNetworkUnavaliable();
            try
            {
                var data = await _accountProvider.GetMyHistorySetAsync("archive", cursor);
                var args = new HistoryVideoIterationEventArgs(data);
                HistoryVideoIteration?.Invoke(this, args);
            }
            catch (Exception ex)
            {
                _loggerModule.LogError(ex, cursor.Max > 0);
                if (cursor.Max == 0)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// 删除历史记录条目.
        /// </summary>
        /// <param name="historyId">历史记录Id.</param>
        /// <returns>是否删除成功.</returns>
        public async Task<bool> RemoveHistoryItemAsync(long historyId)
        {
            ThrowWhenNetworkUnavaliable();
            try
            {
                return await _accountProvider.RemoveHistoryItemAsync("archive", historyId);
            }
            catch (Exception ex)
            {
                _loggerModule.LogError(ex, true);
                return false;
            }
        }

        /// <summary>
        /// 清空历史记录.
        /// </summary>
        /// <returns>是否清除成功.</returns>
        public async Task<bool> ClearHistoryAsync()
        {
            ThrowWhenNetworkUnavaliable();
            try
            {
                return await _accountProvider.ClearHistoryAsync("archive");
            }
            catch (Exception ex)
            {
                _loggerModule.LogError(ex, true);
                return false;
            }
        }

        /// <summary>
        /// 请求用户空间数据.
        /// </summary>
        /// <param name="userId">用户Id.</param>
        /// <returns><see cref="Task"/>.</returns>
        public async Task<UserSpaceInformation> RequestUserSpaceInformationAsync(int userId)
        {
            ThrowWhenNetworkUnavaliable();

            try
            {
                var data = await _accountProvider.GetUserSpaceInformationAsync(userId);
                if (data.VideoSet != null)
                {
                    var args = new UserSpaceVideoIterationEventArgs(data.VideoSet, userId);
                    UserSpaceVideoIteration?.Invoke(this, args);
                }

                return data.User;
            }
            catch (Exception ex)
            {
                _loggerModule.LogError(ex);
                throw;
            }
        }

        /// <summary>
        /// 请求用户空间视频集.
        /// </summary>
        /// <param name="userId">用户Id.</param>
        /// <param name="offsetId">偏移Id.</param>
        /// <returns><see cref="Task"/>.</returns>
        public async Task RequestUserSpaceVideoSetAsync(int userId, string offsetId)
        {
            ThrowWhenNetworkUnavaliable();

            try
            {
                var data = await _accountProvider.GetUserSpaceVideoSetAsync(userId, offsetId);
                var args = new UserSpaceVideoIterationEventArgs(data, userId);
                UserSpaceVideoIteration?.Invoke(this, args);
            }
            catch (Exception ex)
            {
                _loggerModule.LogError(ex);
                throw;
            }
        }

        /// <summary>
        /// 请求搜索用户空间视频.
        /// </summary>
        /// <param name="userId">用户Id.</param>
        /// <param name="keyword">关键词.</param>
        /// <param name="pageNumber">页码.</param>
        /// <returns><see cref="Task"/>.</returns>
        public async Task RequestSearchUserVideoAsync(int userId, string keyword, int pageNumber)
        {
            ThrowWhenNetworkUnavaliable();

            try
            {
                if (pageNumber < 1)
                {
                    pageNumber = 1;
                }

                var data = await _accountProvider.SearchUserSpaceVideoAsync(userId, keyword, pageNumber);
                var args = new UserSpaceSearchVideoIterationEventArgs(data, userId);
                UserSpaceSearchVideoIteration?.Invoke(this, args);
            }
            catch (Exception ex)
            {
                _loggerModule.LogError(ex);
                throw;
            }
        }

        /// <summary>
        /// 修改用户关系(关注/取消关注).
        /// </summary>
        /// <param name="userId">用户Id.</param>
        /// <param name="isFollow">是否关注.</param>
        /// <returns>结果.</returns>
        public async Task<bool> ModifyUserRelationAsync(int userId, bool isFollow)
        {
            ThrowWhenNetworkUnavaliable();

            try
            {
                var result = await _accountProvider.ModifyUserRelationAsync(userId, isFollow);
                return result;
            }
            catch (Exception ex)
            {
                _loggerModule.LogError(ex);
                return false;
            }
        }

        /// <summary>
        /// 请求新的粉丝列表.
        /// </summary>
        /// <param name="userId">需要查询的用户Id.</param>
        /// <param name="pageNumber">页码.</param>
        /// <returns><see cref="Task"/>.</returns>
        public async Task RequestUserFollowersAsync(int userId, int pageNumber)
        {
            ThrowWhenNetworkUnavaliable();

            try
            {
                var result = await _accountProvider.GetFansAsync(userId, pageNumber);
                var args = new RelatedUserIterationEventArgs(result, pageNumber, userId);
                FansIteration?.Invoke(this, args);
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
        /// 请求新的关注列表.
        /// </summary>
        /// <param name="userId">需要查询的用户Id.</param>
        /// <param name="pageNumber">页码.</param>
        /// <returns><see cref="Task"/>.</returns>
        public async Task RequestUserFollowsAsync(int userId, int pageNumber)
        {
            ThrowWhenNetworkUnavaliable();

            try
            {
                var result = await _accountProvider.GetFollowsAsync(userId, pageNumber);
                var args = new RelatedUserIterationEventArgs(result, pageNumber, userId);
                FollowsIteration?.Invoke(this, args);
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
                var args = new RelatedUserIterationEventArgs(result, pageNumber);
                FollowsIteration?.Invoke(this, args);
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
        /// 获取与用户间的关系.
        /// </summary>
        /// <param name="targetUserId">目标用户Id.</param>
        /// <returns>用户关系响应.</returns>
        public Task<UserRelationResponse> GetRelationAsync(int targetUserId)
            => _accountProvider.GetRelationAsync(targetUserId);

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
