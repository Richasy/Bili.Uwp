// Copyright (c) Richasy. All rights reserved.

using System;
using System.Threading.Tasks;
using Bili.Lib.Interfaces;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Video;
using Bili.ViewModels.Uwp.Base;
using ReactiveUI;
using Splat;
using Windows.UI.Core;

namespace Bili.ViewModels.Uwp.Account
{
    /// <summary>
    /// 历史记录页面视图模型.
    /// </summary>
    public sealed partial class HistoryPageViewModel : InformationFlowViewModelBase<IVideoItemViewModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HistoryPageViewModel"/> class.
        /// </summary>
        public HistoryPageViewModel(
            IAccountProvider accountProvider,
            IResourceToolkit resourceToolkit,
            CoreDispatcher dispatcher)
            : base(dispatcher)
        {
            _accountProvider = accountProvider;
            _resourceToolkit = resourceToolkit;

            ClearCommand = ReactiveCommand.CreateFromTask(ClearAllAsync);
            _isClearing = ClearCommand.IsExecuting.ToProperty(this, x => x.IsClearing);
        }

        /// <inheritdoc/>
        protected override void BeforeReload()
        {
            _accountProvider.ResetHistoryStatus();
            IsEmpty = false;
            _isEnd = false;
        }

        /// <inheritdoc/>
        protected async override Task GetDataAsync()
        {
            if (_isEnd)
            {
                return;
            }

            var data = await _accountProvider.GetMyHistorySetAsync();
            foreach (var item in data.Items)
            {
                var videoVM = Splat.Locator.Current.GetService<IVideoItemViewModel>();
                videoVM.InjectData(item);
                videoVM.InjectAction(vm => RemoveVideo(vm));
                Items.Add(videoVM);
            }

            _isEnd = data.IsFinished;
            IsEmpty = Items.Count == 0;
        }

        /// <inheritdoc/>
        protected override string FormatException(string errorMsg)
            => $"{_resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.RequestHistoryFailed)}\n{errorMsg}";

        private async Task ClearAllAsync()
        {
            var result = await _accountProvider.ClearHistoryAsync();
            if (result)
            {
                TryClear(Items);
                ReloadCommand.Execute().Subscribe();
            }
        }

        private void RemoveVideo(IVideoItemViewModel vm)
        {
            Items.Remove(vm);
            IsEmpty = Items.Count == 0;
        }
    }
}
