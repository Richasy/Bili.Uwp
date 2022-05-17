// Copyright (c) Richasy. All rights reserved.

namespace Bili.Lib.Interfaces
{
    /// <summary>
    /// 支持清除缓存数据的 Provider.
    /// </summary>
    public interface IClearProvider
    {
        /// <summary>
        /// 清除本地的缓存数据.
        /// </summary>
        void Clear();
    }
}
