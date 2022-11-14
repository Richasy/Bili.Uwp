// Copyright (c) Richasy. All rights reserved.

using Bili.Models.App.Other;
using Bili.ViewModels.Interfaces.Community;
using Microsoft.UI.Xaml.Controls;

namespace Bili.Desktop.App.Controls.Community
{
    /// <summary>
    /// 评论区主视图.
    /// </summary>
    public sealed class CommentMainView : ReactiveControl<ICommentMainModuleViewModel>
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
                ViewModel.ChangeSortCommand.Execute(item);
            }
        }
    }
}
