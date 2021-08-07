// Copyright (c) Richasy. All rights reserved.

namespace Richasy.Bili.Models.Enums.Bili
{
    /// <summary>
    /// 收藏结果.
    /// </summary>
    public enum FavoriteResult
    {
        /// <summary>
        /// 操作成功.
        /// </summary>
        Success = 0,

        /// <summary>
        /// 需要登录.
        /// </summary>
        NeedLogin = -101,

        /// <summary>
        /// 请求失败.
        /// </summary>
        RequestError = -400,

        /// <summary>
        /// 访问权限不足.
        /// </summary>
        InsufficientAccess = -403,

        /// <summary>
        /// 未找到指定的收藏视频.
        /// </summary>
        NotFound = 10003,

        /// <summary>
        /// 已经收藏过了.
        /// </summary>
        AlreadyFavorite = 11201,

        /// <summary>
        /// 已经取消收藏了.
        /// </summary>
        AlreadyUnfavorite = 11202,

        /// <summary>
        /// 达到收藏数量的上限.
        /// </summary>
        UpperLimit = 11203,
    }
}
