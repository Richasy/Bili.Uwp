// Copyright (c) Richasy. All rights reserved.

using System;
using Bili.Models.Data.User;
using Bili.ViewModels.Uwp.Account;
using Splat;
using Windows.UI.Xaml;

namespace Bili.App.Controls
{
    /// <summary>
    /// 用户视图.
    /// </summary>
    public partial class UserSpaceView : CenterPopup
    {
        /// <summary>
        /// <see cref="ViewModel"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel), typeof(UserSpaceViewModel), typeof(UserSpaceView), new PropertyMetadata(default));

        /// <summary>
        /// Initializes a new instance of the <see cref="UserSpaceView"/> class.
        /// </summary>
        public UserSpaceView()
        {
            InitializeComponent();
            ViewModel = Splat.Locator.Current.GetService<UserSpaceViewModel>();
            DataContext = ViewModel;
        }

        /// <summary>
        /// 视图模型.
        /// </summary>
        public UserSpaceViewModel ViewModel
        {
            get { return (UserSpaceViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        /// <summary>
        /// 显示.
        /// </summary>
        /// <param name="userId">用户Id.</param>
        public void Show(string userId)
        {
            var profile = new UserProfile(userId);
            Show(profile);
        }

        /// <summary>
        /// 显示.
        /// </summary>
        /// <param name="profile">用户资料.</param>
        public void Show(UserProfile profile)
        {
            ViewModel.SetUserProfile(profile);
            Show();
        }

        private void OnSearchBoxQuerySubmitted(Windows.UI.Xaml.Controls.AutoSuggestBox sender, Windows.UI.Xaml.Controls.AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (!ViewModel.CanSearch)
            {
                return;
            }

            ViewModel.SearchCommand.Execute().Subscribe();
        }
    }
}
