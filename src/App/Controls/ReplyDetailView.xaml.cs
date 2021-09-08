// Copyright (c) Richasy. All rights reserved.

using Richasy.Bili.ViewModels.Uwp;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Richasy.Bili.App.Controls
{
    /// <summary>
    /// 评论回复详情.
    /// </summary>
    public sealed partial class ReplyDetailView : UserControl
    {
        /// <summary>
        /// <see cref="ViewModel"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel), typeof(ReplyDetailViewModel), typeof(ReplyDetailView), new PropertyMetadata(ReplyDetailViewModel.Instance));

        /// <summary>
        /// Initializes a new instance of the <see cref="ReplyDetailView"/> class.
        /// </summary>
        public ReplyDetailView()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 视图模型.
        /// </summary>
        public ReplyDetailViewModel ViewModel
        {
            get { return (ReplyDetailViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        private void OnContainerClosed(Microsoft.UI.Xaml.Controls.TeachingTip sender, Microsoft.UI.Xaml.Controls.TeachingTipClosedEventArgs args)
        {
        }
    }
}
