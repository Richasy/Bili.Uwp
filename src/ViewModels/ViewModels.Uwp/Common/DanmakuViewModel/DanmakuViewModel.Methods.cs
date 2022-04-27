// Copyright (c) Richasy. All rights reserved.

using System.Linq;
using System.Threading.Tasks;
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
        /// <returns><see cref="Task"/>.</returns>
        public async Task RequestNewSegmentDanmakuAsync(int newSegmentIndex)
        {
            if (_isRequestingDanmaku)
            {
                return;
            }

            _isRequestingDanmaku = true;
            try
            {
                await Controller.RequestNewSegmentDanmakuAsync(_videoId, _partId, newSegmentIndex);
            }
            catch (System.Exception)
            {
                throw;
            }
            finally
            {
                _isRequestingDanmaku = false;
            }
        }

        private void OnSegmentDanmakuIteration(object sender, SegmentDanmakuIterationEventArgs e)
        {
            if (e.VideoId == _videoId && e.PartId == _partId)
            {
                var list = e.DanmakuList.ToList();
                DanmakuListAdded?.Invoke(this, list);
            }
        }

        /// <summary>
        /// 转换为弹幕颜色.
        /// </summary>
        /// <param name="hexColor">HEX颜色.</param>
        /// <returns>颜色字符串.</returns>
        private string ToDanmakuColor(string hexColor)
        {
            var color = Microsoft.Toolkit.Uwp.Helpers.ColorHelper.ToColor(hexColor);
            var num = (color.R * 256 * 256) + (color.G * 256) + (color.B * 1);
            return num.ToString();
        }
    }
}
