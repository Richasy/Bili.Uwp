// Copyright (c) Richasy. All rights reserved.

using Bili.App.Controls;
using Bili.ViewModels.Uwp;
using Windows.UI.Xaml;

namespace Bili.App.Pages.Desktop
{
    /// <summary>
    /// 工具箱页面.
    /// </summary>
    public sealed partial class ToolboxPage : AppPage
    {
        private readonly ToolboxPageViewModel _viewModel = ToolboxPageViewModel.Instance;

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
}
