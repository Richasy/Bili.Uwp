// Copyright (c) Richasy. All rights reserved.

using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Bili.App.Pages.Desktop;
using Bili.ViewModels.Interfaces;
using Bili.ViewModels.Uwp;
using Splat;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Bili.App.Controls
{
    /// <summary>
    /// 应用标题栏.
    /// </summary>
    public sealed partial class AppTitleBar : UserControl
    {
        /// <summary>
        /// <see cref="ViewModel"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel), typeof(AppViewModel), typeof(AppTitleBar), new PropertyMetadata(AppViewModel.Instance));

        private readonly INavigationViewModel _navigationViewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppTitleBar"/> class.
        /// </summary>
        public AppTitleBar()
        {
            InitializeComponent();
            _navigationViewModel = Splat.Locator.Current.GetService<INavigationViewModel>();
            Loaded += OnLoaded;
        }

        /// <summary>
        /// 视图模型.
        /// </summary>
        public AppViewModel ViewModel
        {
            get { return (AppViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        private void OnLoaded(object sender, RoutedEventArgs e) => Window.Current.SetTitleBar(TitleBarHost);

        private void OnMenuButtonClick(object sender, RoutedEventArgs e)
            => ViewModel.IsNavigatePaneOpen = !ViewModel.IsNavigatePaneOpen;

        private async void OnHomeButtonClickAsync(object sender, RoutedEventArgs e)
        {
            await PlayerViewModel.Instance.BackToHomeAsync();
            _navigationViewModel.NavigateToMainView(_navigationViewModel.MainViewId);
        }
    }
}
