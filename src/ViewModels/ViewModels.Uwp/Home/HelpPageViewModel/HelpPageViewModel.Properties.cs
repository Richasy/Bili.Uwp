// Copyright (c) Richasy. All rights reserved.

using System.Collections.ObjectModel;
using Bili.Models.App.Other;
using Bili.Models.Data.Local;
using Bili.Toolkit.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Bili.ViewModels.Uwp.Home
{
    /// <summary>
    /// 帮助支持的视图模型.
    /// </summary>
    public sealed partial class HelpPageViewModel
    {
        private readonly IFileToolkit _fileToolkit;
        private readonly IAppToolkit _appToolkit;

        [ObservableProperty]
        private QuestionModule _currentQuestionModule;

        /// <inheritdoc/>
        public ObservableCollection<KeyValue<string>> LinkCollection { get; }

        /// <inheritdoc/>
        public ObservableCollection<QuestionModule> QuestionCollection { get; }

        /// <inheritdoc/>
        public IAsyncRelayCommand AskIssueCommand { get; }

        /// <inheritdoc/>
        public IAsyncRelayCommand GotoProjectHomeCommand { get; }

        /// <inheritdoc/>
        public IAsyncRelayCommand GotoDeveloperBiliBiliHomePageCommand { get; }

        /// <inheritdoc/>
        public IAsyncRelayCommand InitializeCommand { get; }
    }
}
