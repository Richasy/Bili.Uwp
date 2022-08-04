// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Atelier39;
using Bili.Models.Data.Live;
using Windows.UI.Xaml;

namespace Bili.App.Controls.Danmaku
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
        /// <param name="isAdd">是否为添加弹幕.</param>
        public void Prepare(List<DanmakuItem> items, bool isAdd = false)
        {
            if (isAdd)
            {
                _danmakuController?.AddDanmakuList(items);
            }
            else
            {
                _danmakuController?.SetDanmakuList(items);
            }

            _cachedDanmakus = items;
        }

        /// <summary>
        /// 发送弹幕.
        /// </summary>
        /// <param name="item">弹幕条目.</param>
        public void SendDanmu(DanmakuItem item)
            => _danmakuController?.AddRealtimeDanmaku(item, insertToList: false);

        /// <summary>
        /// 更新内部计时.
        /// </summary>
        /// <param name="milliseconds">毫秒时间戳.</param>
        public void UpdateTime(uint milliseconds)
        {
            _canvas?.UpdateLayout();
            _danmakuController?.UpdateTime(milliseconds);
            _currentTs = milliseconds;
        }

        /// <summary>
        /// 暂停弹幕.
        /// </summary>
        public void PauseDanmaku() => _danmakuController?.Pause();

        /// <summary>
        /// 继续弹幕.
        /// </summary>
        public void ResumeDanmaku() => _danmakuController?.Resume();

        /// <summary>
        /// 重新绘制弹幕区域.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        public async Task RedrawAsync()
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                _danmakuController?.Close();

                InitializeController();
                if (_cachedDanmakus?.Count > 0)
                {
                    Prepare(_cachedDanmakus);
                    _danmakuController?.Resume();
                    _danmakuController?.Seek(_currentTs);
                }
            });
        }

        /// <summary>
        /// 清空弹幕.
        /// </summary>
        public void ClearAll()
        {
            _cachedDanmakus?.Clear();
            _currentTs = 0;
            _danmakuController?.Clear();
        }

        private static void OnProgressPositionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as DanmakuView;
            var sec = (double)e.NewValue;
            instance.UpdateTime(Convert.ToUInt32(sec * 1000));
        }

        /// <summary>
        /// 关闭控制器.
        /// </summary>
        private void Close()
            => _danmakuController?.Close();

        private void OnLiveDanmakuAdded(object sender, LiveDanmakuInformation e)
        {
            if (!ViewModel.IsShowDanmaku)
            {
                return;
            }

            var model = new DanmakuItem
            {
                StartMs = 0,
                Mode = DanmakuMode.Rolling,
                TextColor = Microsoft.Toolkit.Uwp.Helpers.ColorHelper.ToColor(e.TextColor ?? "#FFFFFF"),
                BaseFontSize = ViewModel.IsStandardSize ? 20 : 24,
                Text = e.Text,
                HasOutline = true,
            };

            SendDanmu(model);
        }

        private void OnSendDanmakuSucceeded(object sender, string e)
        {
            var model = new DanmakuItem
            {
                StartMs = _currentTs,
                Mode = (DanmakuMode)((int)ViewModel.Location),
                TextColor = Microsoft.Toolkit.Uwp.Helpers.ColorHelper.ToColor(ViewModel.Color),
                BaseFontSize = ViewModel.IsStandardSize ? 20 : 24,
                Text = e,
                HasOutline = true,
            };

            SendDanmu(model);
        }
    }
}
