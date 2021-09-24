// Copyright (c) GodLeaveMe. All rights reserved.

using System.Collections.Generic;
using Richasy.Bili.App.Controls;

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
            return x.Text == y.Text;
        }

        /// <inheritdoc/>
        public int GetHashCode(DanmakuModel obj)
        {
            return obj.Text.GetHashCode();
        }
    }
}
