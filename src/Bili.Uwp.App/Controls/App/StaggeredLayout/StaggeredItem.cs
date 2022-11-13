// Copyright (c) Richasy. All rights reserved.

using Windows.UI.Xaml;

namespace Bili.Uwp.App.Controls
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
