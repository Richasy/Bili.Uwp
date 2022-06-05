// Copyright (c) Richasy. All rights reserved.

using System.Threading.Tasks;
using Bili.Lib.Interfaces;
using Bili.Models.Data.Video;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Uwp.Base;
using Splat;
using Windows.UI.Core;

namespace Bili.ViewModels.Uwp.Video
{
    /// <summary>
    /// 收藏夹分组视图模型.
    /// </summary>
    public sealed partial class VideoFavoriteFolderGroupViewModel : InformationFlowViewModelBase<VideoFavoriteFolderViewModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VideoFavoriteFolderGroupViewModel"/> class.
        /// </summary>
        internal VideoFavoriteFolderGroupViewModel(
            IFavoriteProvider favoriteProvider,
            IResourceToolkit resourceToolkit,
            IAccountProvider accountProvider,
            CoreDispatcher dispatcher)
            : base(dispatcher)
        {
            _favoriteProvider = favoriteProvider;
            _resourceToolkit = resourceToolkit;
            _accountProvider = accountProvider;
        }

        /// <summary>
        /// 设置分组信息.
        /// </summary>
        /// <param name="group">分组信息.</param>
        public void SetGroup(VideoFavoriteFolderGroup group)
        {
            Group = group;
            Items.Clear();
            HandleFavoriteFolderList(group.FavoriteSet);
        }

        /// <inheritdoc/>
        protected override async Task GetDataAsync()
        {
            if (!HasMore)
            {
                return;
            }

            var data = await _favoriteProvider.GetVideoFavoriteFolderListAsync(_accountProvider.UserId.ToString(), Group.IsMine);
            HandleFavoriteFolderList(data);
        }

        /// <inheritdoc/>
        protected override string FormatException(string errorMsg)
            => $"{_resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.RequestVideoFavoriteFailed)}\n{errorMsg}";

        private void HandleFavoriteFolderList(VideoFavoriteSet set)
        {
            foreach (var item in set.Items)
            {
                var folderVM = Splat.Locator.Current.GetService<VideoFavoriteFolderViewModel>();
                folderVM.SetFolder(item, this);
                Items.Add(folderVM);
            }

            IsEmpty = Items.Count == 0;
            HasMore = Items.Count < set.TotalCount;
        }
    }
}
