// Copyright (c) Richasy. All rights reserved.

using System;
using System.ComponentModel;
using Bili.Models.Data.Local;
using Bili.ViewModels.Interfaces.Core;
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
        private readonly IRecordViewModel _recordViewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppTitleBar"/> class.
        /// </summary>
        public AppTitleBar()
        {
            InitializeComponent();
            ViewModel = Locator.Current.GetService<IAppViewModel>();
            _navigationViewModel = Locator.Current.GetService<NavigationViewModel>();
            _recordViewModel = Locator.Current.GetService<IRecordViewModel>();
            (ViewModel as ReactiveObject).PropertyChanged += OnViewModelPropertyChanged;
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

        private void OnRemoveRecordButtonClick(object sender, RoutedEventArgs e)
        {
            var context = (sender as FrameworkElement).DataContext as PlayRecord;
            _recordViewModel.RemovePlayRecordCommand.Execute(context).Subscribe();
        }

        private void OnPlayRecordItemClick(object sender, Windows.UI.Xaml.Controls.ItemClickEventArgs e)
        {
            var context = e.ClickedItem as PlayRecord;
            _navigationViewModel.NavigateToPlayView(context.Snapshot);
        }

        private void OnClearRecordsButtonClick(object sender, RoutedEventArgs e)
            => RecordsFlyout.Hide();
    }

    /// <summary>
    /// <see cref="AppTitleBar"/> 的基类.
    /// </summary>
    public class AppTitleBarBase : ReactiveUserControl<IAppViewModel>
    {
    }
}
