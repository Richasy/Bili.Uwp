// Copyright (c) Richasy. All rights reserved.

using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ReactiveUI.Fody.Helpers;
using Richasy.Bili.Models.BiliBili;

namespace Richasy.Bili.ViewModels.Uwp
{
    /// <summary>
    /// 视频收藏夹分类视图模型.
    /// </summary>
    public class FavoriteVideoFolderViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FavoriteVideoFolderViewModel"/> class.
        /// </summary>
        /// <param name="folder">收藏夹分类.</param>
        public FavoriteVideoFolderViewModel(FavoriteFolder folder)
        {
            Name = folder.Name;
            Id = folder.Id;
            FavoriteCollection = new ObservableCollection<FavoriteListDetail>();
            if (folder.MediaList != null)
            {
                foreach (var item in folder.MediaList.List)
                {
                    item.Cover += "@200w_160h_1c_100q.jpg";
                    FavoriteCollection.Add(item);
                }

                HasMore = folder.MediaList.HasMore;
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
        /// 收藏列表集合.
        /// </summary>
        [Reactive]
        public ObservableCollection<FavoriteListDetail> FavoriteCollection { get; set; }

        /// <summary>
        /// 是否还有更多.
        /// </summary>
        [Reactive]
        public bool HasMore { get; set; }

        /// <summary>
        /// 加载更多.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        public async Task LoadMoreAsync()
        {
            await Task.CompletedTask;
        }
    }
}
