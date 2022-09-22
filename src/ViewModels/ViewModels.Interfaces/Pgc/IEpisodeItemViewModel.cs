// Copyright (c) Richasy. All rights reserved.

using Bili.Models.Data.Pgc;
using CommunityToolkit.Mvvm.Input;

namespace Bili.ViewModels.Interfaces.Pgc
{
    /// <summary>
    /// 单集条目视图模型的接口定义.
    /// </summary>
    public interface IEpisodeItemViewModel : IVideoBaseViewModel<EpisodeInformation>
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
        /// 播放次数的可读文本.
        /// </summary>
        string PlayCountText { get; }

        /// <summary>
        /// 弹幕数的可读文本.
        /// </summary>
        string DanmakuCountText { get; }

        /// <summary>
        /// 追番/追剧次数的可读文本.
        /// </summary>
        string TrackCountText { get; }

        /// <summary>
        /// 时长的可读文本.
        /// </summary>
        string DurationText { get; }

        /// <summary>
        /// 是否被选中.
        /// </summary>
        bool IsSelected { get; set; }
    }
}
