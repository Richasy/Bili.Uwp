// Copyright (c) Richasy. All rights reserved.

using System.Threading.Tasks;
using Bilibili.Main.Community.Reply.V1;
using Richasy.Bili.ViewModels.Uwp;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Richasy.Bili.App.Controls.Player.Related
{
    /// <summary>
    /// 评论视图.
    /// </summary>
    public sealed partial class ReplyView : UserControl
    {
        /// <summary>
        /// <see cref="ViewModel"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel), typeof(ReplyModuleViewModel), typeof(ReplyView), new PropertyMetadata(ReplyModuleViewModel.Instance));

        /// <summary>
        /// Initializes a new instance of the <see cref="ReplyView"/> class.
        /// </summary>
        public ReplyView()
        {
            this.InitializeComponent();
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
        public async Task CheckInitializeAsync()
        {
            OrderTypeComboBox.SelectedIndex = ViewModel.CurrentMode == Mode.MainListHot ? 0 : 1;
            if (!ViewModel.IsRequested)
            {
                await ViewModel.InitializeRequstAsync();
            }
        }

        private async void OnReplyRequestLoadMoreAsync(object sender, System.EventArgs e)
        {
            await ViewModel.RequestDataAsync();
        }

        private async void OnOrderTypeSelectionChangedAsync(object sender, SelectionChangedEventArgs e)
        {
            if (ViewModel.IsRequested)
            {
                var item = OrderTypeComboBox.SelectedItem;
                Mode mode;
                if (item == HotItem)
                {
                    mode = Mode.MainListHot;
                }
                else
                {
                    mode = Mode.MainListTime;
                }

                if (ViewModel.CurrentMode != mode)
                {
                    ViewModel.CurrentMode = mode;
                    await ViewModel.InitializeRequstAsync();
                }
            }
        }

        private async void OnReplyRefreshButtonClickAsync(object sender, RoutedEventArgs e)
        {
            await ViewModel.InitializeRequstAsync();
        }
    }
}
