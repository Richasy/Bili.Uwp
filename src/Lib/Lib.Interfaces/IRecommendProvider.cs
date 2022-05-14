// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using System.Threading.Tasks;
using Bili.Models.Data.Video;

namespace Bili.Lib.Interfaces
{
    /// <summary>
    /// 首页推荐视频处理程序.
    /// </summary>
    public interface IRecommendProvider : IResetProvider
    {
        /// <summary>
        /// 请求推荐视频列表.
        /// </summary>
        /// <param name="offsetIdx">偏移标识符.</param>
        /// <returns>推荐视频或番剧的列表.</returns>
        /// <remarks>
        /// 视频推荐请求返回的是一个信息流，每请求一次返回一个固定数量的集合。
        /// 除第一次请求外，后一次请求依赖前一次请求的偏移值，以避免出现重复的视频.
        /// 视频推荐服务内置偏移值管理，如果要重置偏移值（比如需要刷新），可以调用 <c>Reset</c> 方法重置.
        /// </remarks>
        Task<IEnumerable<IVideoBase>> RequestRecommendVideosAsync();
    }
}
