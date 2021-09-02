// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Richasy.Bili.Locator.Uwp;
using Richasy.Bili.Models.App.Other;
using Richasy.Bili.Models.Enums;
using Richasy.Bili.Models.Enums.App;

namespace Richasy.Bili.ViewModels.Uwp
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
            DefaultVideoFavoriteVideoCollection = new ObservableCollection<VideoViewModel>();
            TypeCollection = new ObservableCollection<FavoriteType>
            {
                FavoriteType.Video,
                FavoriteType.Anime,
                FavoriteType.Movie,
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
                var response = await Controller.GetVideoFavoriteGalleryAsync(UserId);

                foreach (var folder in response.FavoriteFolderList)
                {
                    if (folder.MediaList.List != null)
                    {
                        VideoFolderCollection.Add(new FavoriteVideoFolderViewModel(folder));
                    }
                }

                DefaultVideoFavoriteDetail = response.DefaultFavoriteList.Detail;
                if (response.DefaultFavoriteList.Medias.Count > 0)
                {
                    response.DefaultFavoriteList.Medias.ForEach(p => DefaultVideoFavoriteVideoCollection.Add(new VideoViewModel(p)));
                }
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

            IsVideoRequested = DefaultVideoFavoriteDetail != null;
            IsVideoInitializeLoading = false;
        }

        private void Reset()
        {
            VideoFolderCollection.Clear();
            DefaultVideoFavoriteVideoCollection.Clear();
            IsVideoInitializeLoading = false;
            IsVideoError = false;
            IsVideoRequested = false;
            DefaultVideoFavoriteDetail = null;
        }
    }
}
