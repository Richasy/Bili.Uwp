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
        public IRelayCommand CheckContinuePlayCommand { get; }

        /// <inheritdoc/>
        public IRelayCommand<PlaySnapshot> AddLastPlayItemCommand { get; }

        /// <inheritdoc/>
        public IRelayCommand DeleteLastPlayItemCommand { get; }

        /// <inheritdoc/>
        public IRelayCommand<PlayRecord> AddPlayRecordCommand { get; }

        /// <inheritdoc/>
        public IRelayCommand<PlayRecord> RemovePlayRecordCommand { get; }

        /// <inheritdoc/>
        public IRelayCommand ClearPlayRecordCommand { get; }

        /// <inheritdoc/>
        public ObservableCollection<PlayRecord> PlayRecords { get; }

        /// <inheritdoc/>
        [ObservableProperty]
        public bool IsShowPlayRecordButton { get; set; }
    }
}
