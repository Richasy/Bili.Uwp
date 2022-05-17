// Copyright (c) Richasy. All rights reserved.

using System;
using ReactiveUI;
using Splat;

namespace Bili.ViewModels.Uwp
{
    /// <summary>
    /// ViewModel的基类.
    /// </summary>
    public class ViewModelBase : ReactiveObject
    {
        /// <summary>
        /// 记录错误信息.
        /// </summary>
        /// <param name="exception">错误信息.</param>
        protected void LogException(Exception exception)
            => this.Log().Debug(exception);
    }
}
