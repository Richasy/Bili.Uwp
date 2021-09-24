// Copyright (c) GodLeaveMe. All rights reserved.

using System.Threading.Tasks;
using Richasy.Bili.App.Controls;
using Richasy.Bili.Models.Enums;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace Richasy.Bili.App.Pages
{
    /// <summary>
    /// 电影页面.
    /// </summary>
    public sealed partial class MoviePage : Page, IRefreshPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MoviePage"/> class.
        /// </summary>
        public MoviePage()
        {
            this.InitializeComponent();
            this.Loaded += OnLoaded;
        }

        /// <inheritdoc/>
        public Task RefreshAsync()
            => (RootFrame.Content as IRefreshPage).RefreshAsync();

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (RootFrame.Content == null)
            {
                RootFrame.Navigate(typeof(FeedPage), PgcType.Movie, new SuppressNavigationTransitionInfo());
            }
        }
    }
}
