// Copyright (c) Richasy. All rights reserved.

using System;
using Bili.Models.Data.Player;
using Bili.ViewModels.Interfaces.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Bili.App.Controls.Player
{
    /// <summary>
    /// 互动视频选项面板.
    /// </summary>
    public sealed partial class InteractionChoicePanel : UserControl
    {
        /// <summary>
        /// <see cref="ViewModel"/> 的依赖属性.
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel), typeof(IMediaPlayerViewModel), typeof(InteractionChoicePanel), new PropertyMetadata(default));

        /// <summary>
        /// Initializes a new instance of the <see cref="InteractionChoicePanel"/> class.
        /// </summary>
        public InteractionChoicePanel() => InitializeComponent();

        /// <summary>
        /// 视图模型.
        /// </summary>
        public IMediaPlayerViewModel ViewModel
        {
            get { return (IMediaPlayerViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        private void OnChoiceClick(object sender, RoutedEventArgs e)
        {
            var data = (sender as FrameworkElement).DataContext as InteractionInformation;
            ViewModel.SelectInteractionChoiceCommand.Execute(data).Subscribe();
        }
    }
}
