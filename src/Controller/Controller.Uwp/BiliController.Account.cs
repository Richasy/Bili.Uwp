// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Bilibili.App.Interfaces.V1;
using Richasy.Bili.Models.App.Args;
using Richasy.Bili.Models.BiliBili;
using Richasy.Bili.Models.Enums.App;

namespace Richasy.Bili.Controller.Uwp
{
    /// <summary>
    /// 控制器的账户部分.
    /// </summary>
    public partial class BiliController
    {
        private MyInfo _myInfo;

        /// <summary>
        /// 已登录用户的账户数据.
        /// </summary>
        public MyInfo MyInfo
        {
            get => _myInfo;
            set
            {
                _myInfo = value;
                AccountChanged?.Invoke(this, _myInfo);
            }
        }

        /// <summary>
        /// 获取我的资料.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        public async Task RequestMyProfileAsync()
        {
            if (await _authorizeProvider.IsTokenValidAsync() && IsNetworkAvailable)
            {
                try
                {
                    var profile = await _accountProvider.GetMyInformationAsync();
                    this.MyInfo = profile;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
        }

        /// <summary>
        /// 获取我的用户数据.
        /// </summary>
        /// <returns>用户数据.</returns>
        public async Task<Mine> GetMyDataAsync()
        {
            ThrowWhenNetworkUnavaliable();
            var data = await _accountProvider.GetMyDataAsync();
            return data;
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
            catch (Exception)
            {
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
            catch (Exception)
            {
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
            catch (Exception)
            {
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
            var data = await _accountProvider.GetUserSpaceInformationAsync(userId);
            if (data.VideoSet != null)
            {
                var args = new UserSpaceVideoIterationEventArgs(data.VideoSet, userId);
                UserSpaceVideoIteration?.Invoke(this, args);
            }

            return data.User;
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
            var data = await _accountProvider.GetUserSpaceVideoSetAsync(userId, offsetId);
            var args = new UserSpaceVideoIterationEventArgs(data, userId);
            UserSpaceVideoIteration?.Invoke(this, args);
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
            var result = await _accountProvider.ModifyUserRelationAsync(userId, isFollow);
            return result;
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
            catch (Exception)
            {
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
            catch (Exception)
            {
                if (pageNumber <= 1)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// 请求稍后再看视频列表.
        /// </summary>
        /// <param name="pageNumber">页码.</param>
        /// <returns><see cref="Task"/>.</returns>
        public async Task RequestViewLaterListAsync(int pageNumber)
        {
            ThrowWhenNetworkUnavaliable();

            try
            {
                var result = await _accountProvider.GetViewLaterListAsync(pageNumber);
                var args = new ViewLaterVideoIterationEventArgs(result, pageNumber);
                ViewLaterVideoIteration?.Invoke(this, args);
            }
            catch (Exception)
            {
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
            catch (Exception)
            {
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
                var result = await _accountProvider.AddVideoToViewLaterAsync(videoId);
                return result;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 将视频从稍后再看列表中移除.
        /// </summary>
        /// <param name="videoIds">视频Id列表.</param>
        /// <returns>清除结果.</returns>
        public async Task<bool> RemoveVideoFromViewLaterAsync(params int[] videoIds)
        {
            ThrowWhenNetworkUnavaliable();

            try
            {
                var result = await _accountProvider.RemoveVideoFromViewLaterAsync(videoIds);
                return result;
            }
            catch (Exception)
            {
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

            var list = await _accountProvider.GetFavoriteListAsync(userId, videoId);
            return list.List;
        }

        /// <summary>
        /// 获取视频收藏夹概览信息.
        /// </summary>
        /// <param name="userId">用户Id.</param>
        /// <returns>概览信息.</returns>
        public async Task<VideoFavoriteGalleryResponse> GetVideoFavoriteGalleryAsync(int userId)
        {
            ThrowWhenNetworkUnavaliable();

            var response = await _accountProvider.GetFavoriteVideoGalleryAsync(userId);
            return response;
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
            catch (Exception)
            {
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
        /// <returns><see cref="Task"/>.</returns>
        public async Task RequestPgcFavoriteListAsync(int pageNumber, FavoriteType type)
        {
            ThrowWhenNetworkUnavaliable();
            PgcFavoriteListResponse response = null;

            try
            {
                if (type == FavoriteType.Anime)
                {
                    response = await _accountProvider.GetFavoriteAnimeListAsync(pageNumber);
                }
                else if (type == FavoriteType.Cinema)
                {
                    response = await _accountProvider.GetFavoriteCinemaListAsync(pageNumber);
                }

                var args = new FavoritePgcIterationEventArgs(response, pageNumber, type);
                PgcFavoriteIteration?.Invoke(this, args);
            }
            catch (Exception)
            {
                if (pageNumber <= 1)
                {
                    throw;
                }
            }
        }
    }
}
