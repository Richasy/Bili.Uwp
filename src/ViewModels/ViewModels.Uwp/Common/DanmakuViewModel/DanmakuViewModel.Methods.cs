// Copyright (c) Richasy. All rights reserved.

using Richasy.Bili.Models.App.Args;

namespace Richasy.Bili.ViewModels.Uwp.Common
{
    /// <summary>
    /// 弹幕视图模型.
    /// </summary>
    public partial class DanmakuViewModel
    {
        /// <summary>
        /// 请求新分片弹幕.
        /// </summary>
        /// <param name="newSegmentIndex">新的分片索引.</param>
        public async void RequestNewSegmentDanmakuAsync(int newSegmentIndex)
        {
            await Controller.RequestNewSegmentDanmakuAsync(_videoId, _partId, newSegmentIndex);
        }

        private void OnSegmentDanmakuIteration(object sender, SegmentDanmakuIterationEventArgs e)
        {
            if (e.VideoId == _videoId && e.PartId == _partId)
            {
                e.DanmakuList.ForEach(p => _danmakuList.Add(p));
                DanmakuListAdded?.Invoke(this, e.DanmakuList);
            }
        }
    }
}
