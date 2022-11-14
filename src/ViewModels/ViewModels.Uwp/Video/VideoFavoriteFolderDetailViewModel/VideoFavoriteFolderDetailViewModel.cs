// Copyright (c) Richasy. All rights reserved.

using System.Threading.Tasks;
using Bili.DI.Container;
using Bili.Lib.Interfaces;
using Bili.Models.Data.Video;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Account;
using Bili.ViewModels.Interfaces.Video;
using Bili.ViewModels.Uwp.Base;

namespace Bili.ViewModels.Uwp.Video
{
    /// <summary>
    /// 视频收藏夹详情视图模型.
    /// </summary>
    public sealed partial class VideoFavoriteFolderDetailViewModel : InformationFlowViewModelBase<IVideoItemViewModel>, IVideoFavoriteFolderDetailViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VideoFavoriteFolderDetailViewModel"/> class.
        /// </summary>
        public VideoFavoriteFolderDetailViewModel(
            IFavoriteProvider favoriteProvider,
            IResourceToolkit resourceToolkit,
            IAccountViewModel accountViewModel)
        {
            _favoriteProvider = favoriteProvider;
            _resourceToolkit = resourceToolkit;
            _accountViewModel = accountViewModel;
        }

        /// <inheritdoc/>
        public void InjectData(VideoFavoriteFolder folder)
        {
            Data = folder;
            User = folder.User;
            TryClear(Items);

            if (User == null)
            {
                User = _accountViewModel.AccountInformation.User;
            }

            BeforeReload();
        }

        /// <inheritdoc/>
        protected override void BeforeReload()
        {
            _favoriteProvider.ResetVideoFolderDetailStatus();
            IsEmpty = Data.TotalCount == 0;
            _isEnd = Data.TotalCount == 0;
        }

        /// <inheritdoc/>
        protected override string FormatException(string errorMsg)
            => $"{_resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.RequestVideoFavoriteFailed)}\n{errorMsg}";

        /// <inheritdoc/>
        protected override async Task GetDataAsync()
        {
            if (_isEnd || Data.TotalCount == 0)
            {
                return;
            }

            var data = await _favoriteProvider.GetVideoFavoriteFolderDetailAsync(Data.Id);
            foreach (var item in data.VideoSet.Items)
            {
                var videoVM = Locator.Instance.GetService<IVideoItemViewModel>();
                videoVM.InjectData(item);
                videoVM.SetAdditionalData(Data.Id);
                videoVM.InjectAction(RemoveVideo);
                Items.Add(videoVM);
            }

            IsEmpty = Items.Count == 0;
            _isEnd = Items.Count == Data.TotalCount;
        }

        private void RemoveVideo(IVideoItemViewModel vm)
        {
            Items.Remove(vm);
            IsEmpty = Items.Count == 0;
        }
    }
}
