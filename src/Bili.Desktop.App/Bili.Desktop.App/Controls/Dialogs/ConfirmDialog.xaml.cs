// Copyright (c) Richasy. All rights reserved.

using Microsoft.UI.Xaml.Controls;

namespace Bili.Desktop.App.Controls.Dialogs
{
    /// <summary>
    /// 表示确认的对话框.
    /// </summary>
    public sealed partial class ConfirmDialog : ContentDialog
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConfirmDialog"/> class.
        /// </summary>
        public ConfirmDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfirmDialog"/> class.
        /// </summary>
        /// <param name="message">显示信息.</param>
        public ConfirmDialog(string message)
            : this()
        {
            DisplayBlock.Text = message;
        }
    }
}
