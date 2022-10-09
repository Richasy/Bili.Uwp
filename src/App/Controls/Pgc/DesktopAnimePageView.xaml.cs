// Copyright (c) Richasy. All rights reserved.

using Bili.DI.Container;
using Bili.Models.Data.Community;
using Bili.ViewModels.Interfaces.Core;
using Bili.ViewModels.Interfaces.Pgc;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Bili.App.Controls.Pgc
{
    /// <summary>
    /// 动漫视图.
    /// </summary>
    public sealed partial class DesktopAnimePageView : UserControl
    {
        /// <summary>
        /// <see cref="ViewModel"/> 的依赖属性.
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel), typeof(IAnimePageViewModel), typeof(DesktopAnimePageView), new PropertyMetadata(default, new PropertyChangedCallback(OnViewModelChanged)));

        /// <summary>
        /// Initializes a new instance of the <see cref="DesktopAnimePageView"/> class.
        /// </summary>
        public DesktopAnimePageView()
            => InitializeComponent();

        /// <summary>
        /// 视图模型.
        /// </summary>
        public IAnimePageViewModel ViewModel
        {
            get { return (IAnimePageViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        /// <summary>
        /// 核心视图模型.
        /// </summary>
        public IAppViewModel CoreViewModel { get; } = Locator.Instance.GetService<IAppViewModel>();

        private static void OnViewModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as DesktopAnimePageView;
            instance.DataContext = e.NewValue;
        }

        private void OnRootNavViewItemInvoked(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewItemInvokedEventArgs args)
        {
            var data = args.InvokedItem as Partition;
            ContentScrollViewer.ChangeView(0, 0, 1);
            ViewModel.SelectPartitionCommand.ExecuteAsync(data);
        }
    }
}
