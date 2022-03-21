// Copyright (c) Richasy. All rights reserved.

using System;
using Richasy.Bili.ViewModels.Uwp;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Richasy.Bili.App.Pages
{
    /// <summary>
    /// PGC索引页面.
    /// </summary>
    public sealed partial class PgcIndexPage : AppPage
    {
        /// <summary>
        /// <see cref="ViewModel"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel), typeof(PgcViewModelBase), typeof(PgcIndexPage), new PropertyMetadata(null));

        /// <summary>
        /// Initializes a new instance of the <see cref="PgcIndexPage"/> class.
        /// </summary>
        public PgcIndexPage()
        {
            InitializeComponent();
            Loaded += OnLoadedAsync;
        }

        /// <summary>
        /// 视图模型.
        /// </summary>
        public PgcViewModelBase ViewModel
        {
            get { return (PgcViewModelBase)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        /// <inheritdoc/>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is PgcViewModelBase vm)
            {
                ViewModel = vm;
            }
        }

        private async void OnLoadedAsync(object sender, RoutedEventArgs e)
        {
            if (ViewModel.FilterCollection.Count == 0)
            {
                await ViewModel.LoadIndexAsync();
            }
        }

        private async void OnViewRequestLoadMoreAsync(object sender, EventArgs e)
        {
            await ViewModel.DeltaRequestIndexAsync();
        }

        private async void OnIndexRefreshButtonClickAsync(object sender, RoutedEventArgs e)
        {
            await ViewModel.LoadIndexAsync();
        }

        private async void OnConditionChangedAsync(object sender, SelectionChangedEventArgs e)
        {
            if (ViewModel.IsIndexRequested)
            {
                await ViewModel.LoadIndexAsync();
            }
        }
    }
}
