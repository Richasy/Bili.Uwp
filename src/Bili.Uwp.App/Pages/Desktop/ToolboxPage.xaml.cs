// Copyright (c) Richasy. All rights reserved.

using Bili.Uwp.App.Controls;
using Bili.ViewModels.Interfaces.Home;
using Bili.ViewModels.Interfaces.Toolbox;
using Windows.UI.Xaml;

namespace Bili.Uwp.App.Pages.Desktop
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

        /// <inheritdoc/>
        protected override void OnPageLoaded()
            => Bindings.Update();

        /// <inheritdoc/>
        protected override void OnPageUnloaded()
            => Bindings.StopTracking();

        private void OnItemClick(object sender, RoutedEventArgs e)
        {
            var item = (sender as FrameworkElement).DataContext as IToolboxItemViewModel;
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
    public class ToolboxPageBase : AppPage<IToolboxPageViewModel>
    {
    }
}
