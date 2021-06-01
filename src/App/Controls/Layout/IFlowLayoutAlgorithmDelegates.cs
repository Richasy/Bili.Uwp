// Copyright (c) Richasy. All rights reserved.

using Microsoft.UI.Xaml.Controls;
using Windows.Foundation;
using Windows.UI.Xaml;

namespace Richasy.Bili.App.Controls
{
    /// <summary>
    /// 流式布局算法代理接口.
    /// </summary>
    internal interface IFlowLayoutAlgorithmDelegates
    {
#pragma warning disable SA1600 // Elements should be documented
        Size Algorithm_GetMeasureSize(int index, Size availableSize, VirtualizingLayoutContext context);

        Size Algorithm_GetProvisionalArrangeSize(int index, Size measureSize, Size desiredSize, VirtualizingLayoutContext context);

        bool Algorithm_ShouldBreakLine(int index, double remainingSpace);

        FlowLayoutAnchorInfo Algorithm_GetAnchorForRealizationRect(Size availableSize, VirtualizingLayoutContext context);

        FlowLayoutAnchorInfo Algorithm_GetAnchorForTargetElement(int targetIndex, Size availableSize, VirtualizingLayoutContext context);

        Rect Algorithm_GetExtent(
            Size availableSize,
            VirtualizingLayoutContext context,
            UIElement firstRealized,
            int firstRealizedItemIndex,
            Rect firstRealizedLayoutBounds,
            UIElement lastRealized,
            int lastRealizedItemIndex,
            Rect lastRealizedLayoutBounds);

        void Algorithm_OnElementMeasured(
            UIElement element,
            int index,
            Size availableSize,
            Size measureSize,
            Size desiredSize,
            Size provisionalArrangeSize,
            VirtualizingLayoutContext context);

        void Algorithm_OnLineArranged(
            int startIndex,
            int countInLine,
            double lineSize,
            VirtualizingLayoutContext context);
#pragma warning restore SA1600 // Elements should be documented
    }
}
