// Copyright (c) Richasy. All rights reserved.

using Bili.Models.Data.Video;

namespace Bili.ViewModels.Uwp
{
    /// <summary>
    /// 视频基类视图模型接口.
    /// </summary>
    public interface IVideoBaseViewModel
    {
        /// <summary>
        /// 传入数据.
        /// </summary>
        /// <param name="information">核心数据.</param>
        void SetInformation(IVideoBase information);
    }
}
