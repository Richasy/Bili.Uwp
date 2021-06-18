// Copyright (c) Richasy. All rights reserved.

using System.Threading.Tasks;
using Richasy.Bili.Models.BiliBili;
using Richasy.Bili.Models.Enums;

namespace Richasy.Bili.Controller.Uwp
{
    /// <summary>
    /// 控制器的排行榜部分.
    /// </summary>
    public partial class BiliController
    {
        /// <summary>
        /// 获取排行榜信息.
        /// </summary>
        /// <param name="partitionId">分区Id.</param>
        /// <param name="scope">排行榜范围.</param>
        /// <returns>排行榜信息.</returns>
        public async Task<RankInfo> GetRankAsync(int partitionId, RankScope scope)
        {
            try
            {
                var rank = await _rankProvider.GetRankDetailAsync(partitionId, scope);
                return rank;
            }
            catch
            {
                throw;
            }
        }
    }
}
