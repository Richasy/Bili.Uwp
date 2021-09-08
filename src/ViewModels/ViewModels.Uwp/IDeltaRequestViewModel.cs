// Copyright (c) Richasy. All rights reserved.

using System.Threading.Tasks;

namespace Richasy.Bili.ViewModels.Uwp
{
    /// <summary>
    /// 增量请求视图模型接口.
    /// </summary>
    public interface IDeltaRequestViewModel
    {
        /// <summary>
        /// 请求数据.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        Task RequestDataAsync();

        /// <summary>
        /// 初始化请求.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        Task InitializeRequestAsync();
    }
}
