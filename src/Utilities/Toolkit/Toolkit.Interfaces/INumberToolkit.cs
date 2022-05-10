﻿// Copyright (c) Richasy. All rights reserved.

using System;

namespace Bili.Toolkit.Interfaces
{
    /// <summary>
    /// 数字处理工具.
    /// </summary>
    public interface INumberToolkit
    {
        /// <summary>
        /// 获取时长的文本描述.
        /// </summary>
        /// <param name="timeSpan">时长.</param>
        /// <returns>文本描述.</returns>
        string GetDurationText(TimeSpan timeSpan);

        /// <summary>
        /// 对网络数据的时长字符串进行格式调整，网络数据格式为：hh:mm:ss.
        /// </summary>
        /// <param name="webDurationText">网络时长数据.</param>
        /// <returns>重新解析后的可读时长文本.</returns>
        string FormatDurationText(string webDurationText);

        /// <summary>
        /// 获取次数的中文简写文本.
        /// </summary>
        /// <param name="count">次数.</param>
        /// <returns>简写文本.</returns>
        string GetCountText(double count);

        /// <summary>
        /// 将数字简写文本中转换为大略的次数.
        /// </summary>
        /// <param name="text">数字简写文本.</param>
        /// <param name="removeText">需要先在简写文本中移除的内容.</param>
        /// <returns>一个大概的次数，比如 <c>3.2万播放</c>，最终会返回 <c>32000</c>.</returns>
        double GetCountNumber(string text, string removeText = "");
    }
}
