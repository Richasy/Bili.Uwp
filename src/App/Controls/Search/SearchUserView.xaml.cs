// Copyright (c) Richasy. All rights reserved.

using Windows.UI.Xaml;

namespace Bili.App.Controls
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
            InitializeComponent();
        }

        private async void OnUserRefreshButtonClickAsync(object sender, RoutedEventArgs e)
        {
            await ViewModel.UserModule.InitializeRequestAsync();
        }

        private void OnUserCardClick(object sender, System.EventArgs e)
        {
            new UserSpaceView().Show((sender as UserSlimCard).ViewModel.Id.ToString());
        }
    }
}
