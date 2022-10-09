// Copyright (c) Richasy. All rights reserved.

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Bili.App.Controls
{
    /// <summary>
    /// 响应式用户控件基类.
    /// </summary>
    /// <typeparam name="TViewModel">控件对应的视图模型.</typeparam>
    public class ReactiveUserControl<TViewModel> : UserControl
        where TViewModel : class
    {
        /// <summary>
        /// <see cref="ViewModel"/> 的依赖属性.
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty
                .Register(nameof(ViewModel), typeof(TViewModel), typeof(ReactiveUserControl<TViewModel>), new PropertyMetadata(default, new PropertyChangedCallback((dp, args) =>
                {
                    var instance = dp as ReactiveUserControl<TViewModel>;
                    instance.OnViewModelChanged(args);
                })));

        /// <summary>
        /// 视图模型.
        /// </summary>
        public TViewModel ViewModel
        {
            get => (TViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        /// <summary>
        /// 当 <see cref="ViewModel"/> 改变时调用该方法，可由派生类重写.
        /// </summary>
        /// <param name="e">依赖属性更改事件参数.</param>
        internal virtual void OnViewModelChanged(DependencyPropertyChangedEventArgs e)
        {
            // 什么都不做.
        }
    }
}
