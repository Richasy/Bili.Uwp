// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Bili.Models.App.Other;
using Bili.Toolkit.Interfaces;
using ReactiveUI;
using Splat;

namespace Bili.ViewModels.Uwp.Base
{
    /// <summary>
    /// 信息流视图模型基类，支持重载和增量加载.
    /// </summary>
    public abstract partial class InformationFlowViewModelBase : ViewModelBase, IInitializeViewModel, IReloadViewModel, IIncrementalViewModel
    {
        internal InformationFlowViewModelBase(IResourceToolkit resourceToolkit)
        {
            _resourceToolkit = resourceToolkit;
            VideoCollection = new ObservableCollection<IVideoBaseViewModel>();

            var canRequest = this.WhenAnyValue(
                x => x.IsReloading,
                x => x.IsIncrementalLoading,
                (isInitializing, isIncrementalLoading) => !isInitializing && !isIncrementalLoading);

            var canInitialize = this.WhenAnyValue(
                x => x.VideoCollection.Count,
                count => count == 0);

            InitializeCommand = ReactiveCommand.CreateFromTask(ReloadAsync, canInitialize, outputScheduler: RxApp.MainThreadScheduler);
            ReloadCommand = ReactiveCommand.CreateFromTask(ReloadAsync, canRequest, RxApp.MainThreadScheduler);
            IncrementalCommand = ReactiveCommand.CreateFromTask(IncrementalAsync, canRequest, RxApp.MainThreadScheduler);

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

        private async Task ReloadAsync()
        {
            BeforeReload();
            VideoCollection.Clear();
            ClearException();
            await GetDataAsync();
        }

        private async Task IncrementalAsync()
            => await GetDataAsync();

        private void DisplayException(Exception exception)
        {
            IsError = true;
            var msg = exception is ServiceException se
                ? se.Error?.Message ?? se.Message
                : exception.Message;
            ErrorText = $"{_resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.RequestPopularFailed)}\n{msg}";
            LogException(exception);
        }

        private void LogException(Exception exception)
            => this.Log().Debug(exception);

        private void ClearException()
        {
            IsError = false;
            ErrorText = string.Empty;
        }
    }
}
