// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Bili.Models.Data.Live;
using Bili.Models.Data.Local;
using Bili.Models.Data.Player;
using Bili.Models.Enums;
using Bili.Models.Enums.App;
using CommunityToolkit.Mvvm.Input;

namespace Bili.ViewModels.Interfaces.Common
{
    /// <summary>
    /// 弹幕模块视图模型的接口定义.
    /// </summary>
    public interface IDanmakuModuleViewModel : INotifyPropertyChanged, IReloadViewModel
    {
        /// <summary>
        /// 弹幕列表已添加.
        /// </summary>
        event EventHandler<IEnumerable<DanmakuInformation>> DanmakuListAdded;

        /// <summary>
        /// 已成功发送弹幕.
        /// </summary>
        event EventHandler<string> SendDanmakuSucceeded;

        /// <summary>
        /// 请求清除弹幕列表.
        /// </summary>
        event EventHandler RequestClearDanmaku;

        /// <summary>
        /// 直播弹幕已添加.
        /// </summary>
        event EventHandler<LiveDanmakuInformation> LiveDanmakuAdded;

        /// <summary>
        /// 弹幕位置可选集合.
        /// </summary>
        ObservableCollection<DanmakuLocation> LocationCollection { get; }

        /// <summary>
        /// 弹幕颜色集合.
        /// </summary>
        ObservableCollection<KeyValue<string>> ColorCollection { get; }

        /// <summary>
        /// 系统字体集合.
        /// </summary>
        ObservableCollection<string> FontCollection { get; }

        /// <summary>
        /// 重置命令.
        /// </summary>
        IRelayCommand ResetCommand { get; }

        /// <summary>
        /// 发送弹幕命令.
        /// </summary>
        IRelayCommand<string> SendDanmakuCommand { get; }

        /// <summary>
        /// 获取分片弹幕命令.
        /// </summary>
        IRelayCommand<int> LoadSegmentDanmakuCommand { get; }

        /// <summary>
        /// 重新定位命令.
        /// </summary>
        IRelayCommand<double> SeekCommand { get; }

        /// <summary>
        /// 添加新的直播弹幕命令.
        /// </summary>
        IRelayCommand<LiveDanmakuInformation> AddLiveDanmakuCommand { get; }

        /// <summary>
        /// 是否显示弹幕.
        /// </summary>
        bool IsShowDanmaku { get; set; }

        /// <summary>
        /// 是否可以显示弹幕.
        /// </summary>
        bool CanShowDanmaku { get; set; }

        /// <summary>
        /// 弹幕透明度.
        /// </summary>
        double DanmakuOpacity { get; set; }

        /// <summary>
        /// 弹幕文本大小.
        /// </summary>
        double DanmakuFontSize { get; set; }

        /// <summary>
        /// 弹幕显示区域.
        /// </summary>
        double DanmakuArea { get; set; }

        /// <summary>
        /// 弹幕速度.
        /// </summary>
        double DanmakuSpeed { get; set; }

        /// <summary>
        /// 弹幕字体.
        /// </summary>
        string DanmakuFont { get; set; }

        /// <summary>
        /// 是否启用同屏弹幕限制.
        /// </summary>
        bool IsDanmakuLimit { get; set; }

        /// <summary>
        /// 是否启用弹幕合并.
        /// </summary>
        bool IsDanmakuMerge { get; set; }

        /// <summary>
        /// 是否加粗弹幕.
        /// </summary>
        bool IsDanmakuBold { get; set; }

        /// <summary>
        /// 是否启用云屏蔽设置.
        /// </summary>
        bool UseCloudShieldSettings { get; set; }

        /// <summary>
        /// 是否为标准字号.
        /// </summary>
        bool IsStandardSize { get; set; }

        /// <summary>
        /// 弹幕位置.
        /// </summary>
        DanmakuLocation Location { get; set; }

        /// <summary>
        /// 弹幕颜色.
        /// </summary>
        string Color { get; set; }

        /// <summary>
        /// 是否正在加载分片弹幕.
        /// </summary>
        bool IsDanmakuLoading { get; }

        /// <summary>
        /// 加载弹幕.
        /// </summary>
        /// <param name="mainId">视频 Id.</param>
        /// <param name="partId">分P Id.</param>
        void SetData(string mainId, string partId, VideoType type);
    }
}
