// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Bili.Locator.Uwp;
using Bili.Models.App.Other;
using Bili.Models.Enums;
using Bili.Models.Enums.App;

namespace Bili.ViewModels.Uwp
{
    /// <summary>
    /// 收藏夹视图模型.
    /// </summary>
    public partial class FavoriteViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FavoriteViewModel"/> class.
        /// </summary>
        protected FavoriteViewModel()
        {
            VideoFolderCollection = new ObservableCollection<FavoriteVideoFolderViewModel>();
            TypeCollection = new ObservableCollection<FavoriteType>
            {
                FavoriteType.Video,
                FavoriteType.Anime,
                FavoriteType.Cinema,
                FavoriteType.Article,
            };

            ServiceLocator.Instance.LoadService(out _resourceToolkit);
        }

        /// <summary>
        /// 设置用户.
        /// </summary>
        /// <param name="userId">用户Id.</param>
        /// <param name="userName">用户名.</param>
        public void SetUser(int userId, string userName)
        {
            if (UserId != userId)
            {
                UserId = userId;
                UserName = userName;
                Reset();
            }
        }

        /// <summary>
        /// 初始化请求.
        /// </summary>
        /// <param name="type">收藏夹类型.</param>
        /// <returns><see cref="Task"/>.</returns>
        public async Task InitializeRequestAsync(FavoriteType type)
        {
            CurrentType = type;

            if (CurrentType == FavoriteType.Video)
            {
                await InitializeVideoRequestAsync();
            }
        }

        /// <summary>
        /// 取消关注视频.
        /// </summary>
        /// <param name="favoriteId">收藏夹Id.</param>
        /// <param name="resourceId">资源Id.</param>
        /// <returns>取消收藏结果.</returns>
        public Task<bool> RemoveFavoriteVideoAsync(int favoriteId, int resourceId)
            => Controller.RemoveFavoriteVideoAsync(favoriteId, resourceId);

        private async Task InitializeVideoRequestAsync()
        {
            if (UserId == 0)
            {
                throw new ArgumentException("必须先传入用户Id");
            }

            if (IsVideoInitializeLoading)
            {
                return;
            }

            IsVideoInitializeLoading = true;
            try
            {
                VideoFolderCollection.Clear();
                var response = await Controller.GetVideoFavoriteGalleryAsync(UserId);

                foreach (var folder in response.FavoriteFolderList)
                {
                    if (folder.MediaList.List != null)
                    {
                        VideoFolderCollection.Add(new FavoriteVideoFolderViewModel(folder));
                    }
                }

                var myInfo = response.DefaultFavoriteList.Detail.Publisher;
                myInfo.Publisher = AccountViewModel.Instance.DisplayName;
                myInfo.PublisherAvatar = AccountViewModel.Instance.Avatar;

                DefaultVideoViewModel = new FavoriteVideoViewModel(response.DefaultFavoriteList);
            }
            catch (ServiceException ex)
            {
                IsVideoError = true;
                VideoErrorText = $"{_resourceToolkit.GetLocaleString(LanguageNames.RequestVideoFavoriteFailed)}\n{ex.Error?.Message ?? ex.Message}";
            }
            catch (InvalidOperationException invalidEx)
            {
                IsVideoError = true;
                VideoErrorText = invalidEx.Message;
            }

            IsVideoRequested = DefaultVideoViewModel != null;
            IsVideoInitializeLoading = false;
        }

        private void Reset()
        {
            VideoFolderCollection.Clear();
            IsVideoInitializeLoading = false;
            IsVideoError = false;
            IsVideoRequested = false;
            DefaultVideoViewModel = null;
        }
    }
}
