// Copyright (c) Richasy. All rights reserved.

using Bili.ViewModels.Interfaces.Core;
using Bili.ViewModels.Uwp.Base;
using Splat;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Bili.App.Controls.Community
{
    /// <summary>
    /// 关系页面视图.
    /// </summary>
    public sealed partial class RelationPageView : UserControl
    {
        /// <summary>
        /// <see cref="ViewModel"/> 的依赖属性.
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel), typeof(RelationPageViewModelBase), typeof(RelationPageView), new PropertyMetadata(default));

        private readonly IAppViewModel _appViewModel = Locator.Current.GetService<IAppViewModel>();

        /// <summary>
        /// Initializes a new instance of the <see cref="RelationPageView"/> class.
        /// </summary>
        public RelationPageView() => InitializeComponent();

        /// <summary>
        /// 视图模型.
        /// </summary>
        public RelationPageViewModelBase ViewModel
        {
            get { return (RelationPageViewModelBase)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }
    }
}
