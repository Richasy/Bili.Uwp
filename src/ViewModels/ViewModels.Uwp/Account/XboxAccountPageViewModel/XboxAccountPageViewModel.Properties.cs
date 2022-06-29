// Copyright (c) Richasy. All rights reserved.

using System.Reactive;
using Bili.ViewModels.Uwp.Core;
using ReactiveUI;

namespace Bili.ViewModels.Uwp.Account
{
    /// <summary>
    /// XBOX 账户页面视图模型.
    /// </summary>
    public sealed partial class XboxAccountPageViewModel
    {
        private readonly NavigationViewModel _navigationViewModel;

        /// <summary>
        /// 账户视图模型.
        /// </summary>
        public AccountViewModel AccountViewModel { get; }

        /// <summary>
        /// 前往收藏页面的命令.
        /// </summary>
        public ReactiveCommand<Unit, Unit> GotoFavoritePageCommand { get; }

        /// <summary>
        /// 前往稍后再看页面的命令.
        /// </summary>
        public ReactiveCommand<Unit, Unit> GotoViewLaterPageCommand { get; }

        /// <summary>
        /// 前往历史记录页面的命令.
        /// </summary>
        public ReactiveCommand<Unit, Unit> GotoHistoryPageCommand { get; }

        /// <summary>
        /// 登出命令.
        /// </summary>
        public ReactiveCommand<Unit, Unit> SignOutCommand { get; }
    }
}
