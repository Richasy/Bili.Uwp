// Copyright (c) Richasy. All rights reserved.

using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.UI.Xaml.Controls;

namespace Richasy.Bili.ViewModels.Uwp
{
    /// <summary>
    /// 可索引的可观察集合.
    /// </summary>
    /// <typeparam name="T">类型.</typeparam>
    public class IndexObservableCollection<T>
        : ObservableCollection<T>, IKeyIndexMapping
        where T : IIndexViewModel
    {
        /// <inheritdoc/>
        public string KeyFromIndex(int index) => this[index].Id;

        /// <inheritdoc/>
        public int IndexFromKey(string key) => IndexOf(this.FirstOrDefault(p => p.Id == key));
    }
}
