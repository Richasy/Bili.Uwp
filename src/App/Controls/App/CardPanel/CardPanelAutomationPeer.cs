﻿// Copyright (c) Richasy. All rights reserved.

using Microsoft.UI.Xaml.Controls;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation.Peers;

namespace Richasy.Bili.App.Controls
{
    /// <summary>
    /// <see cref="CardPanel"/>的自动化公开属性.
    /// </summary>
    public class CardPanelAutomationPeer : ToggleButtonAutomationPeer
    {
        private readonly CardPanel owner;

        /// <summary>
        /// Initializes a new instance of the <see cref="CardPanelAutomationPeer"/> class.
        /// </summary>
        /// <param name="owner">对象.</param>
        public CardPanelAutomationPeer(CardPanel owner)
            : base(owner) => this.owner = owner;

        /// <inheritdoc/>
        protected override AutomationControlType GetAutomationControlTypeCore()
        {
            return AutomationControlType.ListItem;
        }

        /// <inheritdoc/>
        protected override int GetPositionInSetCore()
        {
            var element = this.owner as FrameworkElement;
            var parent = this.owner.Parent;
            if (!(parent is ItemsRepeater) && parent != null)
            {
                parent = (parent as FrameworkElement).Parent;
                element = this.owner.Parent as FrameworkElement;
            }

            return (parent as ItemsRepeater)?.GetElementIndex(element) + 1
                ?? base.GetPositionInSetCore();
        }

        /// <inheritdoc/>
        protected override int GetSizeOfSetCore()
        {
            var parent = this.owner.Parent;
            if (!(parent is ItemsRepeater) && parent != null)
            {
                parent = (parent as FrameworkElement).Parent;
            }

            var count = (parent as ItemsRepeater)?.ItemsSourceView?.Count;
            return count ?? base.GetSizeOfSetCore();
        }
    }
}
