// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using Bili.Models.App.Other;
using Bili.Toolkit.Interfaces;
using ReactiveUI.Fody.Helpers;

namespace Bili.ViewModels.Uwp
{
    /// <summary>
    /// 帮助支持的视图模型.
    /// </summary>
    public partial class HelpViewModel
    {
        private readonly IFileToolkit _fileToolkit;
        private readonly IAppToolkit _appToolkit;

        /// <summary>
        /// 实例.
        /// </summary>
        public static HelpViewModel Instance { get; } = new Lazy<HelpViewModel>(() => new HelpViewModel()).Value;

        /// <summary>
        /// 关联链接集合.
        /// </summary>
        [Reactive]
        public ObservableCollection<KeyValue<string>> LinkCollection { get; set; }

        /// <summary>
        /// 问题集合.
        /// </summary>
        [Reactive]
        public ObservableCollection<QuestionModule> QuestionCollection { get; set; }

        /// <summary>
        /// 当前问题模块.
        /// </summary>
        [Reactive]
        public QuestionModule CurrentQuestionModule { get; set; }
    }
}
