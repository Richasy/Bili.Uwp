// Copyright (c) Richasy. All rights reserved.

using Microsoft.UI.Xaml.Controls;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation.Peers;

namespace Richasy.Bili.App.Controls
{
    /// <summary>
    /// <see cref="CardPanel"/>的自动化公开属性.
    /// </summary>
    public class CardPanelAutomationPeer : FrameworkElementAutomationPeer
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
            var parent = this.owner.Parent;
            if (!(parent is ItemsRepeater))
            {
                parent = (parent as FrameworkElement).Parent;
            }

            return (this.owner.Parent as ItemsRepeater)?.GetElementIndex(this.owner) + 1
                ?? base.GetPositionInSetCore();
        }

        /// <inheritdoc/>
        protected override int GetSizeOfSetCore()
        {
            var parent = this.owner.Parent;
            if (!(parent is ItemsRepeater))
            {
                parent = (parent as FrameworkElement).Parent;
            }

            var count = (this.owner.Parent as ItemsRepeater)?.ItemsSourceView?.Count;
            return count ?? base.GetSizeOfSetCore();
        }
    }
}
