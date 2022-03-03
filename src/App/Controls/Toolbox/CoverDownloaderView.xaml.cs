// Copyright (c) Richasy. All rights reserved.

using System;
using Richasy.Bili.ViewModels.Uwp;

namespace Richasy.Bili.App.Controls
{
    /// <summary>
    /// 封面下载工具.
    /// </summary>
    public sealed partial class CoverDownloaderView : CenterPopup
    {
        private readonly CoverDownloaderViewModel _viewModel = new CoverDownloaderViewModel();

        /// <summary>
        /// Initializes a new instance of the <see cref="CoverDownloaderView"/> class.
        /// </summary>
        public CoverDownloaderView()
        {
            InitializeComponent();
            _viewModel.Dispatcher = Dispatcher;
        }

        private void OnIdQuerySubmitted(Windows.UI.Xaml.Controls.AutoSuggestBox sender, Windows.UI.Xaml.Controls.AutoSuggestBoxQuerySubmittedEventArgs args)
            => _viewModel.LoadPreviewCommand.Execute().Subscribe();
    }
}
