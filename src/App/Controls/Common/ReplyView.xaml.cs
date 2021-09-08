// Copyright (c) Richasy. All rights reserved.

using System.Threading.Tasks;
using Bilibili.Main.Community.Reply.V1;
using Richasy.Bili.ViewModels.Uwp;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Richasy.Bili.App.Controls
{
    /// <summary>
    /// 评论回复视图.
    /// </summary>
    public sealed partial class ReplyView : UserControl
    {
        /// <summary>
        /// <see cref="ViewModel"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel), typeof(ReplyViewModelBase), typeof(ReplyView), new PropertyMetadata(null));

        /// <summary>
        /// Initializes a new instance of the <see cref="ReplyView"/> class.
        /// </summary>
        public ReplyView()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 依赖属性.
        /// </summary>
        public ReplyViewModelBase ViewModel
        {
            get { return (ReplyViewModelBase)GetValue(ViewModelProperty); }
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
                await ViewModel.InitializeRequestAsync();
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
                    await ViewModel.InitializeRequestAsync();
                }
            }
        }

        private async void OnReplyRefreshButtonClickAsync(object sender, RoutedEventArgs e)
        {
            await ViewModel.InitializeRequestAsync();
        }
    }
}
