// Copyright (c) Richasy. All rights reserved.

using System;
using Bili.Models.App.Other;
using Bili.ViewModels.Uwp.Community;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Bili.App.Controls.Community
{
    /// <summary>
    /// 评论区主视图.
    /// </summary>
    public sealed class CommentMainView : ReactiveControl<CommentMainModuleViewModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommentMainView"/> class.
        /// </summary>
        public CommentMainView() => DefaultStyleKey = typeof(CommentMainView);

        /// <inheritdoc/>
        protected override void OnApplyTemplate()
        {
            var orderComboBox = GetTemplateChild("OrderTypeComboBox") as ComboBox;
            orderComboBox.SelectionChanged += OnOrderComboBoxSelectionChanged;
        }

        private void OnOrderComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((sender as ComboBox).SelectedItem is CommentSortHeader item && item != ViewModel.CurrentSort)
            {
                ViewModel.ChangeSortCommand.Execute(item).Subscribe();
            }
        }
    }
}
