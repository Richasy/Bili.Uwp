// Copyright (c) Richasy. All rights reserved.

using System;
using Bili.Workspace.Resources.Extension;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Bili.Workspace.Controls.App
{
    /// <summary>
    /// 视频视图.
    /// </summary>
    public sealed partial class VerticalRepeaterView : Control
    {
        private readonly double _itemHolderHeight = 24d;
        private ScrollViewer _parentScrollViewer;

        /// <summary>
        /// Initializes a new instance of the <see cref="VerticalRepeaterView"/> class.
        /// </summary>
        public VerticalRepeaterView()
        {
            DefaultStyleKey = typeof(VerticalRepeaterView);
            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (EnableDetectParentScrollViewer)
            {
                _parentScrollViewer = this.FindAscendantElementByType<ScrollViewer>();
                if (_parentScrollViewer != null)
                {
                    _parentScrollViewer.ViewChanged += OnParentScrollViewerViewChanged;
                }
            }
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            if (_parentScrollViewer != null)
            {
                _parentScrollViewer.ViewChanged -= OnParentScrollViewerViewChanged;
                _parentScrollViewer = null;
            }
        }

        private void OnParentScrollViewerViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            if (!e.IsIntermediate && _parentScrollViewer != null)
            {
                var currentPosition = _parentScrollViewer.VerticalOffset;
                if (_parentScrollViewer.ScrollableHeight - currentPosition <= _itemHolderHeight &&
                    Visibility == Visibility.Visible)
                {
                    RequestLoadMore?.Invoke(this, EventArgs.Empty);
                }
            }
        }
    }
}
