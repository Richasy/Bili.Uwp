// Copyright (c) Richasy. All rights reserved.

using Bili.ViewModels.Uwp;
using ReactiveUI;
using Splat;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Bili.App.Pages.Desktop
{
    /// <summary>
    /// 应用页面基类.
    /// </summary>
    public class AppPage : Page
    {
        /// <summary>
        /// 核心视图模型.
        /// </summary>
        protected AppViewModel CoreViewModel { get; } = Splat.Locator.Current.GetService<AppViewModel>();

        /// <summary>
        /// 获取页面注册的视图模型.
        /// </summary>
        /// <returns>视图模型，如果没有则返回<c>null</c>.</returns>
        public virtual object GetViewModel() => null;
    }

    /// <summary>
    /// 带视图模型的应用页面基类.
    /// </summary>
    /// <typeparam name="TViewModel">视图模型.</typeparam>
    public class AppPage<TViewModel> : AppPage, IViewFor<TViewModel>
        where TViewModel : class
    {
        /// <summary>
        /// <see cref="ViewModel"/> 的依赖属性.
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel), typeof(TViewModel), typeof(AppPage<TViewModel>), new PropertyMetadata(default));

        /// <summary>
        /// Initializes a new instance of the <see cref="AppPage{TViewModel}"/> class.
        /// </summary>
        public AppPage()
        {
            ViewModel = Splat.Locator.Current.GetService<TViewModel>();
            DataContext = ViewModel;
        }

        /// <summary>
        /// 页面的视图模型.
        /// </summary>
        public TViewModel ViewModel
        {
            get { return (TViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        /// <inheritdoc/>
        object IViewFor.ViewModel { get => ViewModel; set => ViewModel = (TViewModel)value; }

        /// <inheritdoc/>
        public override object GetViewModel() => ViewModel;
    }
}
