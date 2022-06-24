// Copyright (c) Richasy. All rights reserved.

using System.Reactive;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Bili.ViewModels.Uwp.Community
{
    /// <summary>
    /// 评论页面/模块视图模型.
    /// </summary>
    public sealed partial class CommentPageViewModel
    {
        /// <summary>
        /// 主视图模型.
        /// </summary>
        public CommentMainModuleViewModel MainViewModel { get; }

        /// <summary>
        /// 二级视图模型.
        /// </summary>
        public CommentDetailModuleViewModel DetailViewModel { get; }

        /// <summary>
        /// 是否显示主视图.
        /// </summary>
        [Reactive]
        public bool IsMainShown { get; set; }

        /// <summary>
        /// 是否显示二级视图.
        /// </summary>
        [Reactive]
        public bool IsDetailShown { get; set; }
    }
}
