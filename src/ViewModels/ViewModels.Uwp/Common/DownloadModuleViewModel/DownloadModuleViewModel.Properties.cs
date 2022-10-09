// Copyright (c) Richasy. All rights reserved.

using System.Collections.ObjectModel;
using Bili.Lib.Interfaces;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Account;
using Bili.ViewModels.Interfaces.Common;
using Bili.ViewModels.Interfaces.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

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

        [ObservableProperty]
        private bool _useMp4Box;

        [ObservableProperty]
        private bool _onlyHevc;

        [ObservableProperty]
        private bool _onlyAvc;

        [ObservableProperty]
        private bool _onlyAv1;

        [ObservableProperty]
        private bool _onlyAudio;

        [ObservableProperty]
        private bool _onlyVideo;

        [ObservableProperty]
        private bool _onlySubtitle;

        [ObservableProperty]
        private bool _useMultiThread;

        [ObservableProperty]
        private bool _useTvInterface;

        [ObservableProperty]
        private bool _useAppInterface;

        [ObservableProperty]
        private bool _useInternationalInterface;

        [ObservableProperty]
        private bool _downloadDanmaku;

        [ObservableProperty]
        private string _downloadFolder;

        [ObservableProperty]
        private bool _useInteractionQuality;

        [ObservableProperty]
        private bool _isShowPart;

        /// <inheritdoc/>
        public IAsyncRelayCommand ChangeSaveLocationCommand { get; }

        /// <inheritdoc/>
        public IAsyncRelayCommand SaveDownloadTextCommand { get; }

        /// <inheritdoc/>
        public ObservableCollection<INumberPartViewModel> TotalPartCollection { get; }

        /// <inheritdoc/>
        public string DownloadParameter { get; set; }
    }
}
