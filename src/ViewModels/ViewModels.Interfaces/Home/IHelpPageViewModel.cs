// Copyright (c) Richasy. All rights reserved.

using System.Collections.ObjectModel;
using System.ComponentModel;
using Bili.Models.App.Other;
using Bili.Models.Data.Local;
using CommunityToolkit.Mvvm.Input;

namespace Bili.ViewModels.Interfaces.Home
{
    /// <summary>
    /// 帮助支持页面视图模型的接口定义.
    /// </summary>
    public interface IHelpPageViewModel : INotifyPropertyChanged, IInitializeViewModel
    {
        /// <summary>
        /// 报告问题命令.
        /// </summary>
        IAsyncRelayCommand AskIssueCommand { get; }

        /// <summary>
        /// 打开项目主页命令.
        /// </summary>
        IAsyncRelayCommand GotoProjectHomeCommand { get; }

        /// <summary>
        /// 打开开发者B站账户页面命令.
        /// </summary>
        IAsyncRelayCommand GotoDeveloperBiliBiliHomePageCommand { get; }

        /// <summary>
        /// 关联链接集合.
        /// </summary>
        ObservableCollection<KeyValue<string>> LinkCollection { get; }

        /// <summary>
        /// 问题集合.
        /// </summary>
        ObservableCollection<QuestionModule> QuestionCollection { get; }

        /// <summary>
        /// 当前问题模块.
        /// </summary>
        QuestionModule CurrentQuestionModule { get; set; }
    }
}
