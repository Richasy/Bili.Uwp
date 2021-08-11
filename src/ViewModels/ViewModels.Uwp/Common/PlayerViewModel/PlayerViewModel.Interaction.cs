// Copyright (c) Richasy. All rights reserved.

using System.Threading.Tasks;
using Richasy.Bili.Models.BiliBili;

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
            TripleResult result = null;
            if (_videoId > 0)
            {
                result = await Controller.TripleAsync(_videoId);
            }
            else if (IsPgc)
            {
                result = await Controller.TripleAsync(CurrentPgcEpisode.Aid);
            }

            if (result != null)
            {
                IsLikeChecked = result.IsLike;
                IsCoinChecked = result.IsCoin;
                IsFavoriteChecked = result.IsFavorite;
            }
        }
    }
}
