﻿// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Atelier39;

namespace Richasy.Bili.App.Controls
{
    /// <summary>
    /// 弹幕视图.
    /// </summary>
    public sealed partial class DanmakuView
    {
        /// <summary>
        /// 发送弹幕.
        /// </summary>
        /// <param name="items">弹幕列表.</param>
        public void Prepare(List<DanmakuItem> items)
        {
            _danmakuController.SetIsTextBold(DanmakuBold);
            _danmakuController.SetFontFamilyName(DanmakuFontFamily);
            _danmakuController.SetDanmakuList(items);
            _cachedDanmakus = items;
        }

        /// <summary>
        /// 发送弹幕.
        /// </summary>
        /// <param name="item">弹幕条目.</param>
        public void SendDanmu(DanmakuItem item)
        {
            _danmakuController.AddRealtimeDanmaku(item, insertToList: false);
        }

        /// <summary>
        /// 更新内部计时.
        /// </summary>
        /// <param name="milliseconds">毫秒时间戳.</param>
        public void Update(uint milliseconds)
        {
            _danmakuController?.UpdateTime(milliseconds);
            _currentTs = milliseconds;
        }

        /// <summary>
        /// 暂停弹幕.
        /// </summary>
        public void PauseDanmaku()
        {
            _danmakuController?.Pause();
        }

        /// <summary>
        /// 继续弹幕.
        /// </summary>
        public void ResumeDanmaku()
        {
            _danmakuController?.Resume();
        }

        /// <summary>
        /// 重新绘制弹幕区域.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        public async Task RedrawAsync()
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High, () =>
            {
                _danmakuController?.Close();
                InitializeController();
                if (_cachedDanmakus?.Count > 0)
                {
                    Prepare(_cachedDanmakus);
                    _danmakuController.Resume();
                    _danmakuController.Seek(_currentTs);
                }
            });
        }

        /// <summary>
        /// 清空弹幕.
        /// </summary>
        public void ClearAll()
        {
            if (!_isApplyTemplate)
            {
                return;
            }

            _cachedDanmakus?.Clear();
            _currentTs = 0;
            _danmakuController?.Clear();
        }
    }
}
