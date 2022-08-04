// Copyright (c) Richasy. All rights reserved.

using System.Collections.ObjectModel;
using System.Reactive;
using Bili.Lib.Interfaces;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Account;
using Bili.ViewModels.Interfaces.Common;
using Bili.ViewModels.Interfaces.Core;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Bili.ViewModels.Uwp.Common
{
    /// <summary>
    /// 下载模块视图模型.
    /// </summary>
    public sealed partial class DownloadModuleViewModel
    {
        private readonly ISettingsToolkit _settingsToolkit;
        private readonly IResourceToolkit _resourceToolkit;
        private readonly IAuthorizeProvider _authorizeProvider;
        private readonly ICallerViewModel _callerViewModel;
        private readonly IAccountViewModel _accountViewModel;

        /// <inheritdoc/>
        public ReactiveCommand<Unit, Unit> ChangeSaveLocationCommand { get; }

        /// <inheritdoc/>
        public ReactiveCommand<Unit, Unit> SaveDownloadTextCommand { get; }

        /// <inheritdoc/>
        public ObservableCollection<INumberPartViewModel> TotalPartCollection { get; }

        /// <inheritdoc/>
        public string DownloadParameter { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public bool UseMp4Box { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public bool OnlyHevc { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public bool OnlyAvc { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public bool OnlyAv1 { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public bool OnlyAudio { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public bool OnlyVideo { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public bool OnlySubtitle { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public bool UseMultiThread { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public bool UseTvInterface { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public bool UseAppInterface { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public bool UseInternationalInterface { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public bool DownloadDanmaku { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public string DownloadFolder { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public bool UseInteractionQuality { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public bool IsShowPart { get; set; }
    }
}
