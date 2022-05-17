// Copyright (c) Richasy. All rights reserved.

namespace Bili.ViewModels.Interfaces
{
    /// <summary>
    /// 支持后退行为并预期发送回退参数的页面视图模型接口.
    /// </summary>
    public interface IBackPageViewModel
    {
        /// <summary>
        /// 获取回退参数.
        /// </summary>
        /// <returns>回退页面时附加的参数.</returns>
        object GetBackParameter();
    }
}
