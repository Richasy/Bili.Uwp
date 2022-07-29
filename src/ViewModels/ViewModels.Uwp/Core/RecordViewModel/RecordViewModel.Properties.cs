// Copyright (c) Richasy. All rights reserved.

using System.Collections.ObjectModel;
using System.Reactive;
using Bili.Models.Data.Local;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Core;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Bili.ViewModels.Uwp.Core
{
    /// <summary>
    /// 本机播放记录视图模型的接口定义.
    /// </summary>
    public sealed partial class RecordViewModel
    {
        private readonly IFileToolkit _fileToolkit;
        private readonly ISettingsToolkit _settingsToolkit;
        private readonly ICallerViewModel _callerViewModel;

        /// <inheritdoc/>
        public ReactiveCommand<Unit, Unit> CheckContinuePlayCommand { get; }

        /// <inheritdoc/>
        public ReactiveCommand<PlaySnapshot, Unit> AddLastPlayItemCommand { get; }

        /// <inheritdoc/>
        public ReactiveCommand<Unit, Unit> DeleteLastPlayItemCommand { get; }

        /// <inheritdoc/>
        public ReactiveCommand<PlayRecord, Unit> AddPlayRecordCommand { get; }

        /// <inheritdoc/>
        public ReactiveCommand<PlayRecord, Unit> RemovePlayRecordCommand { get; }

        /// <inheritdoc/>
        public ReactiveCommand<Unit, Unit> ClearPlayRecordCommand { get; }

        /// <inheritdoc/>
        public ObservableCollection<PlayRecord> PlayRecords { get; }

        /// <inheritdoc/>
        [Reactive]
        public bool IsShowPlayRecordButton { get; set; }
    }
}
