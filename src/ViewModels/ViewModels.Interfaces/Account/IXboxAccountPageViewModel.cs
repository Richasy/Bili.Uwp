// Copyright (c) Richasy. All rights reserved.

using System.Reactive;
using ReactiveUI;

namespace Bili.ViewModels.Interfaces.Account
{
    /// <summary>
    /// XBOX 账户页面视图模型的接口定义.
    /// </summary>
    public interface IXboxAccountPageViewModel : IReactiveObject
    {
        /// <summary>
        /// 账户视图模型.
        /// </summary>
        IAccountViewModel AccountViewModel { get; }

        /// <summary>
        /// 前往收藏页面的命令.
        /// </summary>
        ReactiveCommand<Unit, Unit> GotoFavoritePageCommand { get; }

        /// <summary>
        /// 前往稍后再看页面的命令.
        /// </summary>
        ReactiveCommand<Unit, Unit> GotoViewLaterPageCommand { get; }

        /// <summary>
        /// 前往历史记录页面的命令.
        /// </summary>
        ReactiveCommand<Unit, Unit> GotoHistoryPageCommand { get; }

        /// <summary>
        /// 登出命令.
        /// </summary>
        ReactiveCommand<Unit, Unit> SignOutCommand { get; }
    }
}
