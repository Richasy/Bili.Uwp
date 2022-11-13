// Copyright (c) Richasy. All rights reserved.

using Bili.DI.Container;
using Bili.ViewModels.Interfaces.Toolbox;

namespace Bili.Desktop.App.Controls
{
    /// <summary>
    /// 封面下载工具.
    /// </summary>
    public sealed partial class CoverDownloaderView : CenterPopup
    {
        private readonly ICoverDownloaderViewModel _viewModel = Locator.Instance.GetService<ICoverDownloaderViewModel>();

        /// <summary>
        /// Initializes a new instance of the <see cref="CoverDownloaderView"/> class.
        /// </summary>
        public CoverDownloaderView() => InitializeComponent();

        private void OnIdQuerySubmitted(Microsoft.UI.Xaml.Controls.AutoSuggestBox sender, Microsoft.UI.Xaml.Controls.AutoSuggestBoxQuerySubmittedEventArgs args)
            => _viewModel.LoadPreviewCommand.ExecuteAsync(null);
    }
}
