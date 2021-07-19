// Copyright (c) Richasy. All rights reserved.

using System;
using Richasy.Bili.Models.Enums;
using Richasy.Bili.ViewModels.Uwp;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace Richasy.Bili.App.Pages
{
    /// <summary>
    /// 番剧页面.
    /// </summary>
    public sealed partial class BangumiPage : Page
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BangumiPage"/> class.
        /// </summary>
        public BangumiPage()
        {
            this.InitializeComponent();
            this.Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (RootFrame.Content == null)
            {
                RootFrame.Navigate(typeof(AnimePage), PgcType.Bangumi, new SuppressNavigationTransitionInfo());
            }
        }
    }
}
