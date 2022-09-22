// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using Bili.Models.App.Other;
using Bili.Models.Data.Live;
using Bili.Models.Data.Local;
using Bili.ViewModels.Interfaces.Account;
using CommunityToolkit.Mvvm.Input;

namespace Bili.ViewModels.Interfaces.Live
{
    /// <summary>
    /// 直播播放页面视图模型的接口定义.
    /// </summary>
    public interface ILivePlayerPageViewModel : IPlayerPageViewModel, IReloadViewModel, IErrorViewModel
    {
        /// <summary>
        /// 当有新的弹幕传入，预期让弹幕池滚动到底部的事件.
        /// </summary>
        public event EventHandler RequestDanmakusScrollToBottom;

        /// <summary>
        /// 分享命令.
        /// </summary>
        public IRelayCommand ShareCommand { get; }

        /// <summary>
        /// 固定条目命令.
        /// </summary>
        public IRelayCommand FixedCommand { get; }

        /// <summary>
        /// 清除数据命令.
        /// </summary>
        public IRelayCommand ClearCommand { get; }

        /// <summary>
        /// 在网页中打开的命令.
        /// </summary>
        public IRelayCommand OpenInBroswerCommand { get; }

        /// <summary>
        /// 弹幕池.
        /// </summary>
        public ObservableCollection<LiveDanmakuInformation> Danmakus { get; }

        /// <summary>
        /// 播放时的关联区块集合.
        /// </summary>
        public ObservableCollection<PlayerSectionHeader> Sections { get; }

        /// <summary>
        /// 视图信息.
        /// </summary>
        public LivePlayerView View { get; }

        /// <summary>
        /// 用户是否已登录.
        /// </summary>
        public bool IsSignedIn { get; set; }

        /// <summary>
        /// 直播 UP 主.
        /// </summary>
        public IUserItemViewModel User { get; }

        /// <summary>
        /// 正在观看人数的可读文本.
        /// </summary>
        public string WatchingCountText { get; }

        /// <summary>
        /// 该直播是否已经被固定在首页.
        /// </summary>
        public bool IsLiveFixed { get; set; }

        /// <summary>
        /// 当前区块.
        /// </summary>
        public PlayerSectionHeader CurrentSection { get; set; }

        /// <summary>
        /// 弹幕池是否为空.
        /// </summary>
        public bool IsDanmakusEmpty { get; }

        /// <summary>
        /// 是否允许弹幕池自动滚动.
        /// </summary>
        public bool IsDanmakusAutoScroll { get; set; }

        /// <summary>
        /// 设置直播间.
        /// </summary>
        /// <param name="snapshot">直播间信息.</param>
        void SetSnapshot(PlaySnapshot snapshot);
    }
}
