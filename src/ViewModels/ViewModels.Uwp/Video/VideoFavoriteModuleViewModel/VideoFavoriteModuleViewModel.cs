// Copyright (c) Richasy. All rights reserved.

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
            NavigationViewModel navigationViewModel,
            IAccountProvider accountProvider,
            IFavoriteProvider favoriteProvider,
            CoreDispatcher dispatcher)
            : base(dispatcher)
        {
            _navigationViewModel = navigationViewModel;
            _accountProvider = accountProvider;
            _favoriteProvider = favoriteProvider;
            ShowDefaultFolderDetailCommand = ReactiveCommand.Create(ShowDefaultFolderDetail);
        }

        /// <inheritdoc/>
        protected override void BeforeReload()
        {
            DefaultFolder = null;
            IsDefaultFolderEmpty = false;
        }

        /// <inheritdoc/>
        protected override async Task GetDataAsync()
        {
            var data = await _favoriteProvider.GetVideoFavoriteViewAsync(_accountProvider.UserId.ToString());
            DefaultFolder = data.DefaultFolder.Folder;
            IsDefaultFolderEmpty = data.DefaultFolder.VideoSet.TotalCount == 0;

            foreach (var item in data.Groups)
            {
                var groupVM = Locator.Current.GetService<VideoFavoriteFolderGroupViewModel>();
                groupVM.SetGroup(item);
                Items.Add(groupVM);
            }
        }

        private void ShowDefaultFolderDetail()
            => _navigationViewModel.NavigateToSecondaryView(Models.Enums.PageIds.VideoFavoriteDetail, DefaultFolder);
    }
}
