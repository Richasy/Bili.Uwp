// Copyright (c) Richasy. All rights reserved.

using Bili.DI.Container;
using Bili.ViewModels.Interfaces;
using Bili.ViewModels.Interfaces.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Bili.Uwp.App.Controls.Community
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
            DependencyProperty.Register(nameof(ViewModel), typeof(IRelationPageViewModel), typeof(RelationPageView), new PropertyMetadata(default));

        private readonly IAppViewModel _appViewModel = Locator.Instance.GetService<IAppViewModel>();

        /// <summary>
        /// Initializes a new instance of the <see cref="RelationPageView"/> class.
        /// </summary>
        public RelationPageView() => InitializeComponent();

        /// <summary>
        /// 视图模型.
        /// </summary>
        public IRelationPageViewModel ViewModel
        {
            get { return (IRelationPageViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }
    }
}
