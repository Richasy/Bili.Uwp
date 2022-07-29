// Copyright (c) Richasy. All rights reserved.

namespace Bili.ViewModels.Interfaces
{
    /// <summary>
    /// 需要注入数据源的视图模型接口.
    /// </summary>
    /// <typeparam name="T">数据类型.</typeparam>
    public interface IInjectDataViewModel<T>
        where T : class
    {
        /// <summary>
        /// 被注入的数据.
        /// </summary>
        T Data { get; }

        /// <summary>
        /// 注入数据.
        /// </summary>
        /// <param name="data">数据.</param>
        void InjectData(T data);
    }
}
