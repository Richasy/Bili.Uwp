// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using System.Threading.Tasks;
using ReactiveUI.Fody.Helpers;
using Richasy.Bili.Controller.Uwp;
using Richasy.Bili.Models.BiliBili;

namespace Richasy.Bili.ViewModels.Uwp
{
    /// <summary>
    /// 收藏夹详情视图模型.
    /// </summary>
    public class FavoriteListDetailViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FavoriteListDetailViewModel"/> class.
        /// </summary>
        /// <param name="data">数据.</param>
        public FavoriteListDetailViewModel(FavoriteListDetail data)
        {
            var accVM = AccountViewModel.Instance;
            var isMe = accVM.Mid.HasValue && accVM.Mid.Value == data.Mid;
            Data = data;

            if (isMe)
            {
                IsShowDeleteButton = true;
            }
            else
            {
                IsShowUnFavoriteButton = true;
            }
        }

        /// <summary>
        /// 数据.
        /// </summary>
        [Reactive]
        public FavoriteListDetail Data { get; set; }

        /// <summary>
        /// 是否显示删除收藏夹按钮（该收藏夹由自己创建）.
        /// </summary>
        [Reactive]
        public bool IsShowDeleteButton { get; set; }

        /// <summary>
        /// 是否显示取消收藏按钮（该收藏夹来自其他人）.
        /// </summary>
        [Reactive]
        public bool IsShowUnFavoriteButton { get; set; }

        /// <summary>
        /// 删除.
        /// </summary>
        /// <returns>删除结果.</returns>
        public async Task<bool> DeleteAsync()
        {
            var result = false;
            try
            {
                result = await BiliController.Instance.RemoveFavoriteFolderAsync(Data.Id, true);
            }
            catch (System.Exception)
            {
            }

            return result;
        }

        /// <summary>
        /// 取消关注.
        /// </summary>
        /// <returns>删除结果.</returns>
        public async Task<bool> UnFavoriteAsync()
        {
            var result = false;
            try
            {
                result = await BiliController.Instance.RemoveFavoriteFolderAsync(Data.Id, false);
            }
            catch (System.Exception)
            {
            }

            return result;
        }

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is FavoriteListDetailViewModel model && EqualityComparer<int>.Default.Equals(Data.Id, model.Data.Id);

        /// <inheritdoc/>
        public override int GetHashCode() => -301143667 + EqualityComparer<int>.Default.GetHashCode(Data.Id);
    }
}
