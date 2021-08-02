// Copyright (c) Richasy. All rights reserved.

using Richasy.Bili.Models.App.Args;

namespace Richasy.Bili.ViewModels.Uwp
{
    /// <summary>
    /// 播放器视图模型.
    /// </summary>
    public partial class PlayerViewModel
    {
        /// <summary>
        /// 请求新分片弹幕.
        /// </summary>
        /// <param name="newSegmentIndex">新的分片索引.</param>
        public async void RequestNewSegmentDanmakuAsync(int newSegmentIndex)
        {
            await Controller.RequestNewSegmentDanmakuAsync(_detail.Arc.Aid, CurrentPart.Page.Cid, newSegmentIndex);
        }

        private void OnSegmentDanmakuIteration(object sender, SegmentDanmakuIterationEventArgs e)
        {
            if (e.VideoId == _detail.Arc.Aid && e.PartId == CurrentPart.Page.Cid)
            {
                e.DanmakuList.ForEach(p => _danmakuList.Add(p));
                DanmakuListAdded?.Invoke(this, e.DanmakuList);
            }
        }
    }
}
