// Copyright (c) Richasy. All rights reserved.

using System;
using Bili.Desktop.App.Pages.Base;
using Bili.Models.Data.User;
using Microsoft.UI.Xaml.Navigation;

namespace Bili.Desktop.App.Pages.Desktop.Overlay
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
        protected override void OnPageLoaded()
            => Bindings.Update();

        /// <inheritdoc/>
        protected override void OnPageUnloaded()
            => Bindings.StopTracking();

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

        private void OnSearchBoxQuerySubmitted(Microsoft.UI.Xaml.Controls.AutoSuggestBox sender, Microsoft.UI.Xaml.Controls.AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (!ViewModel.CanSearch)
            {
                return;
            }

            ViewModel.SearchCommand.ExecuteAsync(null);
        }
    }
}
