// Copyright (c) Richasy. All rights reserved.

using System.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Bili.ViewModels.Interfaces.Account
{
    /// <summary>
    /// XBOX 账户页面视图模型的接口定义.
    /// </summary>
    public interface IXboxAccountPageViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// 账户视图模型.
        /// </summary>
        IAccountViewModel AccountViewModel { get; }

        /// <summary>
        /// 前往收藏页面的命令.
        /// </summary>
        IRelayCommand GotoFavoritePageCommand { get; }

        /// <summary>
        /// 前往稍后再看页面的命令.
        /// </summary>
        IRelayCommand GotoViewLaterPageCommand { get; }

        /// <summary>
        /// 前往历史记录页面的命令.
        /// </summary>
        IRelayCommand GotoHistoryPageCommand { get; }

        /// <summary>
        /// 登出命令.
        /// </summary>
        IRelayCommand SignOutCommand { get; }
    }
}
