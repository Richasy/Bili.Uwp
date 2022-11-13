// Copyright (c) Richasy. All rights reserved.

using System.Windows.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Bili.Desktop.App.Controls.Community
{
    /// <summary>
    /// 评论文本.
    /// </summary>
    public sealed partial class CommentBox : UserControl
    {
        /// <summary>
        /// <see cref="Text"/> 的依赖属性.
        /// </summary>
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(CommentBox), new PropertyMetadata(default));

        /// <summary>
        /// <see cref="SendCommand"/> 的依赖属性.
        /// </summary>
        public static readonly DependencyProperty SendCommandProperty =
            DependencyProperty.Register(nameof(SendCommand), typeof(ICommand), typeof(CommentBox), new PropertyMetadata(default));

        /// <summary>
        /// <see cref="ReplyTip"/> 的依赖属性.
        /// </summary>
        public static readonly DependencyProperty ReplyTipProperty =
            DependencyProperty.Register(nameof(ReplyTip), typeof(string), typeof(CommentBox), new PropertyMetadata(default));

        /// <summary>
        /// <see cref="ResetSelectedCommand"/> 的依赖属性.
        /// </summary>
        public static readonly DependencyProperty ResetSelectedCommandProperty =
            DependencyProperty.Register(nameof(ResetSelectedCommand), typeof(ICommand), typeof(CommentBox), new PropertyMetadata(default));

        /// <summary>
        /// Initializes a new instance of the <see cref="CommentBox"/> class.
        /// </summary>
        public CommentBox() => InitializeComponent();

        /// <summary>
        /// 回复文本.
        /// </summary>
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        /// <summary>
        /// 发送评论命令.
        /// </summary>
        public ICommand SendCommand
        {
            get { return (ICommand)GetValue(SendCommandProperty); }
            set { SetValue(SendCommandProperty, value); }
        }

        /// <summary>
        /// 评论提示.
        /// </summary>
        public string ReplyTip
        {
            get { return (string)GetValue(ReplyTipProperty); }
            set { SetValue(ReplyTipProperty, value); }
        }

        /// <summary>
        /// 重置已选择评论的命令.
        /// </summary>
        public ICommand ResetSelectedCommand
        {
            get { return (ICommand)GetValue(ResetSelectedCommandProperty); }
            set { SetValue(ResetSelectedCommandProperty, value); }
        }
    }
}
