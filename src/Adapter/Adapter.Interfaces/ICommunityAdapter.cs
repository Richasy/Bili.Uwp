// Copyright (c) Richasy. All rights reserved.

using Bili.Models.BiliBili;
using Bili.Models.Data.Community;

namespace Bili.Adapter.Interfaces
{
    /// <summary>
    /// 社区数据适配器接口.
    /// </summary>
    public interface ICommunityAdapter
    {
        /// <summary>
        /// 将个人信息 <see cref="Mine"/> 转换为用户社区交互信息.
        /// </summary>
        /// <param name="mine">个人信息.</param>
        /// <returns><see cref="UserCommunityInformation"/>.</returns>
        UserCommunityInformation ConvertToUserCommunityInformation(Mine mine);

        /// <summary>
        /// 将用户空间信息 <see cref="UserSpaceInformation"/> 转换为用户社区交互信息.
        /// </summary>
        /// <param name="spaceInfo">用户空间信息.</param>
        /// <returns><see cref="UserCommunityInformation"/>.</returns>
        UserCommunityInformation ConvertToUserCommunityInformation(UserSpaceInformation spaceInfo);

        /// <summary>
        /// 将推荐卡片 <see cref="RecommendCard"/> 转换为视频交互信息.
        /// </summary>
        /// <param name="videoCard">推荐卡片信息.</param>
        /// <returns><see cref="VideoCommunityInformation"/>.</returns>
        /// <remarks>
        /// 这里的转换只是将 <see cref="RecommendCard"/> 中关于社区交互的信息提取出来，其它的视频信息交由 <see cref="IVideoAdapter"/> 来处理.
        /// </remarks>
        VideoCommunityInformation ConvertToVideoCommunityInformation(RecommendCard videoCard);
    }
}
