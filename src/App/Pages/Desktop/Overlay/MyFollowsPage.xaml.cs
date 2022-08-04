// Copyright (c) Richasy. All rights reserved.

using System;
using Bili.Models.Data.Community;
using Bili.ViewModels.Interfaces.Account;

namespace Bili.App.Pages.Desktop
{
    /// <summary>
    /// 我的关注页面.
    /// </summary>
    public sealed partial class MyFollowsPage : MyFollowsPageBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MyFollowsPage"/> class.
        /// </summary>
        public MyFollowsPage() => InitializeComponent();

        /// <inheritdoc/>
        protected override void OnPageLoaded()
            => Bindings.Update();

        /// <inheritdoc/>
        protected override void OnPageUnloaded()
            => Bindings.StopTracking();

        private void OnNavItemInvoked(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewItemInvokedEventArgs args)
        {
            var data = args.InvokedItem as FollowGroup;
            if (data != ViewModel.CurrentGroup)
            {
                ViewModel.SelectGroupCommand.Execute(data).Subscribe();
            }
        }
    }

    /// <summary>
    /// <see cref="MyFollowsPage"/> 的基类.
    /// </summary>
    public class MyFollowsPageBase : AppPage<IMyFollowsPageViewModel>
    {
    }
}
