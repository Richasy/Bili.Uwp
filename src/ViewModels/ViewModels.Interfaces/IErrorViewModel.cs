// Copyright (c) Richasy. All rights reserved.

using System;

namespace Bili.ViewModels.Interfaces
{
    /// <summary>
    /// 支持显示错误内容的视图模型.
    /// </summary>
    public interface IErrorViewModel
    {
        /// <summary>
        /// 是否发生了错误.
        /// </summary>
        bool IsError { get; }

        /// <summary>
        /// 错误内容.
        /// </summary>
        string ErrorText { get; }

        /// <summary>
        /// 显示错误信息.
        /// </summary>
        /// <param name="exception">错误实例.</param>
        void DisplayException(Exception exception);
    }
}
