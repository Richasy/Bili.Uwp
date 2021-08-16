// Copyright (c) Richasy. All rights reserved.

using Richasy.Bili.ViewModels.Uwp;
using Windows.UI.Xaml.Controls;

namespace Richasy.Bili.App.Controls
{
    /// <summary>
    /// 搜索组件.
    /// </summary>
    public class SearchComponent : UserControl
    {
        /// <summary>
        /// 播放器视图模型.
        /// </summary>
        public SearchModuleViewModel ViewModel { get; } = SearchModuleViewModel.Instance;

        /// <summary>
        /// 请求增量加载.
        /// </summary>
        /// <param name="sender">事件发送者.</param>
        /// <param name="e">事件参数.</param>
        protected async void OnViewRequestLoadMoreAsync(object sender, System.EventArgs e)
        {
            await ViewModel.RequestLoadMoreAsync();
        }
    }
}
