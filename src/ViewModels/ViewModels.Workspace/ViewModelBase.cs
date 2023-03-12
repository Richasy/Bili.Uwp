// Copyright (c) Richasy. All rights reserved.

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Bili.ViewModels.Workspace
{
    /// <summary>
    /// 视图模型基类.
    /// </summary>
    public class ViewModelBase : ObservableObject
    {
        /// <summary>
        /// 尝试清除集合，仅在集合内有数据时才执行.
        /// </summary>
        /// <typeparam name="T">数据类型.</typeparam>
        /// <param name="collection">集合.</param>
        protected void TryClear<T>(ObservableCollection<T> collection)
        {
            if (collection.Count > 0)
            {
                collection.Clear();
            }
        }
    }
}
