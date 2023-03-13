// Copyright (c) Richasy. All rights reserved.

using Bili.DI.Container;
using Bili.ViewModels.Interfaces.Workspace;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Bili.Workspace.Pages
{
    /// <summary>
    /// 应用页面基类.
    /// </summary>
    public class PageBase : Page
    {
        /// <summary>
        /// <see cref="CoreViewModel"/> 的依赖属性.
        /// </summary>
        public static readonly DependencyProperty CoreViewModelProperty =
            DependencyProperty.Register(nameof(CoreViewModel), typeof(IWorkspaceViewModel), typeof(PageBase), new PropertyMetadata(default));

        /// <summary>
        /// Initializes a new instance of the <see cref="PageBase"/> class.
        /// </summary>
        public PageBase() => CoreViewModel = Locator.Instance.GetService<IWorkspaceViewModel>();

        /// <summary>
        /// 应用核心视图模型.
        /// </summary>
        public IWorkspaceViewModel CoreViewModel
        {
            get => (IWorkspaceViewModel)GetValue(CoreViewModelProperty);
            set => SetValue(CoreViewModelProperty, value);
        }

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
    public class PageBase<TViewModel> : PageBase
        where TViewModel : class
    {
        /// <summary>
        /// <see cref="ViewModel"/> 的依赖属性.
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel), typeof(TViewModel), typeof(PageBase), new PropertyMetadata(default));

        /// <summary>
        /// Initializes a new instance of the <see cref="PageBase{TViewModel}"/> class.
        /// </summary>
        public PageBase()
        {
            ViewModel = Locator.Instance.GetService<TViewModel>();
            DataContext = ViewModel;
            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
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
        public override object GetViewModel() => ViewModel;

        /// <summary>
        /// 在页面加载完成后调用.
        /// </summary>
        protected virtual void OnPageLoaded()
        {
        }

        /// <summary>
        /// 在页面卸载完成后调用.
        /// </summary>
        protected virtual void OnPageUnloaded()
        {
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
            => OnPageLoaded();

        private void OnUnloaded(object sender, RoutedEventArgs e)
            => OnPageUnloaded();
    }
}
