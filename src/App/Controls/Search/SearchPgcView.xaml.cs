// Copyright (c) GodLeaveMe. All rights reserved.

using Richasy.Bili.ViewModels.Uwp;
using Windows.UI.Xaml;

namespace Richasy.Bili.App.Controls
{
    /// <summary>
    /// PGC搜索视图.
    /// </summary>
    public sealed partial class SearchPgcView : SearchComponent
    {
        /// <summary>
        /// <see cref="SubModuleViewModel"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty SubModuleViewModelProperty =
            DependencyProperty.Register(nameof(SubModuleViewModel), typeof(SearchSubModuleViewModel), typeof(SearchPgcView), new PropertyMetadata(null));

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchPgcView"/> class.
        /// </summary>
        public SearchPgcView()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 子模块视图模型.
        /// </summary>
        public SearchSubModuleViewModel SubModuleViewModel
        {
            get { return (SearchSubModuleViewModel)GetValue(SubModuleViewModelProperty); }
            set { SetValue(SubModuleViewModelProperty, value); }
        }

        private async void OnPgcRefreshButtonClickAsync(object sender, RoutedEventArgs e)
        {
            await SubModuleViewModel.InitializeRequestAsync();
        }
    }
}
