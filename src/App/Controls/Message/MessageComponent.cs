// Copyright (c) Richasy. All rights reserved.

using Bili.ViewModels.Uwp;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Bili.App.Controls
{
    /// <summary>
    /// 消息组件.
    /// </summary>
    public class MessageComponent : UserControl
    {
        /// <summary>
        /// <see cref="ViewModel"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel), typeof(MessageModuleViewModel), typeof(MessageComponent), new PropertyMetadata(MessageModuleViewModel.Instance));

        /// <summary>
        /// 视图模型.
        /// </summary>
        public MessageModuleViewModel ViewModel
        {
            get { return (MessageModuleViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        /// <summary>
        /// 当请求更多时发生.
        /// </summary>
        /// <param name="sender">触发事件的元素.</param>
        /// <param name="e">事件参数.</param>
        protected async void OnRequestLoadMoreAsync(object sender, System.EventArgs e)
        {
            await ViewModel.RequestDataAsync();
        }
    }
}
