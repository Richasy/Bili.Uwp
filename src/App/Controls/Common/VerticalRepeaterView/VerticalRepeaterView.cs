// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections;
using System.Diagnostics;
using Bili.App.Resources.Extension;
using Microsoft.UI.Xaml.Controls;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Bili.App.Controls
{
    /// <summary>
    /// 视频视图.
    /// </summary>
    public sealed partial class VerticalRepeaterView : Control
    {
        private ScrollViewer _parentScrollViewer;
        private ItemsRepeater _itemsRepeater;

        private double _itemHolderHeight = 0d;
        private bool _isTempalteApplied = false;
        private bool _isLoaded = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="VerticalRepeaterView"/> class.
        /// </summary>
        public VerticalRepeaterView()
        {
            DefaultStyleKey = typeof(VerticalRepeaterView);
            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
        }

        /// <summary>
        /// 滚动到条目.
        /// </summary>
        /// <param name="index">条目索引值.</param>
        public void ScrollToItem(int index)
        {
            if (_itemsRepeater != null)
            {
                var element = _itemsRepeater.GetOrCreateElement(index);
                var options = new BringIntoViewOptions
                {
                    VerticalAlignmentRatio = 0f,
                };
                element.StartBringIntoView(options);
            }
        }

        /// <inheritdoc/>
        protected override void OnApplyTemplate()
        {
            _itemsRepeater = GetTemplateChild("ItemsRepeater") as ItemsRepeater;

            if (_itemsRepeater != null)
            {
                _itemsRepeater.ElementPrepared += OnElementPrepared;
            }

            _isTempalteApplied = true;
            if (_isTempalteApplied && _isLoaded)
            {
                CheckOrientationStatus();
            }
        }

        private static void OnIsStaggeredChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as VerticalRepeaterView;
            instance.CheckOrientationStatus();
        }

        private static void OnOrientationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as VerticalRepeaterView;
            if (e.NewValue is Orientation)
            {
                instance.CheckOrientationStatus();
            }
        }

        private void CheckOrientationStatus()
        {
            if (_itemsRepeater != null)
            {
                _itemsRepeater.Layout = GetLayout();
            }

            ChangeInitializedItemOrientation();
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

            _isLoaded = true;
            if (_isLoaded && _isTempalteApplied)
            {
                CheckOrientationStatus();
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

        private void ChangeInitializedItemOrientation()
        {
            if (ItemsSource is ICollection items)
            {
                for (var i = 0; i < items.Count; i++)
                {
                    var element = _itemsRepeater?.TryGetElement(i);
                    if (element != null)
                    {
                        if (element is IDynamicLayoutItem vi)
                        {
                            if (vi.Orientation != ItemOrientation)
                            {
                                vi.Orientation = ItemOrientation;
                            }
                        }
                        else if (element is IOrientationControl oc)
                        {
                            oc.ChangeLayout(ItemOrientation);
                        }
                    }
                }
            }
        }

        private void OnElementPrepared(ItemsRepeater sender, ItemsRepeaterElementPreparedEventArgs args)
        {
            if (args.Element != null)
            {
                if (args.Element is IOrientationControl orientationControl)
                {
                    orientationControl.ChangeLayout(ItemOrientation);
                }

                if (args.Element is IDynamicLayoutItem dynamicLayoutItem)
                {
                    dynamicLayoutItem.Orientation = ItemOrientation;
                }

                if (IsAutoFillEnable &&
                    args.Element is IRepeaterItem repeaterItem &&
                    ItemsSource is ICollection collectionSource &&
                    (_parentScrollViewer != null) &&
                    args.Index >= collectionSource.Count - 1)
                {
                    var size = repeaterItem.GetHolderSize();
                    _itemHolderHeight = size.Height;
                    var viewportWidth = _parentScrollViewer.ViewportWidth;
                    var viewportHeight = _parentScrollViewer.ViewportHeight;
                    bool isNeedLoadMore;
                    if (double.IsInfinity(size.Width))
                    {
                        isNeedLoadMore = (args.Index + 1) * size.Height <= viewportHeight;
                    }
                    else
                    {
                        var rowCount = args.Index / (viewportWidth / size.Width);
                        isNeedLoadMore = rowCount * size.Height <= viewportHeight;
                    }

                    if (isNeedLoadMore)
                    {
                        RequestLoadMore?.Invoke(this, EventArgs.Empty);
                    }
                }
            }
        }

        private VirtualizingLayout GetLayout()
        {
            VirtualizingLayout layout = null;
            if (IsStaggered)
            {
                layout = new StaggeredLayout
                {
                    ColumnSpacing = 16,
                    DesiredColumnWidth = MinWideItemWidth,
                    RowSpacing = 16,
                };
            }
            else
            {
                switch (ItemOrientation)
                {
                    case Orientation.Vertical:
                        layout = new UniformGridLayout()
                        {
                            ItemsStretch = UniformGridLayoutItemsStretch.Fill,
                            MinColumnSpacing = 16,
                            MinItemHeight = MinWideItemHeight,
                            MinItemWidth = MinWideItemWidth,
                            MinRowSpacing = 16,
                            Orientation = Orientation.Horizontal,
                        };
                        break;
                    case Orientation.Horizontal:
                        layout = new StackLayout()
                        {
                            Orientation = Orientation.Vertical,
                            Spacing = 8,
                        };
                        break;
                    default:
                        break;
                }
            }

            return layout;
        }
    }
}
