// Copyright (c) Richasy. All rights reserved.

using System;
using Bili.Desktop.App.Controls;
using Bili.ViewModels.Interfaces;
using CommunityToolkit.WinUI.UI.Behaviors;
using Microsoft.UI.Xaml.Controls;

namespace Bili.Desktop.App.Behaviors
{
    /// <summary>
    /// 增量请求行为.
    /// </summary>
    internal sealed class IncrementalBehavior : BehaviorBase<Control>
    {
        protected override void OnAssociatedObjectLoaded()
        {
            if (AssociatedObject is IIncrementalControl control)
            {
                control.IncrementalTriggered += OnIncrementalTriggered;
            }
        }

        protected override void OnAssociatedObjectUnloaded()
        {
            if (AssociatedObject is IIncrementalControl control)
            {
                control.IncrementalTriggered -= OnIncrementalTriggered;
            }
        }

        private void OnIncrementalTriggered(object sender, EventArgs e)
        {
            if (AssociatedObject.DataContext is IIncrementalViewModel vm)
            {
                vm.IncrementalCommand.ExecuteAsync(null);
            }
        }
    }
}
