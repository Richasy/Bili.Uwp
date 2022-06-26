// Copyright (c) Richasy. All rights reserved.

using System;
using System.Diagnostics;
using System.Reactive.Concurrency;
using ReactiveUI;
using Splat;

namespace Bili.ViewModels.Uwp
{
    /// <summary>
    /// 未捕获的错误处理程序.
    /// </summary>
    public class UnhandledExceptionHandler : IObserver<Exception>
    {
        /// <inheritdoc/>
        public void OnNext(Exception value)
        {
            if (Debugger.IsAttached)
            {
                Debugger.Break();
            }

            Locator.Current.GetService<ILogger>().Write(value, "出错了", LogLevel.Error);
            RxApp.MainThreadScheduler.Schedule(() => { throw value; });
        }

        /// <inheritdoc/>
        public void OnError(Exception error)
        {
            if (Debugger.IsAttached)
            {
                Debugger.Break();
            }

            Locator.Current.GetService<ILogger>().Write(error, "出错了", LogLevel.Error);
            RxApp.MainThreadScheduler.Schedule(() => { throw error; });
        }

        /// <inheritdoc/>
        public void OnCompleted()
        {
            if (Debugger.IsAttached)
            {
                Debugger.Break();
            }
        }
    }
}
