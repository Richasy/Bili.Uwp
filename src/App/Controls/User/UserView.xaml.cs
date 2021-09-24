// Copyright (c) GodLeaveMe. All rights reserved.

using System;
using System.Threading.Tasks;
using Richasy.Bili.ViewModels.Uwp;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Richasy.Bili.App.Controls
{
    /// <summary>
    /// 用户视图.
    /// </summary>
    public partial class UserView : CenterPopup
    {
        /// <summary>
        /// <see cref="ViewModel"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel), typeof(UserViewModel), typeof(UserView), new PropertyMetadata(default));

        /// <summary>
        /// Initializes a new instance of the <see cref="UserView"/> class.
        /// </summary>
        protected UserView()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 单例.
        /// </summary>
        public static UserView Instance { get; } = new Lazy<UserView>(() => new UserView()).Value;

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
            Container.Show();
            if (ViewModel == null || ViewModel.Id != userId)
            {
                // 请求用户数据.
                ViewModel = new UserViewModel(userId);
                await ViewModel.InitializeUserDetailAsync();
            }
            else
            {
                ViewModel.Active();
            }
        }

        /// <summary>
        /// 显示.
        /// </summary>
        /// <param name="vm">用户数据模型.</param>
        /// <returns><see cref="Task"/>.</returns>
        public async Task ShowAsync(UserViewModel vm)
        {
            Container.Show();
            if (ViewModel == null || ViewModel.Id != vm.Id)
            {
                // 请求用户数据.
                ViewModel = vm;
                if (!vm.IsRequested)
                {
                    await ViewModel.InitializeUserDetailAsync();
                }
                else
                {
                    ViewModel.Active();
                }
            }
            else
            {
                ViewModel.Active();
            }
        }

        private async void OnRefreshButtonClickAsync(object sender, RoutedEventArgs e)
        {
            await ViewModel.InitializeUserDetailAsync();
        }

        private async void OnVideoViewRequestLoadMoreAsync(object sender, System.EventArgs e)
        {
            await ViewModel.DeltaRequestVideoAsync();
        }

        private void OnVideoItemClick(object sender, VideoViewModel e)
        {
            this.Container.Hide();
        }

        private async void OnFollowButtonClickAsync(object sender, RoutedEventArgs e)
        {
            await ViewModel.ToggleFollowStateAsync();
        }

        private async void OnFansButtonClickAsync(object sender, RoutedEventArgs e)
        {
            await AppViewModel.Instance.EnterRelatedUserViewAsync(Models.Enums.App.RelatedUserType.Fans, ViewModel.Id, ViewModel.Name);
            Container.Hide();
        }

        private async void OnFollowUserButtonClickAsync(object sender, RoutedEventArgs e)
        {
            await AppViewModel.Instance.EnterRelatedUserViewAsync(Models.Enums.App.RelatedUserType.Follows, ViewModel.Id, ViewModel.Name);
            Container.Hide();
        }

        private void OnClosed(object sender, System.EventArgs e)
        {
            ViewModel.Deactive();
        }
    }
}
