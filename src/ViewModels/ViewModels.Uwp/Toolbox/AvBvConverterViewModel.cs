// Copyright (c) Richasy. All rights reserved.

using System;
using System.Reactive;
using System.Threading.Tasks;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Richasy.Bili.Controller.Uwp;
using Richasy.Bili.Locator.Uwp;
using Richasy.Bili.Toolkit.Interfaces;
using Windows.UI.Core;

namespace Richasy.Bili.ViewModels.Uwp
{
    /// <summary>
    /// AV/BV互转视图模型.
    /// </summary>
    public sealed class AvBvConverterViewModel : ViewModelBase
    {
        private readonly IResourceToolkit _resourceToolkit;
        private readonly ObservableAsPropertyHelper<bool> _isConverting;

        /// <summary>
        /// Initializes a new instance of the <see cref="AvBvConverterViewModel"/> class.
        /// </summary>
        public AvBvConverterViewModel()
        {
            ServiceLocator.Instance.LoadService(out _resourceToolkit);
            ConvertCommand = ReactiveCommand.CreateFromTask(ConvertAsync, outputScheduler: RxApp.MainThreadScheduler);
            _isConverting = ConvertCommand.IsExecuting.ToProperty(this, x => x.IsConverting, scheduler: RxApp.MainThreadScheduler);

            ConvertCommand.ThrownExceptions.Subscribe(DisplayExAsync);
        }

        /// <summary>
        /// 转换命令.
        /// </summary>
        public ReactiveCommand<Unit, Unit> ConvertCommand { get; }

        /// <summary>
        /// 输入的Id.
        /// </summary>
        [Reactive]
        public string InputId { get; set; }

        /// <summary>
        /// 输出的Id.
        /// </summary>
        [Reactive]
        public string OutputId { get; set; }

        /// <summary>
        /// 是否出错.
        /// </summary>
        [Reactive]
        public bool IsError { get; set; }

        /// <summary>
        /// 错误信息.
        /// </summary>
        [Reactive]
        public string ErrorMessage { get; set; }

        /// <summary>
        /// 调度器.
        /// </summary>
        public CoreDispatcher Dispatcher { get; set; }

        /// <summary>
        /// 是否正在转换中.
        /// </summary>
        public bool IsConverting => _isConverting.Value;

        /// <summary>
        /// 转换.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        private async Task ConvertAsync()
        {
            if (!BiliController.Instance.IsNetworkAvailable)
            {
                throw new InvalidOperationException(_resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.NetworkError));
            }

            if (!string.IsNullOrEmpty(InputId))
            {
                IsError = false;
                OutputId = string.Empty;

                var type = ToolboxPageViewModel.GetVideoIdType(InputId, out var aid);

                if (type == Models.Enums.VideoIdType.Bv)
                {
                    var reply = await BiliController.Instance.GetVideoDetailAsync(InputId);
                    OutputId = reply.Arc.Aid.ToString();
                }
                else if (type == Models.Enums.VideoIdType.Av)
                {
                    var reply = await BiliController.Instance.GetVideoDetailAsync(aid);
                    OutputId = reply.Bvid;
                }
                else
                {
                    throw new ArgumentException(_resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.InvalidVideoId));
                }
            }
        }

        private async void DisplayExAsync(Exception ex)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                IsError = true;
                ErrorMessage = ex.Message;
            });
        }
    }
}
