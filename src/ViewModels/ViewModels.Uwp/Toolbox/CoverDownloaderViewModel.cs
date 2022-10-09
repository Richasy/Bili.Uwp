// Copyright (c) Richasy. All rights reserved.

using System;
using System.Net.Http;
using System.Threading.Tasks;
using Bili.Lib.Interfaces;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Toolbox;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Windows.Storage;
using Windows.System;
using Windows.UI.Core;

namespace Bili.ViewModels.Uwp.Toolbox
{
    /// <summary>
    /// 封面下载器视图模型.
    /// </summary>
    public sealed partial class CoverDownloaderViewModel : ViewModelBase, ICoverDownloaderViewModel
    {
        private readonly IPlayerProvider _playerProvider;
        private readonly IVideoToolkit _videoToolkit;
        private readonly CoreDispatcher _dispatcher;

        [ObservableProperty]
        private string _coverUrl;

        [ObservableProperty]
        private string _inputId;

        [ObservableProperty]
        private bool _isShowError;

        [ObservableProperty]
        private string _errorMessage;

        [ObservableProperty]
        private bool _isDownloading;

        /// <summary>
        /// Initializes a new instance of the <see cref="CoverDownloaderViewModel"/> class.
        /// </summary>
        public CoverDownloaderViewModel(
             IVideoToolkit videoToolkit,
             IPlayerProvider playerProvider,
             CoreDispatcher dispatcher)
        {
            _videoToolkit = videoToolkit;
            _playerProvider = playerProvider;
            _dispatcher = dispatcher;

            DownloadCommand = new AsyncRelayCommand(DownloadCoverAsync);
            LoadPreviewCommand = new AsyncRelayCommand(LoadPreviewAsync);

            AttachIsRunningToAsyncCommand(p => IsDownloading = p, DownloadCommand);
            AttachExceptionHandlerToAsyncCommand(DisplayExAsync, DownloadCommand, LoadPreviewCommand);
        }

        /// <inheritdoc/>
        public IAsyncRelayCommand LoadPreviewCommand { get; }

        /// <inheritdoc/>
        public IAsyncRelayCommand DownloadCommand { get; }

        private async Task LoadPreviewAsync()
        {
            if (!string.IsNullOrEmpty(InputId))
            {
                IsShowError = false;
                var type = _videoToolkit.GetVideoIdType(InputId, out var avid);
                var id = type == Models.Enums.VideoIdType.Bv ? InputId : avid;
                var reply = await _playerProvider.GetVideoDetailAsync(id);
                CoverUrl = reply.Information.Identifier.Cover.GetSourceUri().ToString();
            }
        }

        private async Task DownloadCoverAsync()
        {
            if (string.IsNullOrEmpty(CoverUrl))
            {
                return;
            }

            IsShowError = false;
            var folder = KnownFolders.PicturesLibrary;

            using (var httpClient = new HttpClient())
            {
                var bytes = await httpClient.GetByteArrayAsync(new Uri(CoverUrl));
                var imgFile = await folder.CreateFileAsync($"{DateTime.Now:yyyy-MM-dd}-{Guid.NewGuid():N}.png");
                await FileIO.WriteBytesAsync(imgFile, bytes);

                await Launcher.LaunchFileAsync(imgFile);
            }
        }

        private async void DisplayExAsync(Exception ex)
        {
            await _dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                ErrorMessage = ex.Message;
            });
        }

        partial void OnErrorMessageChanged(string value)
            => IsShowError = !string.IsNullOrEmpty(value);
    }
}
