// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Bili.Lib.Interfaces;
using Bili.Models.App.Other;
using Bili.Models.Data.Pgc;
using Bili.Models.Enums;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Pgc;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Bili.ViewModels.Uwp.Pgc
{
    /// <summary>
    /// 时间线页面视图模型.
    /// </summary>
    public sealed partial class TimelinePageViewModel : ViewModelBase, ITimelinePageViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TimelinePageViewModel"/> class.
        /// </summary>
        /// <param name="pgcProvider">PGC 服务提供者.</param>
        /// <param name="resourceToolkit">资源管理工具.</param>
        public TimelinePageViewModel(
            IPgcProvider pgcProvider,
            IResourceToolkit resourceToolkit)
        {
            _pgcProvider = pgcProvider;
            _resourceToolkit = resourceToolkit;
            Timelines = new ObservableCollection<TimelineInformation>();

            InitializeCommand = ReactiveCommand.CreateFromTask(InitializeAsync);
            ReloadCommand = ReactiveCommand.CreateFromTask(ReloadAsync);

            InitializeCommand.IsExecuting
                .Merge(ReloadCommand.IsExecuting)
                .ToPropertyEx(this, x => x.IsReloading);

            InitializeCommand.ThrownExceptions
                .Merge(ReloadCommand.ThrownExceptions)
                .Subscribe(DisplayException);
        }

        /// <inheritdoc/>
        public void DisplayException(Exception exception)
        {
            IsError = true;
            var msg = exception is ServiceException se
                ? se.GetMessage()
                : exception.Message;
            ErrorText = $"{_resourceToolkit.GetLocaleString(LanguageNames.RequestPgcTimeLineFailed)}\n{msg}";
            LogException(exception);
        }

        /// <summary>
        /// 设置 PGC 类型.
        /// </summary>
        /// <param name="type">PGC 类型.</param>
        public void SetType(PgcType type)
        {
            _type = type;
            Title = string.Empty;
            Description = string.Empty;
            TryClear(Timelines);
        }

        private async Task InitializeAsync()
        {
            if (Timelines.Count > 0)
            {
                return;
            }

            await ReloadAsync();
        }

        private async Task ReloadAsync()
        {
            if (IsReloading)
            {
                return;
            }

            TryClear(Timelines);
            var data = await _pgcProvider.GetPgcTimelinesAsync(_type);
            Title = data.Title;
            Description = data.Description;
            data.Timelines.ToList().ForEach(p => Timelines.Add(p));
        }
    }
}
