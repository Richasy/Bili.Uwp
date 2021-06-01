// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Microsoft.UI.Xaml.Controls;
using Windows.Foundation;
using Windows.UI.Xaml;

namespace Richasy.Bili.App.Controls
{
    internal class ElementManager
    {
        private readonly List<UIElement> _realizedElements = new List<UIElement>();
        private readonly List<Rect> _realizedElementLayoutBounds = new List<Rect>();
        private int _firstRealizedDataIndex = -1;
        private VirtualizingLayoutContext _context;

        private bool IsVirtualizingContext
        {
            get
            {
                if (_context != null)
                {
                    var rect = _context.RealizationRect;
                    var hasInfiniteSize = double.IsInfinity(rect.Height) || double.IsInfinity(rect.Width);
                    return !hasInfiniteSize;
                }

                return false;
            }
        }

        public int GetGetRealizedElementCount() => IsVirtualizingContext ? _realizedElements.Count : _context.ItemCount;

        public void SetContext(VirtualizingLayoutContext virtualContext)
        {
            _context = virtualContext;
        }

        public void OnBeginMeasure(ScrollOrientation orientation)
        {
            if (_context != null)
            {
                if (IsVirtualizingContext)
                {
                    // We proactively clear elements laid out outside of the realizaton
                    // rect so that they are available for reuse during the current
                    // measure pass.
                    // This is useful during fast panning scenarios in which the realization
                    // window is constantly changing and we want to reuse elements from
                    // the end that's opposite to the panning direction.
                    DiscardElementsOutsideWindow(_context.RealizationRect, orientation);
                }
                else
                {
                }
            }
        }

        public int GetRealizedElementCount()
        {
            return IsVirtualizingContext ? _realizedElements.Count : _context.ItemCount;
        }

        public UIElement GetAt(int realizedIndex)
        {
            UIElement element;
            if (IsVirtualizingContext)
            {
                if (!_realizedElements.TryGetElementAt(realizedIndex, out element))
                {
                    // Sentinel. Create the element now since we need it.
                    var dataIndex = GetDataIndexFromRealizedRangeIndex(realizedIndex);
                    element = _context.GetOrCreateElementAt(dataIndex, ElementRealizationOptions.ForceCreate | ElementRealizationOptions.SuppressAutoRecycle);
                    _realizedElements[realizedIndex] = element;
                }
            }
            else
            {
                // realizedIndex and dataIndex are the same (everything is realized)
                element = _context.GetOrCreateElementAt(realizedIndex, ElementRealizationOptions.ForceCreate | ElementRealizationOptions.SuppressAutoRecycle);
            }

            return element;
        }

        public void Add(UIElement element, int dataIndex)
        {
            if (_realizedElements.Count == 0)
            {
                _firstRealizedDataIndex = dataIndex;
            }

            _realizedElements.Add(element);
            _realizedElementLayoutBounds.Add(default);
        }

        public void Insert(int realizedIndex, int dataIndex, UIElement element)
        {
            if (realizedIndex == 0)
            {
                _firstRealizedDataIndex = dataIndex;
            }

            _realizedElements.AddOrInsert(realizedIndex, element);

            // Set bounds to an invalid rect since we do not know it yet.
            _realizedElementLayoutBounds.AddOrInsert(realizedIndex, new Rect(-1, -1, -1, -1));
        }

        public void ClearRealizedRange(int realizedIndex, int count)
        {
            for (var i = 0; i < count; i++)
            {
                // Clear from the edges so that ItemsRepeater can optimize on maintaining,
                // realized indices without walking through all the children every time.
                var index = realizedIndex == 0 ? realizedIndex + i : realizedIndex + count - 1 - i;
                if (_realizedElements.TryGetElementAt(index, out var elementRef))
                {
                    _context.RecycleElement(elementRef);
                }
            }

            _realizedElements.RemoveRange(realizedIndex, count);
            _realizedElementLayoutBounds.RemoveRange(realizedIndex, count);

            if (realizedIndex == 0)
            {
                _firstRealizedDataIndex = _realizedElements.Count == 0 ? -1 : _firstRealizedDataIndex + count;
            }
        }

        public void DiscardElementsOutsideWindow(bool forward, int startIndex)
        {
            // Remove layout elements that are outside the realized range.
            if (IsDataIndexRealized(startIndex))
            {
                var rangeIndex = GetRealizedRangeIndexFromDataIndex(startIndex);

                if (forward)
                {
                    ClearRealizedRange(rangeIndex, GetGetRealizedElementCount() - rangeIndex);
                }
                else
                {
                    ClearRealizedRange(0, rangeIndex + 1);
                }
            }
        }

        public void ClearRealizedRange()
        {
            ClearRealizedRange(0, GetGetRealizedElementCount());
        }

        public Rect GetLayoutBoundsForDataIndex(int dataIndex)
        {
            var realizedIndex = GetRealizedRangeIndexFromDataIndex(dataIndex);
            return _realizedElementLayoutBounds[realizedIndex];
        }

        public void SetLayoutBoundsForDataIndex(int dataIndex, Rect bounds)
        {
            var realizedIndex = GetRealizedRangeIndexFromDataIndex(dataIndex);
            _realizedElementLayoutBounds[realizedIndex] = bounds;
        }

        public Rect GetLayoutBoundsForRealizedIndex(int realizedIndex)
        {
            return _realizedElementLayoutBounds[realizedIndex];
        }

        public void SetLayoutBoundsForRealizedIndex(int realizedIndex, Rect bounds)
        {
            _realizedElementLayoutBounds[realizedIndex] = bounds;
        }

        public bool IsDataIndexRealized(int index)
        {
            if (IsVirtualizingContext)
            {
                var realizedCount = GetGetRealizedElementCount();
                return
                    realizedCount > 0 &&
                    GetDataIndexFromRealizedRangeIndex(0) <= index &&
                    GetDataIndexFromRealizedRangeIndex(realizedCount - 1) >= index;
            }
            else
            {
                // Non virtualized - everything is realized
                return index >= 0 && index < _context.ItemCount;
            }
        }

        public bool IsIndexValidInData(int currentIndex)
        {
            return currentIndex >= 0 && currentIndex < _context.ItemCount;
        }

        public UIElement GetRealizedElement(int dataIndex)
        {
            return IsVirtualizingContext
                ? GetAt(GetRealizedRangeIndexFromDataIndex(dataIndex))
                : _context.GetOrCreateElementAt(dataIndex, ElementRealizationOptions.ForceCreate | ElementRealizationOptions.SuppressAutoRecycle);
        }

        public void EnsureElementRealized(bool forward, int dataIndex, string layoutId)
        {
            if (IsDataIndexRealized(dataIndex) == false)
            {
                var element = _context.GetOrCreateElementAt(dataIndex, ElementRealizationOptions.ForceCreate | ElementRealizationOptions.SuppressAutoRecycle);

                if (forward)
                {
                    Add(element, dataIndex);
                }
                else
                {
                    Insert(0, dataIndex, element);
                }
            }
        }

        // Does the given window intersect the range of realized elements
        public bool IsWindowConnected(Rect window, ScrollOrientation orientation, bool scrollOrientationSameAsFlow)
        {
            var intersects = false;
            if (_realizedElementLayoutBounds.Count > 0)
            {
                var firstElementBounds = GetLayoutBoundsForRealizedIndex(0);
                var lastElementBounds = GetLayoutBoundsForRealizedIndex(GetGetRealizedElementCount() - 1);

                var effectiveOrientation = scrollOrientationSameAsFlow ? (orientation == ScrollOrientation.Vertical ? ScrollOrientation.Horizontal : ScrollOrientation.Vertical) : orientation;
                var windowStart = effectiveOrientation == ScrollOrientation.Vertical ? window.Y : window.X;
                var windowEnd = effectiveOrientation == ScrollOrientation.Vertical ? window.Y + window.Height : window.X + window.Width;
                var firstElementStart = effectiveOrientation == ScrollOrientation.Vertical ? firstElementBounds.Y : firstElementBounds.X;
                var lastElementEnd = effectiveOrientation == ScrollOrientation.Vertical ? lastElementBounds.Y + lastElementBounds.Height : lastElementBounds.X + lastElementBounds.Width;

                intersects =
                    firstElementStart <= windowEnd &&
                    lastElementEnd >= windowStart;
            }

            return intersects;
        }

        public void DataSourceChanged(object source, NotifyCollectionChangedEventArgs args)
        {
            if (_realizedElements.Count > 0)
            {
                switch (args.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        {
                            OnItemsAdded(args.NewStartingIndex, args.NewItems.Count);
                        }

                        break;
                    case NotifyCollectionChangedAction.Replace:
                        {
                            var oldSize = args.OldItems.Count;
                            var newSize = args.NewItems.Count;
                            var oldStartIndex = args.OldStartingIndex;
                            var newStartIndex = args.NewStartingIndex;

                            if (oldSize == newSize &&
                                oldStartIndex == newStartIndex &&
                                IsDataIndexRealized(oldStartIndex) &&
                                IsDataIndexRealized(oldStartIndex + oldSize - 1))
                            {
                                // Straight up replace of n items within the realization window.
                                // Removing and adding might causes us to lose the anchor causing us
                                // to throw away all containers and start from scratch.
                                // Instead, we can just clear those items and set the element to
                                // null (sentinel) and let the next measure get new containers for them.
                                var startRealizedIndex = GetRealizedRangeIndexFromDataIndex(oldStartIndex);
                                for (var realizedIndex = startRealizedIndex; realizedIndex < startRealizedIndex + oldSize; realizedIndex++)
                                {
                                    if (_realizedElements.TryGetElementAt(realizedIndex, out var elementRef))
                                    {
                                        _context.RecycleElement(elementRef);
                                        _realizedElements[realizedIndex] = null; // TODO UNO .... weird!!!
                                    }
                                }
                            }
                            else
                            {
                                OnItemsRemoved(oldStartIndex, oldSize);
                                OnItemsAdded(newStartIndex, newSize);
                            }
                        }

                        break;

                    case NotifyCollectionChangedAction.Remove:
                        {
                            OnItemsRemoved(args.OldStartingIndex, args.OldItems.Count);
                        }

                        break;

                    case NotifyCollectionChangedAction.Reset:
                        ClearRealizedRange();
                        break;

                    case NotifyCollectionChangedAction.Move:
                        // Move is not supported by default by ItemsRepeater (throw new NotImplementedException();)
                        // This is Uno specific
                        OnItemsRemoved(args.OldStartingIndex, args.OldItems.Count);
                        OnItemsAdded(args.NewStartingIndex, args.NewItems.Count);
                        break;
                }
            }
        }

        public int GetElementDataIndex(UIElement suggestedAnchor)
        {
            var it = _realizedElements.IndexOf(suggestedAnchor);
            return it != -1
                ? GetDataIndexFromRealizedRangeIndex(it)
                : -1;
        }

        public int GetDataIndexFromRealizedRangeIndex(int rangeIndex)
        {
            return IsVirtualizingContext ? rangeIndex + _firstRealizedDataIndex : rangeIndex;
        }

        public int GetRealizedRangeIndexFromDataIndex(int dataIndex)
        {
            return IsVirtualizingContext ? dataIndex - _firstRealizedDataIndex : dataIndex;
        }

        public void DiscardElementsOutsideWindow(Rect window, ScrollOrientation orientation)
        {
            // The following illustration explains the cutoff indices.
            // We will clear all the realized elements from both ends
            // up to the corresponding cutoff index.
            // '-' means the element is outside the cutoff range.
            // '*' means the element is inside the cutoff range and will be cleared.
            //
            // Window:
            //        |______________________________|
            // Realization range:
            // |*****----------------------------------*********|
            //      |                                  |
            //  frontCutoffIndex                backCutoffIndex
            //
            // Note that we tolerate at most one element outside of the window
            // because the FlowLayoutAlgorithm.Generate routine stops *after*
            // it laid out an element outside the realization window.
            // This is also convenient because it protects the anchor
            // during a BringIntoView operation during which the anchor may
            // not be in the realization window (in fact, the realization window
            // might be empty if the BringIntoView is issued before the first
            // layout pass).
            var realizedRangeSize = GetGetRealizedElementCount();
            var frontCutoffIndex = -1;
            var backCutoffIndex = realizedRangeSize;

            for (var i = 0;
                i < realizedRangeSize &&
                !Intersects(window, _realizedElementLayoutBounds[i], orientation);
                ++i)
            {
                ++frontCutoffIndex;
            }

            for (var i = realizedRangeSize - 1;
                i >= 0 &&
                !Intersects(window, _realizedElementLayoutBounds[i], orientation);
                --i)
            {
                --backCutoffIndex;
            }

            if (backCutoffIndex < realizedRangeSize - 1)
            {
                ClearRealizedRange(backCutoffIndex + 1, realizedRangeSize - backCutoffIndex - 1);
            }

            if (frontCutoffIndex > 0)
            {
                ClearRealizedRange(0, Math.Min(frontCutoffIndex, GetGetRealizedElementCount()));
            }
        }

        /* static */
        private bool Intersects(Rect lhs, Rect rhs, ScrollOrientation orientation)
        {
            var lhsStart = orientation == ScrollOrientation.Vertical ? lhs.Y : lhs.X;
            var lhsEnd = orientation == ScrollOrientation.Vertical ? lhs.Y + lhs.Height : lhs.X + lhs.Width;
            var rhsStart = orientation == ScrollOrientation.Vertical ? rhs.Y : rhs.X;
            var rhsEnd = orientation == ScrollOrientation.Vertical ? rhs.Y + rhs.Height : rhs.X + rhs.Width;

            return lhsEnd >= rhsStart && lhsStart <= rhsEnd;
        }

        private void OnItemsAdded(int index, int count)
        {
            // Using the old indices here (before it was updated by the collection change)
            // if the insert data index is between the first and last realized data index, we need
            // to insert items.
            var lastRealizedDataIndex = _firstRealizedDataIndex + GetGetRealizedElementCount() - 1;
            var newStartingIndex = index;
            if (newStartingIndex > _firstRealizedDataIndex &&
                newStartingIndex <= lastRealizedDataIndex)
            {
                // Inserted within the realized range
                var insertRangeStartIndex = newStartingIndex - _firstRealizedDataIndex;
                for (var i = 0; i < count; i++)
                {
                    // Insert null (sentinel) here instead of an element, that way we dont,
                    // end up creating a lot of elements only to be thrown out in the next layout.
                    var insertRangeIndex = insertRangeStartIndex + i;
                    var dataIndex = newStartingIndex + i;

                    // This is to keep the contiguousness of the mapping
                    Insert(insertRangeIndex, dataIndex, null);
                }
            }
            else if (index <= _firstRealizedDataIndex)
            {
                // Items were inserted before the realized range.
                // We need to update _firstRealizedDataIndex;
                _firstRealizedDataIndex += count;
            }
        }

        private void OnItemsRemoved(int index, int count)
        {
            var lastRealizedDataIndex = _firstRealizedDataIndex + _realizedElements.Count - 1;
            var startIndex = Math.Max(_firstRealizedDataIndex, index);
            var endIndex = Math.Min(lastRealizedDataIndex, index + count - 1);
            var removeAffectsFirstRealizedDataIndex = index <= _firstRealizedDataIndex;

            if (endIndex >= startIndex)
            {
                ClearRealizedRange(GetRealizedRangeIndexFromDataIndex(startIndex), endIndex - startIndex + 1);
            }

            if (removeAffectsFirstRealizedDataIndex &&
                _firstRealizedDataIndex != -1)
            {
                _firstRealizedDataIndex -= count;
            }
        }
    }
}
