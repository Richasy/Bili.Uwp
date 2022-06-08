// Copyright (c) Richasy. All rights reserved.

using System;
using Bili.ViewModels.Uwp.Toolbox;
using Splat;

namespace Bili.App.Controls
{
    /// <summary>
    /// 封面下载工具.
    /// </summary>
    public sealed partial class CoverDownloaderView : CenterPopup
    {
        private readonly CoverDownloaderViewModel _viewModel = Splat.Locator.Current.GetService<CoverDownloaderViewModel>();

        /// <summary>
        /// Initializes a new instance of the <see cref="CoverDownloaderView"/> class.
        /// </summary>
        public CoverDownloaderView() => InitializeComponent();

        private void OnIdQuerySubmitted(Windows.UI.Xaml.Controls.AutoSuggestBox sender, Windows.UI.Xaml.Controls.AutoSuggestBoxQuerySubmittedEventArgs args)
            => _viewModel.LoadPreviewCommand.Execute().Subscribe();
    }
}
