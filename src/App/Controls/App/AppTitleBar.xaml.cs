﻿// Copyright (c) Richasy. All rights reserved.

using System.ComponentModel;
using Bili.ViewModels.Uwp.Core;
using ReactiveUI;
using Splat;
using Windows.UI.Xaml;

namespace Bili.App.Controls
{
    /// <summary>
    /// 应用标题栏.
    /// </summary>
    public sealed partial class AppTitleBar : AppTitleBarBase
    {
        private readonly NavigationViewModel _navigationViewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppTitleBar"/> class.
        /// </summary>
        public AppTitleBar()
        {
            InitializeComponent();
            ViewModel = Locator.Current.GetService<AppViewModel>();
            _navigationViewModel = Locator.Current.GetService<NavigationViewModel>();
            ViewModel.PropertyChanged += OnViewModelPropertyChanged;
            Loaded += OnLoaded;
        }

        private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ViewModel.IsShowTitleBar))
            {
                ChangeTitleBarVisibility();
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
            => ChangeTitleBarVisibility();

        private void OnMenuButtonClick(object sender, RoutedEventArgs e)
            => ViewModel.IsNavigatePaneOpen = !ViewModel.IsNavigatePaneOpen;

        private void ChangeTitleBarVisibility()
        {
            if (ViewModel.IsShowTitleBar)
            {
                Height = 48;
                Window.Current.SetTitleBar(TitleBarHost);
            }
            else
            {
                Height = 0;
                Window.Current.SetTitleBar(null);
            }
        }
    }

    /// <summary>
    /// <see cref="AppTitleBar"/> 的基类.
    /// </summary>
    public class AppTitleBarBase : ReactiveUserControl<AppViewModel>
    {
    }
}
