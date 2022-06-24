// Copyright (c) Richasy. All rights reserved.

using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Bili.Lib.Interfaces;
using Bili.ViewModels.Uwp.Base;
using Bili.ViewModels.Uwp.Core;
using ReactiveUI;
using Splat;
using Windows.UI.Core;

namespace Bili.ViewModels.Uwp.Video
{
    /// <summary>
    /// 视频收藏模块视图模型.
    /// </summary>
    public sealed partial class VideoFavoriteModuleViewModel : InformationFlowViewModelBase<VideoFavoriteFolderGroupViewModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VideoFavoriteModuleViewModel"/> class.
        /// </summary>
        public VideoFavoriteModuleViewModel(
            AppViewModel appViewModel,
            IAccountProvider accountProvider,
            IFavoriteProvider favoriteProvider,
            CoreDispatcher dispatcher)
            : base(dispatcher)
        {
            _appViewModel = appViewModel;
            _accountProvider = accountProvider;
            _favoriteProvider = favoriteProvider;
            DefaultVideos = new ObservableCollection<VideoItemViewModel>();
            ShowDefaultFolderDetailCommand = ReactiveCommand.Create(ShowDefaultFolderDetail, outputScheduler: RxApp.MainThreadScheduler);
        }

        /// <inheritdoc/>
        protected override void BeforeReload()
        {
            DefaultFolder = null;
            IsDefaultFolderEmpty = false;
            CanShowMoreDefaultVideos = false;
            DefaultVideos.Clear();
        }

        /// <inheritdoc/>
        protected override async Task GetDataAsync()
        {
            var data = await _favoriteProvider.GetVideoFavoriteViewAsync(_accountProvider.UserId.ToString());
            DefaultFolder = data.DefaultFolder.Folder;
            foreach (var item in data.DefaultFolder.VideoSet.Items)
            {
                var videoVM = Splat.Locator.Current.GetService<VideoItemViewModel>();
                videoVM.SetInformation(item);
                videoVM.SetAdditionalData(DefaultFolder.Id);
                videoVM.SetAdditionalAction(vm => RemoveDefaultVideo(vm));
                DefaultVideos.Add(videoVM);
            }

            var count = data.DefaultFolder.VideoSet.TotalCount;
            IsDefaultFolderEmpty = DefaultVideos.Count == 0;
            CanShowMoreDefaultVideos = count > DefaultVideos.Count;

            foreach (var item in data.Groups)
            {
                var groupVM = Splat.Locator.Current.GetService<VideoFavoriteFolderGroupViewModel>();
                groupVM.SetGroup(item);
                Items.Add(groupVM);
            }
        }

        private void RemoveDefaultVideo(VideoItemViewModel vm)
        {
            DefaultVideos.Remove(vm);
            IsDefaultFolderEmpty = DefaultVideos.Count == 0;
        }

        private void ShowDefaultFolderDetail()
            => _appViewModel.ShowVideoFavoriteFolderDetail(DefaultFolder);
    }
}
