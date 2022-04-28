// Copyright (c) Richasy. All rights reserved.

using System;
using System.Threading.Tasks;

namespace Bili.App.Controls
{
    /// <summary>
    /// 评论回复详情.
    /// </summary>
    public partial class ReplyDetailView : CenterPopup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReplyDetailView"/> class.
        /// </summary>
        protected ReplyDetailView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 单例.
        /// </summary>
        public static ReplyDetailView Instance { get; } = new Lazy<ReplyDetailView>(() => new ReplyDetailView()).Value;

        /// <summary>
        /// 显示.
        /// </summary>
        /// <param name="data">数据.</param>
        /// <returns><see cref="Task"/>.</returns>
        public async Task ShowAsync(object data)
        {
            Container.Show();
            await ModuleView.InitializeAsync(data);
        }
    }
}
