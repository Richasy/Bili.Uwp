// Copyright (c) Richasy. All rights reserved.

using System.Threading.Tasks;
using Bili.App.Controls;
using Bili.Models.Enums;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace Bili.App.Pages
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
            InitializeComponent();
            Loaded += OnLoaded;
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
