// Copyright (c) Richasy. All rights reserved.

using System.Threading.Tasks;
using Richasy.Bili.App.Pages;
using Richasy.Bili.ViewModels.Uwp;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Richasy.Bili.App.Controls
{
    /// <summary>
    /// PGC播放列表视图.
    /// </summary>
    public sealed partial class PgcPlayListView : UserControl
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
            this.InitializeComponent();
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
            Container.IsOpen = true;
            ((Window.Current.Content as Frame).Content as RootPage).ShowOnHolder(this);
            await ViewModel.InitializeAsync(listId);
        }

        private void OnContainerClosed(Microsoft.UI.Xaml.Controls.TeachingTip sender, Microsoft.UI.Xaml.Controls.TeachingTipClosedEventArgs args)
        {
            ((Window.Current.Content as Frame).Content as RootPage).ClearHolder();
        }

        private void OnItemClick(object sender, SeasonViewModel e)
        {
            this.Container.IsOpen = false;
        }

        private async void OnRefreshButtonClickAsync(object sender, RoutedEventArgs e)
        {
            await ViewModel.InitializeAsync(ViewModel.Id, isRefresh: true);
        }
    }
}
