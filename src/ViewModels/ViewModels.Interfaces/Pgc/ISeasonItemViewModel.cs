// Copyright (c) Richasy. All rights reserved.

using Bili.Models.Data.Pgc;
using CommunityToolkit.Mvvm.Input;

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
        IRelayCommand OpenInBroswerCommand { get; }

        /// <summary>
        /// 播放命令.
        /// </summary>
        IRelayCommand PlayCommand { get; }

        /// <summary>
        /// 取消关注命令.
        /// </summary>
        IRelayCommand UnfollowCommand { get; }

        /// <summary>
        /// 改变收藏状态命令.
        /// </summary>
        IRelayCommand<int> ChangeFavoriteStatusCommand { get; }

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
