// Copyright (c) Richasy. All rights reserved.

using Richasy.Bili.ViewModels.Uwp;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Richasy.Bili.App.Pages
{
    /// <summary>
    /// 应用页面基类.
    /// </summary>
    public class AppPage : Page
    {
        /// <summary>
        /// <see cref="CoreViewModel"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty CoreViewModelProperty =
            DependencyProperty.Register(nameof(CoreViewModel), typeof(AppViewModel), typeof(AppPage), new PropertyMetadata(AppViewModel.Instance));

        /// <summary>
        /// 应用视图模型.
        /// </summary>
        public AppViewModel CoreViewModel
        {
            get { return (AppViewModel)GetValue(CoreViewModelProperty); }
            set { SetValue(CoreViewModelProperty, value); }
        }
    }
}
