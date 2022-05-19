// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Bili.Models.App.Other;
using Bili.ViewModels.Interfaces;
using ReactiveUI;
using Windows.UI.Core;

namespace Bili.ViewModels.Uwp.Base
{
    /// <summary>
    /// 信息流视图模型基类，支持重载和增量加载.
    /// </summary>
    public abstract partial class InformationFlowViewModelBase : ViewModelBase, IInitializeViewModel, IReloadViewModel, IIncrementalViewModel
    {
        internal InformationFlowViewModelBase(CoreDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
            VideoCollection = new ObservableCollection<IVideoBaseViewModel>();

            InitializeCommand = ReactiveCommand.CreateFromTask(InitializeAsync, outputScheduler: RxApp.MainThreadScheduler);
            ReloadCommand = ReactiveCommand.CreateFromTask(ReloadAsync, outputScheduler: RxApp.MainThreadScheduler);
            IncrementalCommand = ReactiveCommand.CreateFromTask(IncrementalAsync, outputScheduler: RxApp.MainThreadScheduler);

            _isReloading = ReloadCommand.IsExecuting
                .Merge(InitializeCommand.IsExecuting)
                .ToProperty(
                this,
                x => x.IsReloading,
                scheduler: RxApp.MainThreadScheduler);

            _isIncrementalLoading = IncrementalCommand.IsExecuting.ToProperty(
                this,
                x => x.IsIncrementalLoading,
                scheduler: RxApp.MainThreadScheduler);

            ReloadCommand.ThrownExceptions
                .Merge(InitializeCommand.ThrownExceptions)
                .Subscribe(DisplayException);

            IncrementalCommand.ThrownExceptions.Subscribe(LogException);
        }

        /// <summary>
        /// 在执行重新载入操作前的准备工作.
        /// </summary>
        protected virtual void BeforeReload()
        {
        }

        /// <summary>
        /// 从网络获取数据，并将其加入 <see cref="VideoCollection"/> 中.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        protected virtual Task GetDataAsync() => Task.CompletedTask;

        /// <summary>
        /// 拼接错误信息.
        /// </summary>
        /// <param name="errorMsg">原始错误信息.</param>
        /// <returns>格式化后的错误信息.</returns>
        protected virtual string FormatException(string errorMsg) => errorMsg;

        private async Task InitializeAsync()
        {
            if (VideoCollection.Count > 0)
            {
                await FakeLoadingAsync();
                return;
            }

            await ReloadAsync();
        }

        private void DisplayException(Exception exception)
        {
            IsError = true;
            var msg = exception is ServiceException se
                ? se.Error?.Message ?? se.Message
                : exception.Message;
            ErrorText = FormatException(msg);
            LogException(exception);
        }

        private async Task ReloadAsync()
        {
            BeforeReload();
            VideoCollection.Clear();
            ClearException();
            await _dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () => await GetDataAsync());
        }

        private async Task IncrementalAsync()
        {
            if (IsReloading || IsIncrementalLoading)
            {
                return;
            }

            await GetDataAsync();
        }

        private void ClearException()
        {
            IsError = false;
            ErrorText = string.Empty;
        }
    }
}
