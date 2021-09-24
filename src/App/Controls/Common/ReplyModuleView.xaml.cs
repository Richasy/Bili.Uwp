// Copyright (c) GodLeaveMe. All rights reserved.

using System.Threading.Tasks;
using Bilibili.Main.Community.Reply.V1;
using Richasy.Bili.ViewModels.Uwp;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Richasy.Bili.App.Controls
{
    /// <summary>
    /// 回复模块视图.
    /// </summary>
    public sealed partial class ReplyModuleView : UserControl
    {
        /// <summary>
        /// <see cref="CanShowBackButton"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty CanShowBackButtonProperty =
            DependencyProperty.Register(nameof(CanShowBackButton), typeof(bool), typeof(ReplyModuleView), new PropertyMetadata(false));

        /// <summary>
        /// Initializes a new instance of the <see cref="ReplyModuleView"/> class.
        /// </summary>
        public ReplyModuleView()
        {
            this.InitializeComponent();
            MainView.ViewModel = ReplyModuleViewModel.Instance;
            DetailView.ViewModel = ReplyDetailViewModel.Instance;
        }

        /// <summary>
        /// 是否可以显示返回主列表的按钮.
        /// </summary>
        public bool CanShowBackButton
        {
            get { return (bool)GetValue(CanShowBackButtonProperty); }
            set { SetValue(CanShowBackButtonProperty, value); }
        }

        /// <summary>
        /// 初始化.
        /// </summary>
        /// <param name="item">数据.</param>
        /// <returns><see cref="Task"/>.</returns>
        public async Task InitializeAsync(object item)
        {
            if (item is ReplyModuleViewModel)
            {
                MainContainer.Visibility = Visibility.Visible;
                DetailContainer.Visibility = Visibility.Collapsed;
                CanShowBackButton = true;
                await MainView.CheckInitializeAsync();
            }
            else if (item is ReplyInfo info)
            {
                MainContainer.Visibility = Visibility.Collapsed;
                DetailContainer.Visibility = Visibility.Visible;
                CanShowBackButton = false;
                await DetailView.CheckInitializeAsync();
            }
        }

        private void OnBackButtonClick(object sender, RoutedEventArgs e)
        {
            DetailContainer.Visibility = Visibility.Collapsed;
            MainContainer.Visibility = Visibility.Visible;
        }

        private async void OnRequestDetailViewAsync(object sender, ReplyInfo e)
        {
            MainContainer.Visibility = Visibility.Collapsed;
            DetailContainer.Visibility = Visibility.Visible;
            RootReplyItem.Data = e;
            ReplyDetailViewModel.Instance.SetRootReply(e);
            await DetailView.CheckInitializeAsync();
        }
    }
}
