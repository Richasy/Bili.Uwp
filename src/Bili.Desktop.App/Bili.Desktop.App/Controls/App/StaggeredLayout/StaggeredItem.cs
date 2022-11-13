// Copyright (c) Richasy. All rights reserved.

using Microsoft.UI.Xaml;

namespace Bili.Desktop.App.Controls
{
    internal class StaggeredItem
    {
        public StaggeredItem(int index)
        {
            Index = index;
        }

        public double Top { get; internal set; }

        public double Height { get; internal set; }

        public int Index { get; }

        public UIElement Element { get; internal set; }
    }
}
