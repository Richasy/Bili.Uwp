// Copyright (c) Richasy. All rights reserved.

using System.Threading.Tasks;
using Richasy.Bili.App.Pages;
using Richasy.Bili.ViewModels.Uwp;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Richasy.Bili.App.Controls
{
    /// <summary>
    /// 用户视图.
    /// </summary>
    public partial class UserView : UserControl
    {
        /// <summary>
        /// <see cref="ViewModel"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel), typeof(UserViewModel), typeof(UserView), new PropertyMetadata(default));

        /// <summary>
        /// Initializes a new instance of the <see cref="UserView"/> class.
        /// </summary>
        public UserView()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 视图模型.
        /// </summary>
        public UserViewModel ViewModel
        {
            get { return (UserViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        /// <summary>
        /// 显示.
        /// </summary>
        /// <param name="userId">用户Id.</param>
        /// <returns><see cref="Task"/>.</returns>
        public async Task ShowAsync(int userId)
        {
            Container.IsOpen = true;
            ((Window.Current.Content as Frame).Content as RootPage).ShowOnHolder(this);
            if (ViewModel == null || ViewModel.Id != userId)
            {
                // 请求用户数据.
                ViewModel = new UserViewModel(userId);
                await ViewModel.InitializeUserDetailAsync();
            }
        }

        /// <summary>
        /// 显示.
        /// </summary>
        /// <param name="vm">用户数据模型.</param>
        /// <returns><see cref="Task"/>.</returns>
        public async Task ShowAsync(UserViewModel vm)
        {
            Container.IsOpen = true;
            ((Window.Current.Content as Frame).Content as RootPage).ShowOnHolder(this);
            if (ViewModel == null || ViewModel.Id != vm.Id)
            {
                // 请求用户数据.
                ViewModel = vm;
                if (!vm.IsRequested)
                {
                    await ViewModel.InitializeUserDetailAsync();
                }
            }
        }

        private void OnRefreshButtonClickAsync(object sender, RoutedEventArgs e)
        {
        }

        private async void OnVideoViewRequestLoadMoreAsync(object sender, System.EventArgs e)
        {
            await ViewModel.DeltaRequestVideoAsync();
        }

        private void OnContainerClosed(Microsoft.UI.Xaml.Controls.TeachingTip sender, Microsoft.UI.Xaml.Controls.TeachingTipClosedEventArgs args)
        {
            ViewModel.Reset();
            ((Window.Current.Content as Frame).Content as RootPage).ClearHolder();
        }

        private void OnVideoItemClick(object sender, VideoViewModel e)
        {
            this.Container.IsOpen = false;
        }

        private async void OnFollowButtonClickAsync(object sender, RoutedEventArgs e)
        {
            await ViewModel.ToggleFollowStateAsync();
        }
    }
}
