// Copyright (c) Richasy. All rights reserved.

using Bili.Models.Data.Video;
using Bili.ViewModels.Interfaces.Account;
using CommunityToolkit.Mvvm.Input;

namespace Bili.ViewModels.Interfaces.Video
{
    /// <summary>
    /// 视频条目视图模型的接口定义.
    /// </summary>
    public interface IVideoItemViewModel : IVideoBaseViewModel<VideoInformation>, IInjectActionViewModel<IVideoItemViewModel>
    {
        /// <summary>
        /// 添加到稍后再看的命令.
        /// </summary>
        public IAsyncRelayCommand AddToViewLaterCommand { get; }

        /// <summary>
        /// 从稍后再看中移除的命令.
        /// </summary>
        public IAsyncRelayCommand RemoveFromViewLaterCommand { get; }

        /// <summary>
        /// 从历史记录中移除的命令.
        /// </summary>
        public IAsyncRelayCommand RemoveFromHistoryCommand { get; }

        /// <summary>
        /// 从指定收藏夹中移除的命令.
        /// </summary>
        public IAsyncRelayCommand RemoveFromFavoriteCommand { get; }

        /// <summary>
        /// 在网页中打开的命令.
        /// </summary>
        public IAsyncRelayCommand OpenInBroswerCommand { get; }

        /// <summary>
        /// 播放命令.
        /// </summary>
        public IAsyncRelayCommand PlayCommand { get; }

        /// <summary>
        /// 无痕播放命令.
        /// </summary>
        public IAsyncRelayCommand PlayInPrivateCommand { get; }

        /// <summary>
        /// 发布者.
        /// </summary>
        public IUserItemViewModel Publisher { get; }

        /// <summary>
        /// 播放次数的可读文本.
        /// </summary>
        public string PlayCountText { get; }

        /// <summary>
        /// 弹幕数的可读文本.
        /// </summary>
        public string DanmakuCountText { get; }

        /// <summary>
        /// 点赞数的可读文本.
        /// </summary>
        public string LikeCountText { get; }

        /// <summary>
        /// 视频时长.
        /// </summary>
        public string DurationText { get; }

        /// <summary>
        /// 是否显示评分.
        /// </summary>
        public bool IsShowScore { get; }

        /// <summary>
        /// 分数文本.
        /// </summary>
        public string ScoreText { get; }

        /// <summary>
        /// 是否显示社区信息.
        /// </summary>
        public bool IsShowCommunity { get; }

        /// <summary>
        /// 是否被选中.
        /// </summary>
        public bool IsSelected { get; set; }

        /// <summary>
        /// 设置附加数据.
        /// </summary>
        /// <param name="data">附加数据.</param>
        void SetAdditionalData(object data);
    }
}
