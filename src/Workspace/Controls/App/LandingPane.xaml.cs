// Copyright (c) Richasy. All rights reserved.

using System;
using System.ComponentModel;
using Bili.ViewModels.Interfaces.Account;

namespace Bili.Workspace.Controls.App
{
    /// <summary>
    /// 未登录时显示的面板.
    /// </summary>
    public sealed partial class LandingPane : LandingPaneBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LandingPane"/> class.
        /// </summary>
        public LandingPane()
        {
            InitializeComponent();
        }

        private void OnSignButtonClick(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
            => ViewModel.TrySignInCommand.Execute(false);
    }

    /// <summary>
    /// <see cref="LandingPane"/>的基类.
    /// </summary>
    public class LandingPaneBase : ReactiveUserControl<IAccountViewModel>
    {
    }
}
