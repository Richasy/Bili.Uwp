// Copyright (c) Richasy. All rights reserved.

using Bili.ViewModels.Interfaces.Core;
using Bili.ViewModels.Uwp.Base;
using Bili.ViewModels.Uwp.Core;
using Splat;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Bili.App.Controls.Pgc
{
    /// <summary>
    /// PGC 页面视图.
    /// </summary>
    public sealed partial class DesktopPgcPageView : UserControl
    {
        /// <summary>
        /// <see cref="ViewModel"/> 的依赖属性.
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel), typeof(PgcPageViewModelBase), typeof(DesktopPgcPageView), new PropertyMetadata(default, new PropertyChangedCallback(OnViewModelChanged)));

        /// <summary>
        /// Initializes a new instance of the <see cref="DesktopPgcPageView"/> class.
        /// </summary>
        public DesktopPgcPageView() => InitializeComponent();

        /// <summary>
        /// 视图模型.
        /// </summary>
        public PgcPageViewModelBase ViewModel
        {
            get { return (PgcPageViewModelBase)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        /// <summary>
        /// 核心视图模型.
        /// </summary>
        public IAppViewModel CoreViewModel { get; } = Locator.Current.GetService<IAppViewModel>();

        private static void OnViewModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as DesktopPgcPageView;
            instance.DataContext = e.NewValue;
        }
    }
}
