// Copyright (c) Richasy. All rights reserved.

using Bili.Models.Data.Dynamic;
using Bilibili.App.Dynamic.V2;

namespace Bili.Adapter.Interfaces
{
    /// <summary>
    /// 动态数据适配器接口定义.
    /// </summary>
    public interface IDynamicAdapter
    {
        /// <summary>
        /// 将动态条目 <see cref="DynamicItem"/> 转换为动态信息.
        /// </summary>
        /// <param name="item">动态条目.</param>
        /// <returns><see cref="DynamicInformation"/>.</returns>
        DynamicInformation ConvertToDynamicInformation(DynamicItem item);

        /// <summary>
        /// 将动态转发条目 <see cref="MdlDynForward"/> 转换为动态信息.
        /// </summary>
        /// <param name="forward">动态转发条目.</param>
        /// <returns><see cref="DynamicInformation"/>.</returns>
        DynamicInformation ConvertToDynamicInformation(MdlDynForward forward);

        /// <summary>
        /// 将视频动态响应 <see cref="DynVideoReply"/> 转换为动态视图.
        /// </summary>
        /// <param name="reply">视频动态响应.</param>
        /// <returns><see cref="DynamicView"/>.</returns>
        DynamicView ConvertToDynamicView(DynVideoReply reply);

        /// <summary>
        /// 将综合动态响应 <see cref="DynAllReply"/> 转换为动态视图.
        /// </summary>
        /// <param name="reply">综合动态响应.</param>
        /// <returns><see cref="DynamicView"/>.</returns>
        DynamicView ConvertToDynamicView(DynAllReply reply);
    }
}
