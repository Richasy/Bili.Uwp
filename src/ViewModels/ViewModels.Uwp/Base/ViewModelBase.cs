// Copyright (c) Richasy. All rights reserved.

using System;
using System.Threading.Tasks;
using Bili.Models.App.Constants;
using Bili.Models.App.Other;
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
        /// 从错误中获取错误的消息文本.
        /// </summary>
        /// <param name="exception">抛出的异常.</param>
        /// <returns>错误消息.</returns>
        protected string GetErrorMessage(Exception exception)
        {
            var msg = exception is ServiceException se
                ? se.Error?.Message ?? se.Message
                : exception.Message;
            return msg;
        }

        /// <summary>
        /// 记录错误信息.
        /// </summary>
        /// <param name="exception">错误信息.</param>
        protected void LogException(Exception exception)
            => this.Log().Debug(exception);

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
    }
}
