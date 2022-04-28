// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using Bili.Controller.Uwp;
using Bili.Toolkit.Interfaces;
using ReactiveUI.Fody.Helpers;

namespace Bili.ViewModels.Uwp
{
    /// <summary>
    /// 专栏模块视图模型.
    /// </summary>
    public partial class SpecialColumnModuleViewModel
    {
        private readonly BiliController _controller;

        private readonly IResourceToolkit _resourceToolkit;

        /// <summary>
        /// 实例.
        /// </summary>
        public static SpecialColumnModuleViewModel Instance { get; } = new Lazy<SpecialColumnModuleViewModel>(() => new SpecialColumnModuleViewModel()).Value;

        /// <summary>
        /// 分类集合.
        /// </summary>
        [Reactive]
        public ObservableCollection<SpecialColumnCategoryViewModel> CategoryCollection { get; set; }

        /// <summary>
        /// 当前正在展示的分类.
        /// </summary>
        [Reactive]
        public SpecialColumnCategoryViewModel CurrentCategory { get; set; }

        /// <summary>
        /// 是否正在请求数据.
        /// </summary>
        [Reactive]
        public bool IsLoading { get; set; }

        /// <summary>
        /// 是否发生了错误.
        /// </summary>
        [Reactive]
        public bool IsError { get; set; }

        /// <summary>
        /// 错误文本.
        /// </summary>
        [Reactive]
        public string ErrorText { get; set; }
    }
}
