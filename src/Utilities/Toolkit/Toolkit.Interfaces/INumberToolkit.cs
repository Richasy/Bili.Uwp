// Copyright (c) Richasy. All rights reserved.

using System;

namespace Richasy.Bili.Toolkit.Interfaces
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
        /// 获取次数的中文简写文本.
        /// </summary>
        /// <param name="count">次数.</param>
        /// <returns>简写文本.</returns>
        string GetCountText(double count);
    }
}
