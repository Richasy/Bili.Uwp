// Copyright (c) Richasy. All rights reserved.

using System.Collections.ObjectModel;
using Bili.ViewModels.Interfaces.Common;
using CommunityToolkit.Mvvm.Input;

namespace Bili.ViewModels.Interfaces.Pgc
{
    /// <summary>
    /// PGC 信息流页面（不包括动漫）的通用视图模型的接口定义.
    /// </summary>
    public interface IPgcPageViewModel : IInformationFlowViewModel<ISeasonItemViewModel>
    {
        /// <summary>
        /// 横幅列表.
        /// </summary>
        ObservableCollection<IBannerViewModel> Banners { get; }

        /// <summary>
        /// 导航至索引页面的命令.
        /// </summary>
        IRelayCommand GotoIndexPageCommand { get; }

        /// <summary>
        /// 是否显示横幅.
        /// </summary>
        bool IsShowBanner { get; set; }

        /// <summary>
        /// 页面标题.
        /// </summary>
        string Title { get; set; }
    }
}
