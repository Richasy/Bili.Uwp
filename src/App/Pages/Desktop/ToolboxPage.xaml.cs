// Copyright (c) Richasy. All rights reserved.

using Bili.App.Controls;
using Bili.ViewModels.Uwp.Home;
using Bili.ViewModels.Uwp.Toolbox;
using Windows.UI.Xaml;

namespace Bili.App.Pages.Desktop
{
    /// <summary>
    /// 工具箱页面.
    /// </summary>
    public sealed partial class ToolboxPage : ToolboxPageBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ToolboxPage"/> class.
        /// </summary>
        public ToolboxPage() => InitializeComponent();

        private void OnItemClick(object sender, RoutedEventArgs e)
        {
            var item = (sender as FrameworkElement).DataContext as ToolboxItemViewModel;
            switch (item.Type)
            {
                case Models.Enums.ToolboxItemType.AvBvConverter:
                    new AvBvConverterView().Show();
                    break;
                case Models.Enums.ToolboxItemType.CoverDownloader:
                    new CoverDownloaderView().Show();
                    break;
                default:
                    break;
            }
        }
    }

    /// <summary>
    /// <see cref="ToolboxPage"/> 的基类.
    /// </summary>
    public class ToolboxPageBase : AppPage<ToolboxPageViewModel>
    {
    }
}
