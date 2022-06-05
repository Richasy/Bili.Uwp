// Copyright (c) Richasy. All rights reserved.

using System.Collections.ObjectModel;
using System.Reactive;
using Bili.Models.App.Other;
using Bili.Models.Enums.App;
using Bili.Toolkit.Interfaces;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Bili.ViewModels.Uwp.Account
{
    /// <summary>
    /// 收藏夹页面视图模型.
    /// </summary>
    public sealed partial class FavoritePageViewModel
    {
        private readonly IResourceToolkit _resourceToolkit;

        /// <summary>
        /// 类型集合.
        /// </summary>
        public ObservableCollection<FavoriteHeader> TypeCollection { get; }

        /// <summary>
        /// 选择收藏类型的命令.
        /// </summary>
        public ReactiveCommand<FavoriteHeader, Unit> SelectTypeCommand { get; }

        /// <summary>
        /// 当前选中项.
        /// </summary>
        [Reactive]
        public FavoriteHeader CurrentType { get; set; }

        /// <summary>
        /// 是否显示视频收藏.
        /// </summary>
        [Reactive]
        public bool IsVideoShown { get; set; }

        /// <summary>
        /// 是否显示动漫收藏.
        /// </summary>
        [Reactive]
        public bool IsAnimeShown { get; set; }

        /// <summary>
        /// 是否显示影视收藏.
        /// </summary>
        [Reactive]
        public bool IsCinemaShown { get; set; }

        /// <summary>
        /// 是否显示文章收藏.
        /// </summary>
        [Reactive]
        public bool IsArticleShown { get; set; }
    }
}
