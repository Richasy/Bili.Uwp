// Copyright (c) Richasy. All rights reserved.

using System.Threading.Tasks;
using Bili.ViewModels.Uwp;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Bili.App.Controls.Player.Related
{
    /// <summary>
    /// 评论视图.
    /// </summary>
    public sealed partial class PlayerReplyView : UserControl
    {
        /// <summary>
        /// <see cref="ViewModel"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel), typeof(ReplyModuleViewModel), typeof(PlayerReplyView), new PropertyMetadata(ReplyModuleViewModel.Instance));

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerReplyView"/> class.
        /// </summary>
        public PlayerReplyView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 视图模型.
        /// </summary>
        public ReplyModuleViewModel ViewModel
        {
            get { return (ReplyModuleViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        /// <summary>
        /// 检查评论初始化.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        public Task CheckInitializeAsync()
        {
            return ReplyView.InitializeAsync(ReplyModuleViewModel.Instance);
        }
    }
}
