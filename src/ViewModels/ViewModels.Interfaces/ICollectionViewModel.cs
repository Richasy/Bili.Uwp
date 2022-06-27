// Copyright (c) Richasy. All rights reserved.

using System;

namespace Bili.ViewModels.Interfaces
{
    /// <summary>
    /// 包含集合的视图模型.
    /// </summary>
    public interface ICollectionViewModel
    {
        /// <summary>
        /// 集合初始化完成.
        /// </summary>
        event EventHandler CollectionInitialized;
    }
}
