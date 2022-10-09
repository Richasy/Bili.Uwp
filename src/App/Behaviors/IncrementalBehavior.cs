// Copyright (c) Richasy. All rights reserved.

using System;
using Bili.App.Controls;
using Bili.ViewModels.Interfaces;
using Microsoft.Toolkit.Uwp.UI.Behaviors;
using Windows.UI.Xaml.Controls;

namespace Bili.App.Behaviors
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
