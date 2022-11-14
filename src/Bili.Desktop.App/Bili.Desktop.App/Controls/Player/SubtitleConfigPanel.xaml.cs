// Copyright (c) Richasy. All rights reserved.

using Bili.Models.Data.Player;
using Bili.ViewModels.Interfaces.Common;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Bili.Desktop.App.Controls.Player
{
    /// <summary>
    /// 字幕配置面板.
    /// </summary>
    public sealed partial class SubtitleConfigPanel : UserControl
    {
        /// <summary>
        /// <see cref="ViewModel"/> 的依赖属性.
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel), typeof(ISubtitleModuleViewModel), typeof(SubtitleConfigPanel), new PropertyMetadata(default));

        /// <summary>
        /// Initializes a new instance of the <see cref="SubtitleConfigPanel"/> class.
        /// </summary>
        public SubtitleConfigPanel() => InitializeComponent();

        /// <summary>
        /// 视图模型.
        /// </summary>
        public ISubtitleModuleViewModel ViewModel
        {
            get { return (ISubtitleModuleViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        private void OnMetaItemClick(object sender, ItemClickEventArgs e)
        {
            var data = e.ClickedItem as SubtitleMeta;
            if (ViewModel.CurrentMeta != data)
            {
                ViewModel.ChangeMetaCommand.Execute(data);
            }
        }
    }
}
