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
        public IRelayCommand ChangeSaveLocationCommand { get; }

        /// <inheritdoc/>
        public IRelayCommand SaveDownloadTextCommand { get; }

        /// <inheritdoc/>
        public ObservableCollection<INumberPartViewModel> TotalPartCollection { get; }

        /// <inheritdoc/>
        public string DownloadParameter { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public bool UseMp4Box { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public bool OnlyHevc { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public bool OnlyAvc { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public bool OnlyAv1 { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public bool OnlyAudio { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public bool OnlyVideo { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public bool OnlySubtitle { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public bool UseMultiThread { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public bool UseTvInterface { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public bool UseAppInterface { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public bool UseInternationalInterface { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public bool DownloadDanmaku { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public string DownloadFolder { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public bool UseInteractionQuality { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public bool IsShowPart { get; set; }
    }
}
