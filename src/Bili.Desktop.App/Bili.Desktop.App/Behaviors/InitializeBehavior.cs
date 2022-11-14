// Copyright (c) Richasy. All rights reserved.

using Bili.ViewModels.Interfaces;
using CommunityToolkit.WinUI.UI.Behaviors;
using Microsoft.UI.Xaml.Controls;

namespace Bili.Desktop.App.Behaviors
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
                initVM.InitializeCommand.ExecuteAsync(null);
            }
        }
    }
}
