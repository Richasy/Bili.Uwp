// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Bili.DI.Container;
using Bili.Models.App.Constants;
using Bili.Models.App.Other;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Bili.ViewModels.Uwp
{
    /// <summary>
    /// ViewModel的基类.
    /// </summary>
    public class ViewModelBase : ObservableObject
    {
        /// <summary>
        /// 从错误中获取错误的消息文本.
        /// </summary>
        /// <param name="exception">抛出的异常.</param>
        /// <returns>错误消息.</returns>
        protected string GetErrorMessage(Exception exception)
        {
            var msg = exception is ServiceException se
                ? se.GetMessage()
                : exception.Message;
            return msg;
        }

        /// <summary>
        /// 记录错误信息.
        /// </summary>
        /// <param name="exception">错误信息.</param>
        protected void LogException(Exception exception)
        {
            Debug.WriteLine(exception.StackTrace);
            var logger = NLog.LogManager.GetLogger("Richasy.Bili");
            logger.Error(exception);
        }

        /// <summary>
        /// 这是一种退避策略，当调用时，通常意味着重新导航到了加载过的页面，
        /// 此时视频集合中已经有了数十条数据，如果直接让 UI 渲染，会出现导航动画与列表渲染的资源竞争，导致 UI 卡顿。
        /// 这里的延时会让 UI 先渲染加载状态，等页面可以被用户感知到的时候再进行已有视频条目的渲染。
        /// 这个 250ms 是一个估计值，并不能够完全确保 UI 不会卡顿（绝大部分情况下可以）.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        protected Task FakeLoadingAsync()
            => Task.Delay(AppConstants.FakeLoadingMilliseconds);

        /// <summary>
        /// 执行一个具有最小运行时间的任务.
        /// </summary>
        /// <param name="work">需要执行的任务.</param>
        /// <returns><see cref="Task"/>.</returns>
        protected Task RunDelayTask(Task work)
            => Task.WhenAll(
                    Task.Run(async () => await work),
                    Task.Delay(AppConstants.FakeLoadingMilliseconds));

        /// <summary>
        /// 尝试清除集合，仅在集合内有数据时才执行.
        /// </summary>
        /// <typeparam name="T">数据类型.</typeparam>
        /// <param name="collection">集合.</param>
        protected void TryClear<T>(ObservableCollection<T> collection)
        {
            if (collection.Count > 0)
            {
                collection.Clear();
            }
        }

        /// <summary>
        /// 为异步命令添加错误回调.
        /// </summary>
        /// <param name="handler">错误回调.</param>
        /// <param name="commands">命令集.</param>
        protected void AttachExceptionHandlerToAsyncCommand(Action<Exception> handler, params IAsyncRelayCommand[] commands)
        {
            foreach (var command in commands)
            {
                command.PropertyChanged += (s, e) =>
                {
                    if (e.PropertyName == nameof(AsyncRelayCommand.ExecutionTask) &&
                        ((IAsyncRelayCommand)s).ExecutionTask is Task task &&
                        task.Exception is AggregateException exception)
                    {
                        exception.Handle(ex =>
                        {
                            handler(ex);
                            return true;
                        });
                    }
                };
            }
        }

        /// <summary>
        /// 为异步命令的 <see cref="AsyncRelayCommand.IsRunning"/> 属性添加回调.
        /// </summary>
        /// <param name="handler">回调.</param>
        /// <param name="commands">命令集合.</param>
        protected void AttachIsRunningToAsyncCommand(Action<bool> handler, params IAsyncRelayCommand[] commands)
        {
            foreach (var command in commands)
            {
                command.PropertyChanged += (s, e) =>
                {
                    if (e.PropertyName == nameof(AsyncRelayCommand.IsRunning))
                    {
                        handler(command.IsRunning);
                    }
                };
            }
        }
    }
}
