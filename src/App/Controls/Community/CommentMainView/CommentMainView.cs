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
            var replyBox = GetTemplateChild("ReplyBox") as TextBox;
            var orderComboBox = GetTemplateChild("OrderTypeComboBox") as ComboBox;

            replyBox.LosingFocus += OnReplyBoxLostFocus;
            orderComboBox.SelectionChanged += OnOrderComboBoxSelectionChanged;
        }

        private void OnOrderComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = (sender as ComboBox).SelectedItem as CommentSortHeader;
            if (item != ViewModel.CurrentSort)
            {
                ViewModel.ChangeSortCommand.Execute(item).Subscribe();
            }
        }

        private void OnReplyBoxLostFocus(UIElement sender, LosingFocusEventArgs args)
        {
            if (string.IsNullOrEmpty(ViewModel.ReplyText))
            {
                ViewModel.ResetSelectedCommentCommand.Execute().Subscribe();
            }
        }
    }
}
