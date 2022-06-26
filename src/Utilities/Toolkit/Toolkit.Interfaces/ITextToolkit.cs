// Copyright (c) Richasy. All rights reserved.

namespace Bili.Toolkit.Interfaces
{
    /// <summary>
    /// 文本工具接口定义.
    /// </summary>
    public interface ITextToolkit
    {
        /// <summary>
        /// 如果当前语言环境为繁体中文，那就将传入文本转换为繁体中文.
        /// </summary>
        /// <param name="text">文本.</param>
        /// <returns>繁体文本或普通文本.</returns>
        string ConvertToTraditionalChineseIfNeeded(string text);
    }
}
