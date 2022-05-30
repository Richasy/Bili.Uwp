// Copyright (c) Richasy. All rights reserved.

using System;
using System.Linq;
using System.Threading.Tasks;
using Bili.Lib.Interfaces;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Uwp.Base;
using Bili.ViewModels.Uwp.Core;
using Bili.ViewModels.Uwp.Video;
using ReactiveUI;
using Splat;
using Windows.UI.Core;

namespace Bili.ViewModels.Uwp.Account
{
    /// <summary>
    /// 稍后再看页面视图模型.
    /// </summary>
    public sealed partial class ViewLaterPageViewModel : InformationFlowViewModelBase<VideoItemViewModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ViewLaterPageViewModel"/> class.
        /// </summary>
        internal ViewLaterPageViewModel(
            IAccountProvider accountProvider,
            IResourceToolkit resourceToolkit,
            NavigationViewModel navigationViewModel,
            CoreDispatcher dispatcher)
            : base(dispatcher)
        {
            _accountProvider = accountProvider;
            _resourceToolkit = resourceToolkit;
            _navigationViewModel = navigationViewModel;

            ClearCommand = ReactiveCommand.CreateFromTask(ClearAllAsync, outputScheduler: RxApp.MainThreadScheduler);
            PlayAllCommand = ReactiveCommand.Create(PlayAll, outputScheduler: RxApp.MainThreadScheduler);
            RemoveVideoCommand = ReactiveCommand.Create<VideoItemViewModel>(RemoveVideo, outputScheduler: RxApp.MainThreadScheduler);

            _isClearing = ClearCommand.IsExecuting.ToProperty(this, x => x.IsClearing, scheduler: RxApp.MainThreadScheduler);
        }

        /// <inheritdoc/>
        protected override void BeforeReload()
        {
            _accountProvider.ResetViewLaterStatus();
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

            var data = await _accountProvider.GetViewLaterListAsync();
            foreach (var item in data.Items)
            {
                var videoVM = Splat.Locator.Current.GetService<VideoItemViewModel>();
                videoVM.SetInformation(item);
                Items.Add(videoVM);
            }

            _isEnd = Items.Count == data.TotalCount;
            IsEmpty = Items.Count == 0;
        }

        /// <inheritdoc/>
        protected override string FormatException(string errorMsg)
            => $"{_resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.RequestViewLaterFailed)}\n{errorMsg}";

        private async Task ClearAllAsync()
        {
            var result = await _accountProvider.ClearViewLaterAsync();
            if (result)
            {
                Items.Clear();
                ReloadCommand.Execute().Subscribe();
            }
        }

        private void PlayAll()
        {
            if (Items.Count > 1)
            {
                _navigationViewModel.NavigateToPlayView(Items.Select(p => p.Information).ToList());
            }
            else if (Items.Count > 0)
            {
                _navigationViewModel.NavigateToPlayView(Items.First().Information);
            }
        }

        private void RemoveVideo(VideoItemViewModel vm)
        {
            Items.Remove(vm);
            IsEmpty = Items.Count == 0;
        }
    }
}
