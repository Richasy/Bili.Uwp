// Copyright (c) Richasy. All rights reserved.

using Windows.UI.Xaml;

namespace Richasy.Bili.App.Controls
{
    internal class StaggeredItem
    {
        public StaggeredItem(int index)
        {
            this.Index = index;
        }

        public double Top { get; internal set; }

        public double Height { get; internal set; }

        public int Index { get; }

        public UIElement Element { get; internal set; }
    }
}
