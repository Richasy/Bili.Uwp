// Copyright (c) Richasy. All rights reserved.

using Richasy.Bili.ViewModels.Uwp;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Richasy.Bili.App.Pages
{
    /// <summary>
    /// The page is used for default loading.
    /// </summary>
    public sealed partial class RootPage : Page
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RootPage"/> class.
        /// </summary>
        public RootPage()
        {
            this.InitializeComponent();
            this.Loaded += OnLoadedAsync;
        }

        private async void OnLoadedAsync(object sender, RoutedEventArgs e)
        {
            await AccountViewModel.Instance.TrySignInAsync(true);
        }
    }
}
