// Copyright (c) GodLeaveMe. All rights reserved.

using System;

namespace Richasy.Bili.Toolkit.Interfaces
{
    /// <summary>
    /// MD5工具类.
    /// </summary>
    public interface IMD5Toolkit : IDisposable
    {
        /// <summary>
        /// 获取转换后的MD5字符串.
        /// </summary>
        /// <param name="source">输入字符串.</param>
        /// <returns>转换后的结果.</returns>
        string GetMd5String(string source);
    }
}
