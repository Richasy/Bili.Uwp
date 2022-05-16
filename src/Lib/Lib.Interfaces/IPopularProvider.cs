// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using System.Threading.Tasks;
using Bili.Models.Data.Video;

namespace Bili.Lib.Interfaces
{
    /// <summary>
    /// 热门数据处理.
    /// </summary>
    public interface IPopularProvider : IResetProvider
    {
        /// <summary>
        /// 获取热门详情.
        /// </summary>
        /// <returns>热门视频信息.</returns>
        Task<IEnumerable<VideoInformation>> RequestPopularVideosAsync();
    }
}
