// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bili.Models.BiliBili;

namespace Bili.Controller.Uwp
{
    /// <summary>
    /// 控制器的账户部分.
    /// </summary>
    public partial class BiliController
    {
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
                var list = await _favoriteProvider.GetFavoriteListAsync(userId, videoId);
                return list.List;
            }
            catch (Exception ex)
            {
                _loggerModule.LogError(ex);
                throw;
            }
        }
    }
}
