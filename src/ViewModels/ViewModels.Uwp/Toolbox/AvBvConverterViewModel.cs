// Copyright (c) Richasy. All rights reserved.

using System;
using System.Threading.Tasks;
using Bili.Lib.Interfaces;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Core;
using Bili.ViewModels.Interfaces.Toolbox;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Windows.UI.Core;

namespace Bili.ViewModels.Uwp.Toolbox
{
    /// <summary>
    /// AV/BV互转视图模型.
    /// </summary>
    public sealed partial class AvBvConverterViewModel : ViewModelBase, IAvBvConverterViewModel
    {
        private readonly IPlayerProvider _playerProvider;
        private readonly IResourceToolkit _resourceToolkit;
        private readonly IVideoToolkit _videoToolkit;
        private readonly IAppViewModel _appViewModel;
        private readonly CoreDispatcher _dispatcher;

        [ObservableProperty]
        private string _inputId;

        [ObservableProperty]
        private string _outputId;

        [ObservableProperty]
        private bool _isError;

        [ObservableProperty]
        private string _errorMessage;

        [ObservableProperty]
        private bool _isConverting;

        /// <summary>
        /// Initializes a new instance of the <see cref="AvBvConverterViewModel"/> class.
        /// </summary>
        public AvBvConverterViewModel(
            IResourceToolkit resourceToolkit,
            IVideoToolkit videoToolkit,
            IPlayerProvider playerProvider,
            IAppViewModel appViewModel,
            CoreDispatcher dispatcher)
        {
            _resourceToolkit = resourceToolkit;
            _videoToolkit = videoToolkit;
            _appViewModel = appViewModel;
            _playerProvider = playerProvider;
            ConvertCommand = new AsyncRelayCommand(ConvertAsync);

            AttachIsRunningToAsyncCommand(p => IsConverting = p, ConvertCommand);
            AttachExceptionHandlerToAsyncCommand(DisplayExAsync, ConvertCommand);
            _dispatcher = dispatcher;
        }

        /// <inheritdoc/>
        public IAsyncRelayCommand ConvertCommand { get; }

        /// <summary>
        /// 转换.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        private async Task ConvertAsync()
        {
            if (!_appViewModel.IsNetworkAvaliable)
            {
                throw new InvalidOperationException(_resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.NetworkError));
            }

            if (!string.IsNullOrEmpty(InputId))
            {
                IsError = false;
                OutputId = string.Empty;

                var type = _videoToolkit.GetVideoIdType(InputId, out var aid);
                var id = type == Models.Enums.VideoIdType.Bv ? InputId : aid;
                var reply = await _playerProvider.GetVideoDetailAsync(id);
                if (type == Models.Enums.VideoIdType.Bv)
                {
                    OutputId = reply.Information.Identifier.Id;
                }
                else if (type == Models.Enums.VideoIdType.Av)
                {
                    OutputId = reply.Information.AlternateId;
                }
                else
                {
                    throw new ArgumentException(_resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.InvalidVideoId));
                }
            }
        }

        private async void DisplayExAsync(Exception ex)
        {
            await _dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                IsError = true;
                ErrorMessage = ex.Message;
            });
        }
    }
}
