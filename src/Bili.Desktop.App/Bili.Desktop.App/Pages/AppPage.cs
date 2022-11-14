// Copyright (c) Richasy. All rights reserved.

using Bili.DI.Container;
using Bili.ViewModels.Interfaces.Core;
using Microsoft.UI.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;

namespace Bili.Desktop.App.Pages
{
    /// <summary>
    /// 应用页面基类.
    /// </summary>
    public class AppPage : Page
    {
        /// <summary>
        /// <see cref="CoreViewModel"/> 的依赖属性.
        /// </summary>
        public static readonly DependencyProperty CoreViewModelProperty =
            DependencyProperty.Register(nameof(CoreViewModel), typeof(IAppViewModel), typeof(IAppViewModel), new PropertyMetadata(default));

        /// <summary>
        /// Initializes a new instance of the <see cref="AppPage"/> class.
        /// </summary>
        public AppPage() => CoreViewModel = Locator.Instance.GetService<IAppViewModel>();

        /// <summary>
        /// 应用核心视图模型.
        /// </summary>
        public IAppViewModel CoreViewModel
        {
            get { return (IAppViewModel)GetValue(CoreViewModelProperty); }
            set { SetValue(CoreViewModelProperty, value); }
        }

        /// <summary>
        /// 获取页面注册的视图模型.
        /// </summary>
        /// <returns>视图模型，如果没有则返回<c>null</c>.</returns>
        public virtual object GetViewModel() => null;

        /// <inheritdoc/>
        protected override void OnPointerReleased(PointerRoutedEventArgs e)
        {
            var kind = e.GetCurrentPoint(this).Properties.PointerUpdateKind;
            if (kind == PointerUpdateKind.XButton1Released
                || kind == PointerUpdateKind.MiddleButtonReleased)
            {
                e.Handled = true;
                var navigationVM = Locator.Instance.GetService<INavigationViewModel>();
                if (navigationVM.CanBack)
                {
                    navigationVM.BackCommand.Execute(null);
                }
            }

            base.OnPointerReleased(e);
        }
    }

    /// <summary>
    /// 带视图模型的应用页面基类.
    /// </summary>
    /// <typeparam name="TViewModel">视图模型.</typeparam>
    public class AppPage<TViewModel> : AppPage
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
            ViewModel = Locator.Instance.GetService<TViewModel>();
            DataContext = ViewModel;
            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
            SizeChanged += OnSizeChanged;
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

        /// <summary>
        /// 当页面大小变化时调用.
        /// </summary>
        protected virtual void OnPageSizeChanged(SizeChangedEventArgs args)
        {
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
            => OnPageLoaded();

        private void OnUnloaded(object sender, RoutedEventArgs e)
            => OnPageUnloaded();

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
            => OnPageSizeChanged(e);
    }
}
