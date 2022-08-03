// Copyright (c) Richasy. All rights reserved.

using Bili.Models.Data.User;
using Bili.ViewModels.Interfaces.Community;
using Windows.UI.Xaml.Navigation;

namespace Bili.App.Pages.Desktop.Overlay
{
    /// <summary>
    /// 关注用户页面.
    /// </summary>
    public sealed partial class FollowsPage : FollowsPageBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FansPage"/> class.
        /// </summary>
        public FollowsPage()
        {
            InitializeComponent();
        }

        /// <inheritdoc/>
        protected override void OnPageLoaded()
            => Bindings.Update();

        /// <inheritdoc/>
        protected override void OnPageUnloaded()
            => Bindings.StopTracking();

        /// <inheritdoc/>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is UserProfile profile)
            {
                ViewModel.SetProfile(profile);
            }
        }
    }

    /// <summary>
    /// <see cref="FollowsPage"/> 的基类.
    /// </summary>
    public class FollowsPageBase : AppPage<IFollowsPageViewModel>
    {
    }
}
