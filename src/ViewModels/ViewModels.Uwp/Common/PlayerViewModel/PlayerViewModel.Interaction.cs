// Copyright (c) Richasy. All rights reserved.

using System.Threading.Tasks;

namespace Richasy.Bili.ViewModels.Uwp
{
    /// <summary>
    /// 播放器视图模型.
    /// </summary>
    public partial class PlayerViewModel
    {
        /// <summary>
        /// 一键三连.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        public async Task TripleAsync()
        {
            if (_videoId > 0)
            {
                var result = await Controller.TripleAsync(_videoId);

                if (result != null)
                {
                    IsLikeChecked = result.IsLike;
                    IsCoinChecked = result.IsCoin;
                    IsFavoriteChecked = result.IsFavorite;
                }
            }
        }
    }
}
