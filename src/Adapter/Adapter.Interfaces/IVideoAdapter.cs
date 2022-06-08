// Copyright (c) Richasy. All rights reserved.

using System;
using Bili.Models.BiliBili;
using Bili.Models.Data.Video;
using Bilibili.App.Dynamic.V2;
using Bilibili.App.Interfaces.V1;
using Bilibili.App.View.V1;

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
        /// <exception cref="ArgumentException">传入的数据不是预期的视频类型.</exception>
        VideoInformation ConvertToVideoInformation(RecommendCard videoCard);

        /// <summary>
        /// 将分区视频 <see cref="PartitionVideo"/> 转换为视频信息.
        /// </summary>
        /// <param name="video">分区视频.</param>
        /// <returns><see cref="VideoInformation"/>.</returns>
        /// <remarks>
        /// 分区视频指的是网站按内容类型分的区域下的视频，比如舞蹈区视频、科技区视频等.
        /// </remarks>
        VideoInformation ConvertToVideoInformation(PartitionVideo video);

        /// <summary>
        /// 将动态里的视频内容 <see cref="MdlDynArchive"/> 转换为视频信息.
        /// </summary>
        /// <param name="dynamicVideo">动态里的视频内容.</param>
        /// <returns><see cref="VideoInformation"/>.</returns>
        /// <remarks>
        /// 有些番剧的更新可能也是以 <see cref="MdlDynArchive"/> 类型发布，需要提前根据 <c>IsPGC</c> 属性来判断它是不是视频内容，如果不是，执行该方法会抛出异常.
        /// </remarks>
        /// <exception cref="ArgumentException">传入的数据不是预期的视频类型.</exception>
        VideoInformation ConvertToVideoInformation(MdlDynArchive dynamicVideo);

        /// <summary>
        /// 将稍后再看里的视频内容 <see cref="ViewLaterVideo"/> 转换为视频信息.
        /// </summary>
        /// <param name="video">稍后再看的视频.</param>
        /// <returns><see cref="VideoInformation"/>.</returns>
        VideoInformation ConvertToVideoInformation(ViewLaterVideo video);

        /// <summary>
        /// 将排行榜里的视频内容 <see cref="Bilibili.App.Show.V1.Item"/> 转换为视频信息.
        /// </summary>
        /// <param name="rankVideo">排行榜视频.</param>
        /// <returns><see cref="VideoInformation"/>.</returns>
        VideoInformation ConvertToVideoInformation(Bilibili.App.Show.V1.Item rankVideo);

        /// <summary>
        /// 将热门视频内容 <see cref="Bilibili.App.Card.V1.Card"/> 转换为视频信息.
        /// </summary>
        /// <param name="hotVideo">热门视频.</param>
        /// <returns><see cref="VideoInformation"/>.</returns>
        VideoInformation ConvertToVideoInformation(Bilibili.App.Card.V1.Card hotVideo);

        /// <summary>
        /// 将关联视频内容 <see cref="Bilibili.App.View.V1.Relate"/> 转换为视频信息.
        /// </summary>
        /// <param name="relatedVideo">关联视频.</param>
        /// <returns><see cref="VideoInformation"/>.</returns>
        VideoInformation ConvertToVideoInformation(Bilibili.App.View.V1.Relate relatedVideo);

        /// <summary>
        /// 将视频搜索结果 <see cref="VideoSearchItem"/> 转换为视频信息.
        /// </summary>
        /// <param name="searchVideo">视频搜索结果.</param>
        /// <returns><see cref="VideoInformation"/>.</returns>
        VideoInformation ConvertToVideoInformation(VideoSearchItem searchVideo);

        /// <summary>
        /// 将用户空间内的视频内容 <see cref="UserSpaceVideoItem"/> 转换为视频信息.
        /// </summary>
        /// <param name="spaceVideo">用户空间内的视频内容.</param>
        /// <returns><see cref="VideoInformation"/>.</returns>
        VideoInformation ConvertToVideoInformation(UserSpaceVideoItem spaceVideo);

        /// <summary>
        /// 将历史记录里的视频内容 <see cref="CursorItem"/> 转换为视频信息.
        /// </summary>
        /// <param name="historyVideo">历史记录里的视频内容.</param>
        /// <returns><see cref="VideoInformation"/>.</returns>
        VideoInformation ConvertToVideoInformation(CursorItem historyVideo);

        /// <summary>
        /// 将收藏夹内的视频内容 <see cref="FavoriteMedia"/> 转换为视频信息.
        /// </summary>
        /// <param name="video">收藏夹视频.</param>
        /// <returns><see cref="VideoInformation"/>.</returns>
        VideoInformation ConvertToVideoInformation(FavoriteMedia video);

        /// <summary>
        /// 将用户空间里的视频搜索结果 <see cref="Arc"/> 转换为视频信息.
        /// </summary>
        /// <param name="video">用户空间里的视频搜索结果.</param>
        /// <returns><see cref="VideoInformation"/>.</returns>
        VideoInformation ConvertToVideoInformation(Arc video);

        /// <summary>
        /// 将视频详情 <see cref="ViewReply"/> 转换为视频信息.
        /// </summary>
        /// <param name="videoDetail">视频详情.</param>
        /// <returns><see cref="VideoView"/>.</returns>
        VideoView ConvertToVideoView(ViewReply videoDetail);

        /// <summary>
        /// 将稍后再看响应 <see cref="ViewLaterResponse"/> 转换为视频集.
        /// </summary>
        /// <param name="response">稍后在看响应结果.</param>
        /// <returns><see cref="VideoSet"/>.</returns>
        VideoSet ConvertToVideoSet(ViewLaterResponse response);

        /// <summary>
        /// 将用户空间视频集 <see cref="UserSpaceVideoSet"/> 转换为视频集.
        /// </summary>
        /// <param name="set">稍后在看响应结果.</param>
        /// <returns><see cref="VideoSet"/>.</returns>
        VideoSet ConvertToVideoSet(UserSpaceVideoSet set);

        /// <summary>
        /// 将视频搜索结果 <see cref="SearchArchiveReply"/> 转换为视频集.
        /// </summary>
        /// <param name="reply">稍后在看响应结果.</param>
        /// <returns><see cref="VideoSet"/>.</returns>
        VideoSet ConvertToVideoSet(SearchArchiveReply reply);

        /// <summary>
        /// 将历史记录响应结果 <see cref="CursorV2Reply"/> 转换为历史记录视图.
        /// </summary>
        /// <param name="reply">历史记录响应结果.</param>
        /// <returns><see cref="VideoHistoryView"/>.</returns>
        VideoHistoryView ConvertToVideoHistoryView(CursorV2Reply reply);
    }
}
