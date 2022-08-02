// Copyright (c) Richasy. All rights reserved.

using System.Collections.ObjectModel;
using System.Reactive;
using Bili.Models.App.Other;
using Bili.Models.Data.Local;
using Bili.Toolkit.Interfaces;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Bili.ViewModels.Uwp.Home
{
    /// <summary>
    /// 帮助支持的视图模型.
    /// </summary>
    public sealed partial class HelpPageViewModel
    {
        private readonly IFileToolkit _fileToolkit;
        private readonly IAppToolkit _appToolkit;

        /// <inheritdoc/>
        public ObservableCollection<KeyValue<string>> LinkCollection { get; }

        /// <inheritdoc/>
        public ObservableCollection<QuestionModule> QuestionCollection { get; }

        /// <inheritdoc/>
        [Reactive]
        public QuestionModule CurrentQuestionModule { get; set; }

        /// <inheritdoc/>
        public ReactiveCommand<Unit, Unit> AskIssueCommand { get; }

        /// <inheritdoc/>
        public ReactiveCommand<Unit, Unit> GotoProjectHomeCommand { get; }

        /// <inheritdoc/>
        public ReactiveCommand<Unit, Unit> GotoDeveloperBiliBiliHomePageCommand { get; }

        /// <inheritdoc/>
        public ReactiveCommand<Unit, Unit> InitializeCommand { get; }
    }
}
