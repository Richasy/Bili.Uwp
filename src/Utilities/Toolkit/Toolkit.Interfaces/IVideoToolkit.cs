// Copyright (c) Richasy. All rights reserved.
using Bili.Models.Enums;

namespace Bili.Toolkit.Interfaces
{
    /// <summary>
    /// 视频相关工具.
    /// </summary>
    public interface IVideoToolkit
    {
        /// <summary>
        /// 获取视频Id类型.
        /// </summary>
        /// <param name="id">视频Id.</param>
        /// <param name="avId">解析后的 Aid.</param>
        /// <returns><c>av</c> 表示 AV Id, <c>bv</c> 表示 BV Id, 空表示不规范.</returns>
        VideoIdType GetVideoIdType(string id, out string avId);
    }
}
