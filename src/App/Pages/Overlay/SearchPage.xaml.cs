// Copyright (c) Richasy. All rights reserved.

using Richasy.Bili.ViewModels.Uwp;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Richasy.Bili.App.Pages.Overlay
{
    /// <summary>
    /// 搜索页面.
    /// </summary>
    public sealed partial class SearchPage : Page
    {
        /// <summary>
        /// <see cref="ViewModel"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel), typeof(SearchModuleViewModel), typeof(SearchPage), new PropertyMetadata(SearchModuleViewModel.Instance));

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchPage"/> class.
        /// </summary>
        public SearchPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 视图模型.
        /// </summary>
        public SearchModuleViewModel ViewModel
        {
            get { return (SearchModuleViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }
    }
}
