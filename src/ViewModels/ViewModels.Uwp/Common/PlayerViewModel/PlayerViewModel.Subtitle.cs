// Copyright (c) Richasy. All rights reserved.

using System;
using System.Linq;
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
        /// 初始化字幕索引.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        public async Task InitializeSubtitleIndexAsync()
        {
            if (!IsLive)
            {
                var aid = IsPgc ? CurrentPgcEpisode.Aid : Convert.ToInt32(_videoId);
                var cid = IsPgc ? CurrentPgcEpisode.PartId : Convert.ToInt32(CurrentVideoPart.Page.Cid);

                try
                {
                    var index = await Controller.GetSubtitleIndexAsync(aid, cid);
                    if (index != null && index.Subtitles != null && index.Subtitles.Count > 0)
                    {
                        IsShowSubtitleButton = true;
                        index.Subtitles.ForEach(p => SubtitleIndexCollection.Add(new SubtitleIndexItemViewModel(p, false)));
                        var first = SubtitleIndexCollection.FirstOrDefault();
                        if (first != null)
                        {
                            CurrentSubtitleIndex = first.Data;
                            first.IsSelected = true;
                            await InitializeSubtitleAsync(first.Data);
                        }
                    }
                    else
                    {
                        IsShowSubtitleButton = false;
                    }
                }
                catch (Exception)
                {
                    IsShowSubtitle = false;
                    IsShowSubtitleButton = false;
                }
            }
        }

        /// <summary>
        /// 初始化字幕.
        /// </summary>
        /// <param name="item">字幕索引条目.</param>
        /// <returns><see cref="Task"/>.</returns>
        public async Task InitializeSubtitleAsync(SubtitleIndexItem item)
        {
            _subtitleTimer.Stop();
            _subtitleList.Clear();
            IsShowSubtitle = false;

            if (string.IsNullOrEmpty(item.Url))
            {
                return;
            }

            try
            {
                var result = await Controller.GetSubtitleDetailAsync(item.Url);
                _subtitleList = result.Body;
                _subtitleTimer.Start();
            }
            catch (Exception ex)
            {
                var exception = new Exception($"字幕加载失败。\n地址：{item.Url}\n错误信息：{ex.Message}");
                _logger.LogError(exception, true);
            }
        }

        /// <summary>
        /// 检查字幕选中状态.
        /// </summary>
        public void CheckSubtitleSelection()
        {
            foreach (var item in SubtitleIndexCollection)
            {
                item.IsSelected = item.Data.Id == CurrentSubtitleIndex.Id;
            }
        }
    }
}
