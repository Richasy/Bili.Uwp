// Copyright (c) GodLeaveMe. All rights reserved.

using System.ComponentModel;
using Richasy.Bili.ViewModels.Uwp;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Richasy.Bili.App.Controls
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

        /// <summary>
        /// Initializes a new instance of the <see cref="AppTitleBar"/> class.
        /// </summary>
        public AppTitleBar()
        {
            this.InitializeComponent();
            this.Loaded += OnLoaded;
        }

        /// <summary>
        /// 视图模型.
        /// </summary>
        public AppViewModel ViewModel
        {
            get { return (AppViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Window.Current.SetTitleBar(TitleBarHost);
            CheckBackButtonVisibility();
            ViewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ViewModel.IsOpenPlayer) || e.PropertyName == nameof(ViewModel.IsShowOverlay))
            {
                CheckBackButtonVisibility();
            }
        }

        private void OnMenuButtonClick(object sender, RoutedEventArgs e)
        {
            ViewModel.IsNavigatePaneOpen = !ViewModel.IsNavigatePaneOpen;
        }

        private void OnBackButtonClick(object sender, RoutedEventArgs e)
        {
            if (ViewModel.IsOpenPlayer)
            {
                ViewModel.IsOpenPlayer = false;
            }
            else if (ViewModel.IsShowOverlay)
            {
                ViewModel.SetMainContentId(ViewModel.CurrentMainContentId);
            }
        }

        private void CheckBackButtonVisibility()
        {
            BackButton.Visibility = (ViewModel.IsShowOverlay || ViewModel.IsOpenPlayer) ?
                Visibility.Visible : Visibility.Collapsed;
        }
    }
}
