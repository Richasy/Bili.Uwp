// Copyright (c) Richasy. All rights reserved.

using Bili.DI.Container;
using Bili.Models.App.Args;
using Bili.ViewModels.Interfaces.Community;

namespace Bili.Desktop.App.Controls.Community
{
    /// <summary>
    /// 评论区弹出层.
    /// </summary>
    public sealed partial class CommentPopup : CenterPopup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommentPopup"/> class.
        /// </summary>
        public CommentPopup() => InitializeComponent();

        /// <summary>
        /// 显示评论弹出层.
        /// </summary>
        /// <param name="args">显示参数.</param>
        public void Show(ShowCommentEventArgs args)
        {
            var pageVM = Locator.Instance.GetService<ICommentPageViewModel>();
            pageVM.SetData(args.SourceId, args.Type, args.SortType);
            Show();
        }
    }
}
