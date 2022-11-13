// Copyright (c) Richasy. All rights reserved.

using Bili.ViewModels.Interfaces.Account;
using Bili.ViewModels.Interfaces.Core;
using CommunityToolkit.Mvvm.Input;

namespace Bili.ViewModels.Desktop.Account
{
    /// <summary>
    /// XBOX 账户页面视图模型.
    /// </summary>
    public sealed partial class XboxAccountPageViewModel
    {
        private readonly INavigationViewModel _navigationViewModel;

        /// <inheritdoc/>
        public IAccountViewModel AccountViewModel { get; }

        /// <inheritdoc/>
        public IRelayCommand GotoFavoritePageCommand { get; }

        /// <inheritdoc/>
        public IRelayCommand GotoViewLaterPageCommand { get; }

        /// <inheritdoc/>
        public IRelayCommand GotoHistoryPageCommand { get; }

        /// <inheritdoc/>
        public IRelayCommand SignOutCommand { get; }
    }
}
