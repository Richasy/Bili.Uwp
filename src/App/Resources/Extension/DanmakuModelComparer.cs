// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using NSDanmaku.Model;

namespace Richasy.Bili.App.Resources.Extension
{
    /// <summary>
    /// <see cref="DanmakuModel"/>的比较器.
    /// </summary>
    public class DanmakuModelComparer : IEqualityComparer<DanmakuModel>
    {
        /// <inheritdoc/>
        public bool Equals(DanmakuModel x, DanmakuModel y)
        {
            return x.text == y.text;
        }

        /// <inheritdoc/>
        public int GetHashCode(DanmakuModel obj)
        {
            return obj.text.GetHashCode();
        }
    }
}
