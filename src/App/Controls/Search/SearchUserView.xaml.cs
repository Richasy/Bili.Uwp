// Copyright (c) Richasy. All rights reserved.

using Windows.UI.Xaml;

namespace Richasy.Bili.App.Controls
{
    /// <summary>
    /// 搜索用户视图.
    /// </summary>
    public sealed partial class SearchUserView : SearchComponent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SearchUserView"/> class.
        /// </summary>
        public SearchUserView()
        {
            this.InitializeComponent();
        }

        private async void OnUserRefreshButtonClickAsync(object sender, RoutedEventArgs e)
        {
            await ViewModel.UserModule.InitializeRequestAsync();
        }

        private async void OnUserItemClickAsync(object sender, RoutedEventArgs e)
        {
            await UserView.Instance.ShowAsync((int)(sender as FrameworkElement).Tag);
        }
    }
}
