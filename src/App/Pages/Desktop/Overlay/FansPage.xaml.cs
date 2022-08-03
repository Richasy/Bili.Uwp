// Copyright (c) Richasy. All rights reserved.

using Bili.Models.Data.User;
using Bili.ViewModels.Interfaces.Community;
using Windows.UI.Xaml.Navigation;

namespace Bili.App.Pages.Desktop.Overlay
{
    /// <summary>
    /// 粉丝详情页面.
    /// </summary>
    public sealed partial class FansPage : FansPageBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FansPage"/> class.
        /// </summary>
        public FansPage() => InitializeComponent();

        /// <inheritdoc/>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is UserProfile profile)
            {
                ViewModel.SetProfile(profile);
            }
        }

        /// <inheritdoc/>
        protected override void OnPageLoaded()
            => Bindings.Update();

        /// <inheritdoc/>
        protected override void OnPageUnloaded()
            => Bindings.StopTracking();
    }

    /// <summary>
    /// <see cref="FansPage"/> 的基类.
    /// </summary>
    public class FansPageBase : AppPage<IFansPageViewModel>
    {
    }
}
