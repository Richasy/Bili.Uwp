// Copyright (c) Richasy. All rights reserved.

using System.Reactive;
using Bili.Models.Data.Pgc;
using ReactiveUI;

namespace Bili.ViewModels.Interfaces.Pgc
{
    /// <summary>
    /// 剧集条目视图模型的接口定义.
    /// </summary>
    public interface ISeasonItemViewModel : IVideoBaseViewModel<SeasonInformation>, IInjectActionViewModel<ISeasonItemViewModel>
    {
        /// <summary>
        /// 在网页中打开的命令.
        /// </summary>
        ReactiveCommand<Unit, Unit> OpenInBroswerCommand { get; }

        /// <summary>
        /// 播放命令.
        /// </summary>
        ReactiveCommand<Unit, Unit> PlayCommand { get; }

        /// <summary>
        /// 取消关注命令.
        /// </summary>
        ReactiveCommand<Unit, Unit> UnfollowCommand { get; }

        /// <summary>
        /// 改变收藏状态命令.
        /// </summary>
        ReactiveCommand<int, Unit> ChangeFavoriteStatusCommand { get; }

        /// <summary>
        /// 是否显示评分.
        /// </summary>
        bool IsShowRating { get; }

        /// <summary>
        /// 追番次数文本.
        /// </summary>
        string TrackCountText { get; }

        /// <summary>
        /// 是否被选中.
        /// </summary>
        bool IsSelected { get; set; }
    }
}
