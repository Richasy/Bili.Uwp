// Copyright (c) GodLeaveMe. All rights reserved.

using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ReactiveUI.Fody.Helpers;
using Richasy.Bili.Controller.Uwp;
using Richasy.Bili.Models.BiliBili;

namespace Richasy.Bili.ViewModels.Uwp
{
    /// <summary>
    /// 视频收藏夹分类视图模型.
    /// </summary>
    public class FavoriteVideoFolderViewModel : ViewModelBase
    {
        private int _pageNumber;

        /// <summary>
        /// Initializes a new instance of the <see cref="FavoriteVideoFolderViewModel"/> class.
        /// </summary>
        /// <param name="folder">收藏夹分类.</param>
        public FavoriteVideoFolderViewModel(FavoriteFolder folder)
        {
            Name = folder.Name;
            Id = folder.Id;
            FavoriteCollection = new ObservableCollection<FavoriteListDetailViewModel>();
            _pageNumber = 1;
            IsMine = folder.Id == 1;
            if (folder.MediaList != null)
            {
                HandleMediaList(folder.MediaList);
            }
        }

        /// <summary>
        /// 分类名.
        /// </summary>
        [Reactive]
        public string Name { get; set; }

        /// <summary>
        /// 标识符.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 是否是自己创建的收藏夹分类.
        /// </summary>
        public bool IsMine { get; set; }

        /// <summary>
        /// 收藏列表集合.
        /// </summary>
        [Reactive]
        public ObservableCollection<FavoriteListDetailViewModel> FavoriteCollection { get; set; }

        /// <summary>
        /// 是否还有更多.
        /// </summary>
        [Reactive]
        public bool HasMore { get; set; }

        /// <summary>
        /// 是否正在增量加载.
        /// </summary>
        [Reactive]
        public bool IsDeltaLoading { get; set; }

        /// <summary>
        /// 是否显示空白.
        /// </summary>
        [Reactive]
        public bool IsShowEmpty { get; set; }

        /// <summary>
        /// 加载更多.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        public async Task LoadMoreAsync()
        {
            if (!IsDeltaLoading && HasMore)
            {
                IsDeltaLoading = true;
                try
                {
                    var response = await BiliController.Instance.GetFavoriteFolderListAsync(AccountViewModel.Instance.Mid.Value, _pageNumber);
                    HandleMediaList(response);
                }
                catch (System.Exception)
                {
                }

                IsDeltaLoading = false;
            }
        }

        private void HandleMediaList(FavoriteMediaList list)
        {
            foreach (var item in list.List)
            {
                item.Cover += "@200w_160h_1c_100q.jpg";
                FavoriteCollection.Add(new FavoriteListDetailViewModel(item));
            }

            IsShowEmpty = FavoriteCollection.Count == 0;
            HasMore = list.HasMore;
            if (HasMore)
            {
                _pageNumber += 1;
            }
        }
    }
}
