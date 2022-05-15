// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Bili.Locator.Uwp;
using Bili.Models.App.Other;
using Bili.Models.Data.Pgc;
using Bili.Models.Data.Video;
using Bili.ViewModels.Uwp.Pgc;
using Bili.ViewModels.Uwp.Video;
using ReactiveUI;
using Splat;

namespace Bili.ViewModels.Uwp
{
    /// <summary>
    /// 视频推荐视图模型.
    /// </summary>
    public partial class RecommendViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RecommendViewModel"/> class.
        /// </summary>
        internal RecommendViewModel()
        {
            ServiceLocator.Instance.LoadService(out _resourceToolkit)
                .LoadService(out _recommendProvider);
            VideoCollection = new ObservableCollection<IVideoBaseViewModel>();

            var canRequest = this.WhenAnyValue(
                x => x.IsInitializing,
                x => x.IsIncrementalLoading,
                (isInitializing, isIncrementalLoading) => !isInitializing && !isIncrementalLoading);

            InitializeCommand = ReactiveCommand.CreateFromTask(InitializeAsync, canRequest, RxApp.MainThreadScheduler);
            IncrementalCommand = ReactiveCommand.CreateFromTask(IncrementalAsync, canRequest, RxApp.MainThreadScheduler);

            _isInitializing = InitializeCommand.IsExecuting.ToProperty(
                this,
                x => x.IsInitializing,
                scheduler: RxApp.MainThreadScheduler);

            _isIncrementalLoading = IncrementalCommand.IsExecuting.ToProperty(
                this,
                x => x.IsIncrementalLoading,
                scheduler: RxApp.MainThreadScheduler);

            InitializeCommand.ThrownExceptions.Subscribe(DisplayException);
            IncrementalCommand.ThrownExceptions.Subscribe(LogException);
        }

        private async Task InitializeAsync()
        {
            VideoCollection.Clear();
            _recommendProvider.Reset();
            ClearException();
            await GetDataAsync();
        }

        private async Task IncrementalAsync()
            => await GetDataAsync();

        private async Task GetDataAsync()
        {
            var videos = await _recommendProvider.RequestRecommendVideosAsync();
            if (videos?.Any() ?? false)
            {
                foreach (var item in videos)
                {
                    IVideoBaseViewModel vm = null;
                    if (item is VideoInformation videoInfo)
                    {
                        vm = Splat.Locator.Current.GetService<VideoItemViewModel>();
                    }
                    else if (item is EpisodeInformation episodeInfo)
                    {
                        vm = Splat.Locator.Current.GetService<EpisodeItemViewModel>();
                    }

                    vm.SetInformation(item);
                    VideoCollection.Add(vm);
                }
            }
        }

        private void DisplayException(Exception exception)
        {
            IsError = true;
            var msg = exception is ServiceException se
                ? se.Error?.Message ?? se.Message
                : exception.Message;
            ErrorText = $"{_resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.RequestRecommendFailed)}\n{msg}";
            LogException(exception);
        }

        private void LogException(Exception exception)
            => this.Log().Debug(exception);

        private void ClearException()
        {
            IsError = false;
            ErrorText = string.Empty;
        }
    }
}
