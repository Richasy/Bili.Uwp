// Copyright (c) Richasy. All rights reserved.

using System;
using Bili.ViewModels.Uwp.Core;
using ReactiveUI;

namespace Bili.ViewModels.Uwp.Account
{
    /// <summary>
    /// XBOX 账户页面视图模型.
    /// </summary>
    public sealed partial class XboxAccountPageViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="XboxAccountPageViewModel"/> class.
        /// </summary>
        public XboxAccountPageViewModel(
            NavigationViewModel navigationViewModel,
            AccountViewModel accountViewModel)
        {
            _navigationViewModel = navigationViewModel;
            AccountViewModel = accountViewModel;

            GotoFavoritePageCommand = ReactiveCommand.Create(GotoFavoritePage);
            GotoViewLaterPageCommand = ReactiveCommand.Create(GotoViewLaterPage);
            GotoHistoryPageCommand = ReactiveCommand.Create(GotoHistoryPage);
            SignOutCommand = ReactiveCommand.Create(SignOut);
        }

        private void GotoFavoritePage()
            => _navigationViewModel.NavigateToSecondaryView(Models.Enums.PageIds.Favorite);

        private void GotoViewLaterPage()
            => _navigationViewModel.NavigateToSecondaryView(Models.Enums.PageIds.ViewLater);

        private void GotoHistoryPage()
            => _navigationViewModel.NavigateToSecondaryView(Models.Enums.PageIds.ViewHistory);

        private void SignOut()
        {
            _navigationViewModel.NavigateToMainView(Models.Enums.PageIds.Recommend);
            AccountViewModel.SignOutCommand.Execute().Subscribe();
        }
    }
}
