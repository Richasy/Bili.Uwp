// Copyright (c) Richasy. All rights reserved.

using System.Collections.ObjectModel;
using ReactiveUI.Fody.Helpers;
using Richasy.Bili.Models.Enums;

namespace Richasy.Bili.ViewModels.Uwp
{
    /// <summary>
    /// PGC模块视图模型.
    /// </summary>
    public partial class PgcModuleViewModel
    {
        /// <summary>
        /// 模块Id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 模块数据类型.
        /// </summary>
        public PgcModuleType Type { get; set; }

        /// <summary>
        /// 剧集集合.
        /// </summary>
        [Reactive]
        public ObservableCollection<SeasonViewModel> SeasonCollection { get; set; }

        /// <summary>
        /// 显示标题.
        /// </summary>
        [Reactive]
        public string Title { get; set; }

        /// <summary>
        /// 是否显示“显示更多”按钮.
        /// </summary>
        [Reactive]
        public bool IsDisplayMoreButton { get; set; }
    }
}
