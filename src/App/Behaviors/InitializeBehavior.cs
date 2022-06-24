// Copyright (c) Richasy. All rights reserved.

using System;
using Bili.ViewModels.Interfaces;
using Microsoft.Toolkit.Uwp.UI.Behaviors;
using Windows.UI.Xaml.Controls;

namespace Bili.App.Behaviors
{
    /// <summary>
    /// 初始化行为.
    /// </summary>
    internal sealed class InitializeBehavior : BehaviorBase<Control>
    {
        /// <inheritdoc/>
        protected override void OnAssociatedObjectLoaded()
        {
            if (AssociatedObject.DataContext is IInitializeViewModel initVM)
            {
                initVM.InitializeCommand.Execute().Subscribe();
            }
        }
    }
}
