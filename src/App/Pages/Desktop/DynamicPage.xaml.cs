// Copyright (c) Richasy. All rights reserved.

using System;
using Bili.Models.App.Other;
using Bili.ViewModels.Uwp.Community;

namespace Bili.App.Pages.Desktop
{
    /// <summary>
    /// 动态页面.
    /// </summary>
    public sealed partial class DynamicPage : DynamicPageBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicPage"/> class.
        /// </summary>
        public DynamicPage() => InitializeComponent();

        private void OnHeaderNavItemInvoked(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewItemInvokedEventArgs args)
        {
            var data = args.InvokedItem as DynamicHeader;
            if (data != ViewModel.CurrentHeader)
            {
                ViewModel.SelectHeaderCommand.Execute(data).Subscribe();
            }
        }
    }

    /// <summary>
    /// <see cref="DynamicPage"/> 的基类.
    /// </summary>
    public class DynamicPageBase : AppPage<DynamicPageViewModel>
    {
    }
}
