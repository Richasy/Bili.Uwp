// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using System.Threading.Tasks;
using Bilibili.App.Show.V1;

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
        /// <returns>排行榜信息.</returns>
        public async Task<List<Item>> GetRankAsync(int partitionId)
        {
            ThrowWhenNetworkUnavaliable();

            try
            {
                return await _rankProvider.GetRankDetailAsync(partitionId);
            }
            catch (System.Exception ex)
            {
                _loggerModule.LogError(ex);
                throw;
            }
        }
    }
}
