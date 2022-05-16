// Copyright (c) Richasy. All rights reserved.

using System;
using Bili.App.Controls;
using Bili.App.Pages;
using Bili.ViewModels.Uwp;
using Microsoft.Toolkit.Uwp.UI.Behaviors;
using Windows.UI.Xaml.Controls;

namespace Bili.App.Behaviors
{
    /// <summary>
    /// 初始化行为.
    /// </summary>
    internal sealed class InitializeOrReloadBehavior : BehaviorBase<Control>
    {
        /// <inheritdoc/>
        protected override void OnAssociatedObjectLoaded()
        {
            if (AssociatedObject is AppPage page
                && page.GetViewModel() is IInitializeViewModel initVM)
            {
                initVM.InitializeCommand.Execute().Subscribe();
            }
            else if (AssociatedObject is IActivatableControl activatableControl)
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
                reloadVM.ReloadCommand.Execute().Subscribe();
            }
        }
    }
}
