// Copyright (c) Richasy. All rights reserved.

using Bili.ViewModels.Uwp;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Bili.App.Controls
{
    /// <summary>
    /// 设置区块基类.
    /// </summary>
    public class SettingSectionControl : UserControl
    {
        /// <summary>
        /// <see cref="ViewModel"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel), typeof(SettingViewModel), typeof(ThemeSettingSection), new PropertyMetadata(SettingViewModel.Instance));

        /// <summary>
        /// 设置视图模型.
        /// </summary>
        public SettingViewModel ViewModel
        {
            get { return (SettingViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }
    }
}
