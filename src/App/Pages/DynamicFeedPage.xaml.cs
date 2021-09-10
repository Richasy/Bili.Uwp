// Copyright (c) Richasy. All rights reserved.

using Richasy.Bili.ViewModels.Uwp;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Richasy.Bili.App.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页.
    /// </summary>
    public sealed partial class DynamicFeedPage : Page
    {
        /// <summary>
        /// <see cref="ViewModel"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel), typeof(DynamicModuleViewModel), typeof(DynamicFeedPage), new PropertyMetadata(DynamicModuleViewModel.Instance));

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicFeedPage"/> class.
        /// </summary>
        public DynamicFeedPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 视图模型.
        /// </summary>
        public DynamicModuleViewModel ViewModel
        {
            get { return (DynamicModuleViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        private void OnViewRequestLoadMoreAsync(object sender, System.EventArgs e)
        {
        }

        private void OnDynamicRefreshButtonClickAsync(object sender, RoutedEventArgs e)
        {
        }
    }
}
