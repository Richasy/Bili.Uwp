// Copyright (c) Richasy. All rights reserved.

using System;
using Bili.App.Pages.Base;
using Bili.Models.App.Other;

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
}
