// Copyright (c) Richasy. All rights reserved.

using System.Threading.Tasks;
using Richasy.Bili.Models.App.Args;
using Richasy.Bili.Models.App.Other;

namespace Richasy.Bili.Controller.Uwp
{
    /// <summary>
    /// 控制器中处理直播的模块.
    /// </summary>
    public partial class BiliController
    {
        /// <summary>
        /// 请求直播源列表.
        /// </summary>
        /// <param name="pageNumber">页码.</param>
        /// <returns><see cref="Task"/>.</returns>
        public async Task RequestLiveFeedsAsync(int pageNumber)
        {
            try
            {
                ThrowWhenNetworkUnavaliable();
                var data = await _liveProvider.GetLiveFeedsAsync(pageNumber);
                if (pageNumber == 1)
                {
                    var additionalArgs = LiveFeedAdditionalDataChangedEventArgs.Create(data);
                    if (additionalArgs != null)
                    {
                        LiveFeedAdditionalDataChanged?.Invoke(this, additionalArgs);
                    }
                }

                LiveFeedRoomIteration?.Invoke(this, new LiveFeedRoomIterationEventArgs(data, pageNumber + 1));
            }
            catch (ServiceException)
            {
                if (pageNumber == 1)
                {
                    throw;
                }
            }
        }
    }
}
