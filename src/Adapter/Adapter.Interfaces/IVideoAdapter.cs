// Copyright (c) Richasy. All rights reserved.

using Bili.Models.BiliBili;
using Bili.Models.Data.Player;

namespace Bili.Adapter.Interfaces
{
    /// <summary>
    /// 视频数据适配器接口.
    /// </summary>
    public interface IVideoAdapter
    {
        /// <summary>
        /// 将推荐视频卡片 <see cref="RecommendCard"/> 转换为视频信息.
        /// </summary>
        /// <param name="videoCard">推荐视频卡片.</param>
        /// <returns><see cref="VideoInformation"/>.</returns>
        VideoInformation ConvertToVideoInformation(RecommendCard videoCard);
    }
}
