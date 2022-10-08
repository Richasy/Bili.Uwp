// Copyright (c) Richasy. All rights reserved.

using System.Collections.ObjectModel;
using System.Reactive;
using Bili.Lib.Interfaces;
using Bili.Models.Enums;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Common;
using Bili.ViewModels.Interfaces.Core;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Bili.ViewModels.Uwp.Base
{
    /// <summary>
    /// PGC 信息流页面（不包括动漫）的通用视图模型.
    /// </summary>
    public partial class PgcPageViewModelBase
    {
        private readonly IPgcProvider _pgcProvider;
        private readonly IResourceToolkit _resourceToolkit;
        private readonly INavigationViewModel _navigationViewModel;
        private readonly PgcType _type;

        /// <summary>
        /// 横幅列表.
        /// </summary>
        public ObservableCollection<IBannerViewModel> Banners { get; }

        /// <summary>
        /// 导航至索引页面的命令.
        /// </summary>
        public IRelayCommand GotoIndexPageCommand { get; }

        /// <summary>
        /// 是否显示横幅.
        /// </summary>
        [ObservableProperty]
        public bool IsShowBanner { get; set; }

        /// <summary>
        /// 页面标题.
        /// </summary>
        [ObservableProperty]
        public string Title { get; set; }
    }
}
