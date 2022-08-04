// Copyright (c) Richasy. All rights reserved.

using System.Collections.ObjectModel;
using System.Reactive;
using Bili.ViewModels.Interfaces.Account;
using Bili.ViewModels.Interfaces.Article;
using Bili.ViewModels.Interfaces.Live;
using Bili.ViewModels.Interfaces.Pgc;
using Bili.ViewModels.Interfaces.Video;
using ReactiveUI;

namespace Bili.ViewModels.Interfaces.Search
{
    /// <summary>
    /// 搜索页面视图模型的接口定义.
    /// </summary>
    public interface ISearchPageViewModel : IInformationFlowViewModel<ISearchModuleItemViewModel>
    {
        /// <summary>
        /// 重载搜索模块命令.
        /// </summary>
        ReactiveCommand<Unit, Unit> ReloadModuleCommand { get; }

        /// <summary>
        /// 选中模块命令.
        /// </summary>
        ReactiveCommand<ISearchModuleItemViewModel, Unit> SelectModuleCommand { get; }

        /// <summary>
        /// 视频集合.
        /// </summary>
        ObservableCollection<IVideoItemViewModel> Videos { get; }

        /// <summary>
        /// 动画集合.
        /// </summary>
        ObservableCollection<ISeasonItemViewModel> Animes { get; }

        /// <summary>
        /// 影视集合.
        /// </summary>
        ObservableCollection<ISeasonItemViewModel> Movies { get; }

        /// <summary>
        /// 用户集合.
        /// </summary>
        ObservableCollection<IUserItemViewModel> Users { get; }

        /// <summary>
        /// 文章集合.
        /// </summary>
        ObservableCollection<IArticleItemViewModel> Articles { get; }

        /// <summary>
        /// 直播集合.
        /// </summary>
        ObservableCollection<ILiveItemViewModel> Lives { get; }

        /// <summary>
        /// 当前的过滤器集合.
        /// </summary>
        ObservableCollection<ISearchFilterViewModel> CurrentFilters { get; }

        /// <summary>
        /// 当前选中的模块.
        /// </summary>
        ISearchModuleItemViewModel CurrentModule { get; }

        /// <summary>
        /// 当前内容是否为空.
        /// </summary>
        bool IsCurrentContentEmpty { get; }

        /// <summary>
        /// 当前的过滤器是否为空.
        /// </summary>
        bool IsCurrentFilterEmpty { get; }

        /// <summary>
        /// 是否显示视频模块.
        /// </summary>
        bool IsVideoModuleShown { get; }

        /// <summary>
        /// 是否显示动漫模块.
        /// </summary>
        bool IsAnimeModuleShown { get; }

        /// <summary>
        /// 是否显示影视模块.
        /// </summary>
        bool IsMovieModuleShown { get; }

        /// <summary>
        /// 是否显示文章模块.
        /// </summary>
        bool IsArticleModuleShown { get; }

        /// <summary>
        /// 是否显示直播模块.
        /// </summary>
        bool IsLiveModuleShown { get; }

        /// <summary>
        /// 是否显示用户模块.
        /// </summary>
        bool IsUserModuleShown { get; }

        /// <summary>
        /// 关键词.
        /// </summary>
        string Keyword { get; }

        /// <summary>
        /// 是否正在重载模块.
        /// </summary>
        bool IsReloadingModule { get; }

        /// <summary>
        /// 设置搜索关键词.
        /// </summary>
        /// <param name="keyword">关键词.</param>
        void SetKeyword(string keyword);
    }
}
