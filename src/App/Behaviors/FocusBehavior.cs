// Copyright (c) Richasy. All rights reserved.

using System;
using Bili.ViewModels.Interfaces;
using Microsoft.Toolkit.Uwp.UI.Behaviors;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Bili.App.Behaviors
{
    /// <summary>
    /// 初始化行为.
    /// </summary>
    internal sealed class FocusBehavior : BehaviorBase<Control>
    {
        /// <inheritdoc/>
        protected override void OnAssociatedObjectLoaded()
        {
            if (AssociatedObject.DataContext is ICollectionViewModel collectionVM)
            {
                collectionVM.CollectionInitialized += OnCollectionInitializedAsync;
            }
        }

        protected override void OnAssociatedObjectUnloaded()
        {
            if (AssociatedObject == null)
            {
                return;
            }

            if (AssociatedObject.DataContext is ICollectionViewModel collectionVM)
            {
                collectionVM.CollectionInitialized -= OnCollectionInitializedAsync;
            }
        }

        private async void OnCollectionInitializedAsync(object sender, EventArgs e)
            => await FocusManager.TryFocusAsync(AssociatedObject, Windows.UI.Xaml.FocusState.Programmatic);
    }
}
