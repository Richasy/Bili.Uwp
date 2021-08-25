// Copyright (c) Richasy. All rights reserved.

using System;
using Richasy.Bili.ViewModels.Uwp;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Richasy.Bili.App.Controls
{
    /// <summary>
    /// 用户小卡片.
    /// </summary>
    public sealed partial class UserSlimCard : UserControl
    {
        /// <summary>
        /// <see cref="ViewModel"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel), typeof(UserViewModel), typeof(UserSlimCard), new PropertyMetadata(null));

        /// <summary>
        /// <see cref="LevelVisibility"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty LevelVisibilityProperty =
            DependencyProperty.Register(nameof(LevelVisibility), typeof(Visibility), typeof(UserSlimCard), new PropertyMetadata(Visibility.Visible));

        /// <summary>
        /// Initializes a new instance of the <see cref="UserSlimCard"/> class.
        /// </summary>
        public UserSlimCard()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 当卡片被点击时发生.
        /// </summary>
        public event EventHandler Click;

        /// <summary>
        /// 视图模型.
        /// </summary>
        public UserViewModel ViewModel
        {
            get { return (UserViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        /// <summary>
        /// 是否显示等级.
        /// </summary>
        public Visibility LevelVisibility
        {
            get { return (Visibility)GetValue(LevelVisibilityProperty); }
            set { SetValue(LevelVisibilityProperty, value); }
        }

        private void OnUserItemClickAsync(object sender, RoutedEventArgs e)
        {
            Click?.Invoke(this, EventArgs.Empty);
        }

        private async void OnFollowButtonClickAsync(object sender, RoutedEventArgs e)
        {
            await ViewModel.ToggleFollowStateAsync();
        }
    }
}
