// Copyright (c) Richasy. All rights reserved.

using System;
using Bili.ViewModels.Interfaces.Account;
using Bili.ViewModels.Interfaces.Core;
using Splat;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Bili.App.Controls
{
    /// <summary>
    /// 账户面板.
    /// </summary>
    public sealed partial class AccountPanel : UserControl
    {
        /// <summary>
        /// <see cref="ViewModel"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel), typeof(IAccountViewModel), typeof(AccountAvatar), new PropertyMetadata(Locator.Current.GetService<IAccountViewModel>()));

        private readonly INavigationViewModel _navigationViewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountPanel"/> class.
        /// </summary>
        public AccountPanel()
        {
            InitializeComponent();
            _navigationViewModel = Locator.Current.GetService<INavigationViewModel>();
        }

        /// <summary>
        /// 请求关闭作为容器的Flyout.
        /// </summary>
        public event EventHandler RequestCloseFlyout;

        /// <summary>
        /// 账户视图模型.
        /// </summary>
        public IAccountViewModel ViewModel
        {
            get { return (IAccountViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        private void OnDynamicButtonClick(object sender, RoutedEventArgs e)
        {
            var user = ViewModel.AccountInformation.User;
            _navigationViewModel.NavigateToSecondaryView(Models.Enums.PageIds.UserSpace, user);
            RequestCloseFlyout?.Invoke(this, EventArgs.Empty);
        }

        private void OnFollowButtonClick(object sender, RoutedEventArgs e)
        {
            _navigationViewModel.NavigateToSecondaryView(Models.Enums.PageIds.MyFollows);
            RequestCloseFlyout?.Invoke(this, EventArgs.Empty);
        }

        private void OnFollowerButtonClick(object sender, RoutedEventArgs e)
        {
            _navigationViewModel.NavigateToSecondaryView(Models.Enums.PageIds.Fans, ViewModel.AccountInformation.User);
            RequestCloseFlyout?.Invoke(this, EventArgs.Empty);
        }
    }
}
