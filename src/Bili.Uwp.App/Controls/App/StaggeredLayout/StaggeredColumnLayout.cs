// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;

namespace Bili.Uwp.App.Controls
{
    [System.Diagnostics.DebuggerDisplay("Count = {Count}, Height = {Height}")]
    internal class StaggeredColumnLayout : List<StaggeredItem>
    {
        public double Height { get; private set; }

        public new void Add(StaggeredItem item)
        {
            Height = item.Top + item.Height;
            base.Add(item);
        }

        public new void Clear()
        {
            Height = 0;
            base.Clear();
        }
    }
}
