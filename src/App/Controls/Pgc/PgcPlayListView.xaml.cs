// Copyright (c) Richasy. All rights reserved.

using System.Threading.Tasks;
using Bili.ViewModels.Uwp;
using Windows.UI.Xaml;

namespace Bili.App.Controls
{
    /// <summary>
    /// PGC播放列表视图.
    /// </summary>
    public sealed partial class PgcPlayListView : CenterPopup
    {
        /// <summary>
        /// <see cref="ViewModel"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel), typeof(PgcPlayListViewModel), typeof(PgcPlayListView), new PropertyMetadata(PgcPlayListViewModel.Instance));

        /// <summary>
        /// Initializes a new instance of the <see cref="PgcPlayListView"/> class.
        /// </summary>
        public PgcPlayListView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 视图模型.
        /// </summary>
        public PgcPlayListViewModel ViewModel
        {
            get { return (PgcPlayListViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        /// <summary>
        /// 显示.
        /// </summary>
        /// <param name="listId">用户Id.</param>
        /// <returns><see cref="Task"/>.</returns>
        public async Task ShowAsync(int listId)
        {
            Show();
            await ViewModel.InitializeAsync(listId);
        }

        private void OnItemClick(object sender, SeasonViewModel e)
        {
            Hide();
        }

        private async void OnRefreshButtonClickAsync(object sender, RoutedEventArgs e)
        {
            await ViewModel.InitializeAsync(ViewModel.Id, isRefresh: true);
        }
    }
}
