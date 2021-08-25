// Copyright (c) Richasy. All rights reserved.

using System;
using Richasy.Bili.ViewModels.Uwp;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Richasy.Bili.App.Controls
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
            DependencyProperty.Register(nameof(ViewModel), typeof(AccountViewModel), typeof(AccountAvatar), new PropertyMetadata(AccountViewModel.Instance));

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountPanel"/> class.
        /// </summary>
        public AccountPanel()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 请求关闭作为容器的Flyout.
        /// </summary>
        public event EventHandler RequestCloseFlyout;

        /// <summary>
        /// 账户视图模型.
        /// </summary>
        public AccountViewModel ViewModel
        {
            get { return (AccountViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        private void OnDynamicButtonClick(object sender, RoutedEventArgs e)
        {
            RequestCloseFlyout?.Invoke(this, EventArgs.Empty);
        }

        private async void OnFollowButtonClickAsync(object sender, RoutedEventArgs e)
        {
            await AppViewModel.Instance.EnterRelatedUserViewAsync(Models.Enums.App.RelatedUserType.Follows, ViewModel.Mid.Value, ViewModel.DisplayName);
            RequestCloseFlyout?.Invoke(this, EventArgs.Empty);
        }

        private async void OnFollowerButtonClickAsync(object sender, RoutedEventArgs e)
        {
            await AppViewModel.Instance.EnterRelatedUserViewAsync(Models.Enums.App.RelatedUserType.Fans, ViewModel.Mid.Value, ViewModel.DisplayName);
            RequestCloseFlyout?.Invoke(this, EventArgs.Empty);
        }
    }
}
