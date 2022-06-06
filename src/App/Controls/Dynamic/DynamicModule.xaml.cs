using Bili.ViewModels.Uwp.Base;
using Bili.ViewModels.Uwp.Core;
using Splat;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Bili.App.Controls.Dynamic
{
    /// <summary>
    /// 动态模块.
    /// </summary>
    public sealed partial class DynamicModule : UserControl
    {
        /// <summary>
        /// <see cref="ViewModel"/> 的依赖属性.
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel), typeof(DynamicModuleViewModelBase), typeof(DynamicModule), new PropertyMetadata(default));

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicModule"/> class.
        /// </summary>
        public DynamicModule() => InitializeComponent();

        /// <summary>
        /// 核心视图模型.
        /// </summary>
        public AppViewModel CoreViewModel { get; } = Splat.Locator.Current.GetService<AppViewModel>();

        /// <summary>
        /// 视图模型.
        /// </summary>
        public DynamicModuleViewModelBase ViewModel
        {
            get { return (DynamicModuleViewModelBase)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }
    }
}
