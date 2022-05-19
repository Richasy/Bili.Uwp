// Copyright (c) Richasy. All rights reserved.

using System.Threading.Tasks;
using Bili.App.Controls;
using Bili.Models.Enums;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace Bili.App.Pages.Desktop
{
    /// <summary>
    /// 纪录片页面.
    /// </summary>
    public sealed partial class DocumentaryPage : Page, IRefreshPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentaryPage"/> class.
        /// </summary>
        public DocumentaryPage()
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
                RootFrame.Navigate(typeof(FeedPage), PgcType.Documentary, new SuppressNavigationTransitionInfo());
            }
        }
    }
}
