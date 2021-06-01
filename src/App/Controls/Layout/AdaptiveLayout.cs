// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Specialized;
using Microsoft.UI.Xaml.Controls;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Richasy.Bili.App.Controls
{
    /// <summary>
    /// Defines constants that specify how items are aligned on the non-scrolling or non-virtualizing axis.
    /// </summary>
    public enum AdaptiveLayoutItemsJustification
    {
        /// <summary>
        /// Items are aligned with the start of the row or column, with extra space at the end.
        /// Spacing between items does not change.
        /// </summary>
        Start = 0,

        /// <summary>
        /// Items are aligned in the center of the row or column, with extra space at the start and
        /// end. Spacing between items does not change.
        /// </summary>
        Center = 1,

        /// <summary>
        /// Items are aligned with the end of the row or column, with extra space at the start.
        /// Spacing between items does not change.
        /// </summary>
        End = 2,

        /// <summary>
        /// Items are aligned so that extra space is added evenly before and after each item.
        /// </summary>
        SpaceAround = 3,

        /// <summary>
        /// Items are aligned so that extra space is added evenly between adjacent items. No space
        /// is added at the start or end.
        /// </summary>
        SpaceBetween = 4,

        /// <summary>
        /// Items are aligned so that extra space is added evenly between adjacent items. No space
        /// is added at the start or end.
        /// </summary>
        SpaceEvenly = 5,
    }

    /// <summary>
    /// Defines constants that specify how items are sized to fill the available space.
    /// </summary>
    public enum AdaptiveLayoutItemsStretch
    {
        /// <summary>
        /// The item retains its natural size. Use of extra space is determined by the
        /// <see cref="AdaptiveLayout.ItemsJustification"/> property.
        /// </summary>
        None = 0,

        /// <summary>
        /// The item is sized to fill the available space in the non-scrolling direction. Item size
        /// in the scrolling direction is not changed.
        /// </summary>
        Fill = 1,

        /// <summary>
        /// The item is sized to both fill the available space in the non-scrolling direction and
        /// maintain its aspect ratio.
        /// </summary>
        Uniform = 2,
    }

    /// <summary>
    /// 自适应虚拟化面板.
    /// </summary>
    public class AdaptiveLayout : VirtualizingLayout, IFlowLayoutAlgorithmDelegates
    {
        /// <summary>
        /// Defines the <see cref="MaximumRowsOrColumnsProperty"/> property.
        /// </summary>
        public static readonly DependencyProperty MaximumRowsOrColumnsProperty =
            DependencyProperty.Register(nameof(MaximumRowsOrColumns), typeof(int), typeof(AdaptiveLayout), new PropertyMetadata(int.MaxValue, new PropertyChangedCallback(OnPropertyChanged)));

        /// <summary>
        /// Defines the <see cref="ItemsJustification"/> property.
        /// </summary>
        public static readonly DependencyProperty ItemsJustificationProperty =
            DependencyProperty.Register(nameof(ItemsJustification), typeof(AdaptiveLayoutItemsJustification), typeof(AdaptiveLayout), new PropertyMetadata(AdaptiveLayoutItemsJustification.SpaceBetween, new PropertyChangedCallback(OnPropertyChanged)));

        /// <summary>
        /// Defines the <see cref="ItemsStretch"/> property.
        /// </summary>
        public static readonly DependencyProperty ItemsStretchProperty =
            DependencyProperty.Register(nameof(ItemsStretch), typeof(AdaptiveLayoutItemsStretch), typeof(AdaptiveLayout), new PropertyMetadata(AdaptiveLayoutItemsStretch.Fill, new PropertyChangedCallback(OnPropertyChanged)));

        /// <summary>
        /// Defines the <see cref="MinColumnSpacing"/> property.
        /// </summary>
        public static readonly DependencyProperty MinColumnSpacingProperty =
            DependencyProperty.Register(nameof(MinColumnSpacing), typeof(double), typeof(AdaptiveLayout), new PropertyMetadata(0d, new PropertyChangedCallback(OnPropertyChanged)));

        /// <summary>
        /// Defines the <see cref="MinItemHeight"/> property.
        /// </summary>
        public static readonly DependencyProperty MinItemHeightProperty =
            DependencyProperty.Register(nameof(MinItemHeight), typeof(double), typeof(AdaptiveLayout), new PropertyMetadata(0, new PropertyChangedCallback(OnPropertyChanged)));

        /// <summary>
        /// Defines the <see cref="MinItemWidth"/> property.
        /// </summary>
        public static readonly DependencyProperty MinItemWidthProperty =
            DependencyProperty.Register(nameof(MinItemWidth), typeof(double), typeof(AdaptiveLayout), new PropertyMetadata(0, new PropertyChangedCallback(OnPropertyChanged)));

        /// <summary>
        /// Defines the <see cref="MinRowSpacing"/> property.
        /// </summary>
        public static readonly DependencyProperty MinRowSpacingProperty =
            DependencyProperty.Register(nameof(MaximumRowsOrColumns), typeof(double), typeof(AdaptiveLayout), new PropertyMetadata(0, new PropertyChangedCallback(OnPropertyChanged)));

        /// <summary>
        /// Defines the <see cref="OrientationProperty"/> property.
        /// </summary>
        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register(nameof(Orientation), typeof(Orientation), typeof(AdaptiveLayout), new PropertyMetadata(0, new PropertyChangedCallback(OnPropertyChanged)));

        private readonly OrientationBasedMeasures _orientation = new OrientationBasedMeasures();
        private double _minItemWidth = double.NaN;
        private double _minItemHeight = double.NaN;
        private double _minRowSpacing;
        private double _minColumnSpacing;
        private int _maximumRowsOrColumns = int.MaxValue;
        private AdaptiveLayoutItemsJustification _itemsJustification;
        private AdaptiveLayoutItemsStretch _itemsStretch;

        /// <summary>
        /// Initializes a new instance of the <see cref="AdaptiveLayout"/> class.
        /// </summary>
        public AdaptiveLayout()
        {
        }

        /// <summary>
        /// Gets or sets a value that indicates how items are aligned on the non-scrolling or non-
        /// virtualizing axis.
        /// </summary>
        /// <value>
        /// An enumeration value that indicates how items are aligned. The default is Start.
        /// </value>
        public AdaptiveLayoutItemsJustification ItemsJustification
        {
            get => (AdaptiveLayoutItemsJustification)GetValue(ItemsJustificationProperty);
            set => SetValue(ItemsJustificationProperty, value);
        }

        /// <summary>
        /// Gets or sets a value that indicates how items are sized to fill the available space.
        /// </summary>
        /// <value>
        /// An enumeration value that indicates how items are sized to fill the available space.
        /// The default is None.
        /// </value>
        /// <remarks>
        /// This property enables adaptive layout behavior where the items are sized to fill the
        /// available space along the non-scrolling axis, and optionally maintain their aspect ratio.
        /// </remarks>
        public AdaptiveLayoutItemsStretch ItemsStretch
        {
            get => (AdaptiveLayoutItemsStretch)GetValue(ItemsStretchProperty);
            set => SetValue(ItemsStretchProperty, value);
        }

        /// <summary>
        /// Gets or sets the minimum space between items on the horizontal axis.
        /// </summary>
        /// <remarks>
        /// The spacing may exceed this minimum value when <see cref="ItemsJustification"/> is set
        /// to SpaceEvenly, SpaceAround, or SpaceBetween.
        /// </remarks>
        public double MinColumnSpacing
        {
            get => (double)GetValue(MinColumnSpacingProperty);
            set => SetValue(MinColumnSpacingProperty, value);
        }

        /// <summary>
        /// Gets or sets the minimum height of each item.
        /// </summary>
        /// <value>
        /// The minimum height (in pixels) of each item. The default is NaN, in which case the
        /// height of the first item is used as the minimum.
        /// </value>
        public double MinItemHeight
        {
            get => (double)GetValue(MinItemHeightProperty);
            set => SetValue(MinItemHeightProperty, value);
        }

        /// <summary>
        /// Gets or sets the minimum width of each item.
        /// </summary>
        /// <value>
        /// The minimum width (in pixels) of each item. The default is NaN, in which case the width
        /// of the first item is used as the minimum.
        /// </value>
        public double MinItemWidth
        {
            get => (double)GetValue(MinItemWidthProperty);
            set => SetValue(MinItemWidthProperty, value);
        }

        /// <summary>
        /// Gets or sets the minimum space between items on the vertical axis.
        /// </summary>
        /// <remarks>
        /// The spacing may exceed this minimum value when <see cref="ItemsJustification"/> is set
        /// to SpaceEvenly, SpaceAround, or SpaceBetween.
        /// </remarks>
        public double MinRowSpacing
        {
            get => (double)GetValue(MinRowSpacingProperty);
            set => SetValue(MinRowSpacingProperty, value);
        }

        /// <summary>
        /// Gets or sets the maximum row or column count.
        /// </summary>
        public int MaximumRowsOrColumns
        {
            get => (int)GetValue(MaximumRowsOrColumnsProperty);
            set => SetValue(MaximumRowsOrColumnsProperty, value);
        }

        /// <summary>
        /// Gets or sets the axis along which items are laid out.
        /// </summary>
        /// <value>
        /// One of the enumeration values that specifies the axis along which items are laid out.
        /// The default is Vertical.
        /// </value>
        public Orientation Orientation
        {
            get => (Orientation)GetValue(OrientationProperty);
            set => SetValue(OrientationProperty, value);
        }

        internal double LineSpacing => Orientation == Orientation.Horizontal ? _minRowSpacing : _minColumnSpacing;

        internal double MinItemSpacing => Orientation == Orientation.Horizontal ? _minColumnSpacing : _minRowSpacing;

        /// <inheritdoc/>
        Size IFlowLayoutAlgorithmDelegates.Algorithm_GetMeasureSize(
            int index,
            Size availableSize,
            VirtualizingLayoutContext context)
        {
            var gridState = (AdaptiveLayoutState)context.LayoutState;
            return new Size(gridState.EffectiveItemWidth, gridState.EffectiveItemHeight);
        }

        /// <inheritdoc/>
        Size IFlowLayoutAlgorithmDelegates.Algorithm_GetProvisionalArrangeSize(
            int index,
            Size measureSize,
            Size desiredSize,
            VirtualizingLayoutContext context)
        {
            var gridState = (AdaptiveLayoutState)context.LayoutState;
            return new Size(gridState.EffectiveItemWidth, gridState.EffectiveItemHeight);
        }

        /// <inheritdoc/>
        bool IFlowLayoutAlgorithmDelegates.Algorithm_ShouldBreakLine(int index, double remainingSpace) => remainingSpace < 0;

        /// <inheritdoc/>
        FlowLayoutAnchorInfo IFlowLayoutAlgorithmDelegates.Algorithm_GetAnchorForRealizationRect(
            Size availableSize,
            VirtualizingLayoutContext context)
        {
            var bounds = new Rect(double.NaN, double.NaN, double.NaN, double.NaN);
            var anchorIndex = -1;

            var itemsCount = context.ItemCount;
            var realizationRect = context.RealizationRect;
            if (itemsCount > 0 && _orientation.MajorSize(realizationRect) > 0)
            {
                var gridState = (AdaptiveLayoutState)context.LayoutState;
                var lastExtent = gridState.FlowAlgorithm.LastExtent;
                var itemsPerLine = Math.Min( // note use of unsigned ints
                    Math.Max(1u, (uint)((_orientation.Minor(availableSize) + MinItemSpacing) / GetMinorSizeWithSpacing(context))),
                    Math.Max(1u, (uint)_maximumRowsOrColumns));
                var majorSize = itemsCount / itemsPerLine * GetMajorSizeWithSpacing(context);
                var realizationWindowStartWithinExtent = _orientation.MajorStart(realizationRect) - _orientation.MajorStart(lastExtent);
                if ((realizationWindowStartWithinExtent + _orientation.MajorSize(realizationRect)) >= 0 && realizationWindowStartWithinExtent <= majorSize)
                {
                    var offset = Math.Max(0.0, _orientation.MajorStart(realizationRect) - _orientation.MajorStart(lastExtent));
                    var anchorRowIndex = (int)(offset / GetMajorSizeWithSpacing(context));

                    anchorIndex = (int)Math.Max(0, Math.Min(itemsCount - 1, anchorRowIndex * itemsPerLine));
                    bounds = GetLayoutRectForDataIndex(availableSize, anchorIndex, lastExtent, context);
                }
            }

            return new FlowLayoutAnchorInfo
            {
                Index = anchorIndex,
                Offset = _orientation.MajorStart(bounds),
            };
        }

        /// <inheritdoc/>
        FlowLayoutAnchorInfo IFlowLayoutAlgorithmDelegates.Algorithm_GetAnchorForTargetElement(
            int targetIndex,
            Size availableSize,
            VirtualizingLayoutContext context)
        {
            var index = -1;
            var offset = double.NaN;
            var count = context.ItemCount;
            if (targetIndex >= 0 && targetIndex < count)
            {
                var itemsPerLine = (int)Math.Min( // note use of unsigned ints
                    Math.Max(1u, (uint)((_orientation.Minor(availableSize) + MinItemSpacing) / GetMinorSizeWithSpacing(context))),
                    Math.Max(1u, _maximumRowsOrColumns));
                var indexOfFirstInLine = targetIndex / itemsPerLine * itemsPerLine;
                index = indexOfFirstInLine;
                var state = context.LayoutState as AdaptiveLayoutState;
                offset = _orientation.MajorStart(GetLayoutRectForDataIndex(availableSize, indexOfFirstInLine, state.FlowAlgorithm.LastExtent, context));
            }

            return new FlowLayoutAnchorInfo
            {
                Index = index,
                Offset = offset,
            };
        }

        /// <inheritdoc/>
        Rect IFlowLayoutAlgorithmDelegates.Algorithm_GetExtent(
            Size availableSize,
            VirtualizingLayoutContext context,
            UIElement firstRealized,
            int firstRealizedItemIndex,
            Rect firstRealizedLayoutBounds,
            UIElement lastRealized,
            int lastRealizedItemIndex,
            Rect lastRealizedLayoutBounds)
        {
            var extent = default(Rect);

            // Constants
            var itemsCount = context.ItemCount;
            var availableSizeMinor = _orientation.Minor(availableSize);
            var calcCount = !double.IsInfinity(availableSizeMinor)
                                    ? (uint)((availableSizeMinor + MinItemSpacing) / GetMinorSizeWithSpacing(context))
                                    : (uint)itemsCount;
            var itemsPerLine =
                (int)Math.Min( // note use of unsigned ints
                    Math.Max(1u, calcCount),
                    Math.Max(1u, _maximumRowsOrColumns));
            var lineSize = GetMajorSizeWithSpacing(context);

            if (itemsCount > 0)
            {
                _orientation.SetMinorSize(
                    ref extent,
                    !double.IsInfinity(availableSizeMinor) && _itemsStretch == AdaptiveLayoutItemsStretch.Fill ? availableSizeMinor : Math.Max(0.0, (itemsPerLine * GetMinorSizeWithSpacing(context)) - (double)MinItemSpacing));
                _orientation.SetMajorSize(
                    ref extent,
                    Math.Max(0.0, (itemsCount / itemsPerLine * lineSize) - (double)LineSpacing));

                if (firstRealized != null)
                {
                    _orientation.SetMajorStart(
                        ref extent,
                        _orientation.MajorStart(firstRealizedLayoutBounds) - (firstRealizedItemIndex / itemsPerLine * lineSize));
                    var remainingItems = itemsCount - lastRealizedItemIndex - 1;
                    _orientation.SetMajorSize(
                        ref extent,
                        _orientation.MajorEnd(lastRealizedLayoutBounds) - _orientation.MajorStart(extent) + (remainingItems / itemsPerLine * lineSize));
                }
            }

            return extent;
        }

        /// <inheritdoc/>
        void IFlowLayoutAlgorithmDelegates.Algorithm_OnElementMeasured(UIElement element, int index, Size availableSize, Size measureSize, Size desiredSize, Size provisionalArrangeSize, VirtualizingLayoutContext context)
        {
        }

        /// <inheritdoc/>
        void IFlowLayoutAlgorithmDelegates.Algorithm_OnLineArranged(int startIndex, int countInLine, double lineSize, VirtualizingLayoutContext context)
        {
        }

        /// <inheritdoc/>
        protected override void InitializeForContextCore(VirtualizingLayoutContext context)
        {
            var state = context.LayoutState;

            if (!(state is AdaptiveLayoutState gridState))
            {
                if (state != null)
                {
                    throw new InvalidOperationException("LayoutState must derive from AdaptiveLayoutState.");
                }

                // Custom deriving layouts could potentially be stateful.
                // If that is the case, we will just create the base state required by UniformGridLayout ourselves.
                gridState = new AdaptiveLayoutState();
            }

            gridState.InitializeForContext(context, this);
        }

        /// <inheritdoc/>
        protected override void UninitializeForContextCore(VirtualizingLayoutContext context)
        {
            var gridState = (AdaptiveLayoutState)context.LayoutState;
            gridState.UninitializeForContext(context);
        }

        /// <inheritdoc/>
        protected override Size MeasureOverride(VirtualizingLayoutContext context, Size availableSize)
        {
            // Set the width and height on the grid state. If the user already set them then use the preset.
            // If not, we have to measure the first element and get back a size which we're going to be using for the rest of the items.
            var gridState = (AdaptiveLayoutState)context.LayoutState;
            gridState.EnsureElementSize(availableSize, context, _minItemWidth, _minItemHeight, _itemsStretch, Orientation, MinRowSpacing, MinColumnSpacing, _maximumRowsOrColumns);

            var desiredSize = GetFlowAlgorithm(context).Measure(
                availableSize,
                context,
                true,
                MinItemSpacing,
                LineSpacing,
                _maximumRowsOrColumns,
                _orientation.ScrollOrientation,
                false,
                nameof(AdaptiveLayout));

            // If after Measure the first item is in the realization rect, then we revoke grid state's ownership,
            // and only use the layout when to clear it when it's done.
            gridState.EnsureFirstElementOwnership(context);

            return desiredSize;
        }

        /// <inheritdoc/>
        protected override Size ArrangeOverride(VirtualizingLayoutContext context, Size finalSize)
        {
            var value = GetFlowAlgorithm(context).Arrange(
               finalSize,
               context,
               true,
               (FlowLayoutAlgorithm.LineAlignment)_itemsJustification,
               nameof(AdaptiveLayout));
            return new Size(value.Width, value.Height);
        }

        /// <inheritdoc/>
        protected override void OnItemsChangedCore(VirtualizingLayoutContext context, object source, NotifyCollectionChangedEventArgs args)
        {
            GetFlowAlgorithm(context).OnItemsSourceChanged(source, args, context);

            // Always invalidate layout to keep the view accurate.
            InvalidateLayout();

            var gridState = (AdaptiveLayoutState)context.LayoutState;
            gridState.ClearElementOnDataSourceChange(context, args);
        }

        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs change)
        {
            var instance = d as AdaptiveLayout;
            if (change.Property == OrientationProperty)
            {
                var orientation = (Orientation)change.NewValue;

                // Note: For UniformGridLayout Vertical Orientation means we have a Horizontal ScrollOrientation. Horizontal Orientation means we have a Vertical ScrollOrientation.
                // i.e. the properties are the inverse of each other.
                var scrollOrientation = (orientation == Orientation.Horizontal) ? ScrollOrientation.Vertical : ScrollOrientation.Horizontal;
                instance._orientation.ScrollOrientation = scrollOrientation;
            }
            else if (change.Property == MinColumnSpacingProperty)
            {
                instance._minColumnSpacing = (double)change.NewValue;
            }
            else if (change.Property == MinRowSpacingProperty)
            {
                instance._minRowSpacing = (double)change.NewValue;
            }
            else if (change.Property == ItemsJustificationProperty)
            {
                instance._itemsJustification = (AdaptiveLayoutItemsJustification)change.NewValue;
            }
            else if (change.Property == ItemsStretchProperty)
            {
                instance._itemsStretch = (AdaptiveLayoutItemsStretch)change.NewValue;
            }
            else if (change.Property == MinItemWidthProperty)
            {
                instance._minItemWidth = (double)change.NewValue;
            }
            else if (change.Property == MinItemHeightProperty)
            {
                instance._minItemHeight = (double)change.NewValue;
            }
            else if (change.Property == MaximumRowsOrColumnsProperty)
            {
                instance._maximumRowsOrColumns = (int)change.NewValue;
            }

            instance.InvalidateLayout();
        }

        private double GetMinorSizeWithSpacing(VirtualizingLayoutContext context)
        {
            var minItemSpacing = MinItemSpacing;
            var gridState = (AdaptiveLayoutState)context.LayoutState;
            return _orientation.ScrollOrientation == ScrollOrientation.Vertical ?
                gridState.EffectiveItemWidth + minItemSpacing :
                gridState.EffectiveItemHeight + minItemSpacing;
        }

        private double GetMajorSizeWithSpacing(VirtualizingLayoutContext context)
        {
            var lineSpacing = LineSpacing;
            var gridState = (AdaptiveLayoutState)context.LayoutState;
            return _orientation.ScrollOrientation == ScrollOrientation.Vertical ?
                gridState.EffectiveItemHeight + lineSpacing :
                gridState.EffectiveItemWidth + lineSpacing;
        }

        private Rect GetLayoutRectForDataIndex(
            Size availableSize,
            int index,
            Rect lastExtent,
            VirtualizingLayoutContext context)
        {
            var itemsPerLine = (int)Math.Min(
                Math.Max(1u, (uint)((_orientation.Minor(availableSize) + MinItemSpacing) / GetMinorSizeWithSpacing(context))),
                Math.Max(1u, _maximumRowsOrColumns));
            var rowIndex = index / itemsPerLine;
            var indexInRow = index - (rowIndex * itemsPerLine);

            var gridState = (AdaptiveLayoutState)context.LayoutState;
            var bounds = _orientation.MinorMajorRect(
                (indexInRow * GetMinorSizeWithSpacing(context)) + _orientation.MinorStart(lastExtent),
                (rowIndex * GetMajorSizeWithSpacing(context)) + _orientation.MajorStart(lastExtent),
                _orientation.ScrollOrientation == ScrollOrientation.Vertical ? gridState.EffectiveItemWidth : gridState.EffectiveItemHeight,
                _orientation.ScrollOrientation == ScrollOrientation.Vertical ? gridState.EffectiveItemHeight : gridState.EffectiveItemWidth);

            return bounds;
        }

        private void InvalidateLayout() => InvalidateMeasure();

        private FlowLayoutAlgorithm GetFlowAlgorithm(VirtualizingLayoutContext context) => ((AdaptiveLayoutState)context.LayoutState).FlowAlgorithm;
    }
}
