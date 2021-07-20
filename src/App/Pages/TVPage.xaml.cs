// Copyright (c) Richasy. All rights reserved.

using Richasy.Bili.Models.Enums;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace Richasy.Bili.App.Pages
{
    /// <summary>
    /// 电视剧页面.
    /// </summary>
    public sealed partial class TVPage : Page
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TVPage"/> class.
        /// </summary>
        public TVPage()
        {
            this.InitializeComponent();
            this.Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (RootFrame.Content == null)
            {
                RootFrame.Navigate(typeof(FeedPage), PgcType.TV, new SuppressNavigationTransitionInfo());
            }
        }
    }
}
