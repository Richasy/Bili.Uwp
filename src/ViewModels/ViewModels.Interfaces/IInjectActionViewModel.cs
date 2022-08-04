// Copyright (c) Richasy. All rights reserved.

using System;

namespace Bili.ViewModels.Interfaces
{
    /// <summary>
    /// 需要注入动作的视图模型的接口定义.
    /// </summary>
    /// <typeparam name="T">动作参数类型.</typeparam>
    public interface IInjectActionViewModel<T>
    {
        /// <summary>
        /// 注入动作.
        /// </summary>
        /// <param name="action">需要注入的动作.</param>
        void InjectAction(Action<T> action);
    }
}
