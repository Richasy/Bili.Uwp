// Copyright (c) Richasy. All rights reserved.

using Bili.App.Pages.Base;
using Bili.Models.Data.User;
using Windows.UI.Xaml.Navigation;

namespace Bili.App.Pages.Xbox.Overlay
{
    /// <summary>
    /// 用户空间页面.
    /// </summary>
    public sealed partial class UserSpacePage : UserSpacePageBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserSpacePage"/> class.
        /// </summary>
        public UserSpacePage() => InitializeComponent();

        /// <inheritdoc/>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is string userId)
            {
                var profile = new UserProfile(userId);
                ViewModel.SetUserProfile(profile);
            }
            else if (e.Parameter is UserProfile profile)
            {
                ViewModel.SetUserProfile(profile);
            }

            ViewModel.ReloadCommand.ExecuteAsync(null);
        }

        /// <inheritdoc/>
        protected override void OnPageLoaded()
            => Bindings.Update();

        /// <inheritdoc/>
        protected override void OnPageUnloaded()
            => Bindings.StopTracking();

        private void OnSearchBoxQuerySubmitted(Windows.UI.Xaml.Controls.AutoSuggestBox sender, Windows.UI.Xaml.Controls.AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (!ViewModel.CanSearch)
            {
                return;
            }

            ViewModel.SearchCommand.ExecuteAsync(null);
        }
    }
}
