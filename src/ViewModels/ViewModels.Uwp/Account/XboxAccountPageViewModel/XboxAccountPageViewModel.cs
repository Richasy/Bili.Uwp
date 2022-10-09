// Copyright (c) Richasy. All rights reserved.

using Bili.ViewModels.Interfaces.Account;
using Bili.ViewModels.Interfaces.Core;
using CommunityToolkit.Mvvm.Input;

namespace Bili.ViewModels.Uwp.Account
{
    /// <summary>
    /// XBOX 账户页面视图模型.
    /// </summary>
    public sealed partial class XboxAccountPageViewModel : ViewModelBase, IXboxAccountPageViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="XboxAccountPageViewModel"/> class.
        /// </summary>
        public XboxAccountPageViewModel(
            INavigationViewModel navigationViewModel,
            IAccountViewModel accountViewModel)
        {
            _navigationViewModel = navigationViewModel;
            AccountViewModel = accountViewModel;

            GotoFavoritePageCommand = new RelayCommand(GotoFavoritePage);
            GotoViewLaterPageCommand = new RelayCommand(GotoViewLaterPage);
            GotoHistoryPageCommand = new RelayCommand(GotoHistoryPage);
            SignOutCommand = new RelayCommand(SignOut);
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
            AccountViewModel.SignOutCommand.ExecuteAsync(null);
        }
    }
}
