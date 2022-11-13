// Copyright (c) Richasy. All rights reserved.

using System;
using Bili.Uwp.App.Controls;
using Bili.ViewModels.Interfaces;
using Microsoft.Toolkit.Uwp.UI.Behaviors;
using Windows.UI.Xaml.Controls;

namespace Bili.Uwp.App.Behaviors
{
    /// <summary>
    /// 重载行为.
    /// </summary>
    internal sealed class ReloadBehavior : BehaviorBase<Control>
    {
        /// <inheritdoc/>
        protected override void OnAssociatedObjectLoaded()
        {
            if (AssociatedObject is IActivatableControl activatableControl)
            {
                activatableControl.Activated += OnActivatableControlActivated;
            }
        }

        /// <inheritdoc/>
        protected override void OnAssociatedObjectUnloaded()
        {
            if (AssociatedObject is IActivatableControl activatableControl)
            {
                activatableControl.Activated -= OnActivatableControlActivated;
            }
        }

        private void OnActivatableControlActivated(object sender, EventArgs e)
        {
            var dataContext = AssociatedObject.DataContext;
            if (dataContext is IReloadViewModel reloadVM)
            {
                reloadVM.ReloadCommand.ExecuteAsync(null);
            }
        }
    }
}
