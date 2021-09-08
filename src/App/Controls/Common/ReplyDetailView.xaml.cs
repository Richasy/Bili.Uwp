// Copyright (c) Richasy. All rights reserved.

using System.Threading.Tasks;
using Bilibili.Main.Community.Reply.V1;
using Richasy.Bili.App.Pages;
using Richasy.Bili.Models.Enums.Bili;
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
            ReplyView.ViewModel = ViewModel;
        }

        /// <summary>
        /// 视图模型.
        /// </summary>
        public ReplyDetailViewModel ViewModel
        {
            get { return (ReplyDetailViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        /// <summary>
        /// 显示.
        /// </summary>
        /// <param name="rootReply">根评论.</param>
        /// <returns><see cref="Task"/>.</returns>
        public async Task ShowAsync(ReplyInfo rootReply)
        {
            Container.IsOpen = true;
            ((Window.Current.Content as Frame).Content as RootPage).ShowOnHolder(this);
            if (ViewModel == null || ViewModel.RootReply?.Id != rootReply.Id)
            {
                ViewModel.SetRootReply(rootReply);
                await ReplyView.CheckInitializeAsync();
            }
        }

        private void OnContainerClosed(Microsoft.UI.Xaml.Controls.TeachingTip sender, Microsoft.UI.Xaml.Controls.TeachingTipClosedEventArgs args)
        {
            ((Window.Current.Content as Frame).Content as RootPage).ClearHolder();
        }
    }
}
