// Copyright (c) Richasy. All rights reserved.

using System;
using System.IO;
using System.Threading.Tasks;
using Bili.Models.App.Args;
using Bili.Models.Data.Player;
using Bili.Models.Enums;

namespace Bili.ViewModels.Interfaces
{
    /// <summary>
    /// 播放器视图模型的接口定义.
    /// </summary>
    public interface IPlayerViewModel : IDisposable
    {
        /// <summary>
        /// 媒体已打开（成功连接到媒体源）.
        /// </summary>
        event EventHandler MediaOpened;

        /// <summary>
        /// 媒体状态变化.
        /// </summary>
        event EventHandler<MediaStateChangedEventArgs> StateChanged;

        /// <summary>
        /// 播放进度变化.
        /// </summary>
        event EventHandler<MediaPositionChangedEventArgs> PositionChanged;

        /// <summary>
        /// 媒体播放器发生改变.
        /// </summary>
        event EventHandler<object> MediaPlayerChanged;

        /// <summary>
        /// 当前媒体播放位置.
        /// </summary>
        TimeSpan Position { get; }

        /// <summary>
        /// 媒体总时长.
        /// </summary>
        TimeSpan Duration { get; }

        /// <summary>
        /// 媒体音量 (0-100).
        /// </summary>
        double Volume { get; }

        /// <summary>
        /// 当前播放速率.
        /// </summary>
        double PlayRate { get; }

        /// <summary>
        /// 当前媒体状态.
        /// </summary>
        PlayerStatus Status { get; }

        /// <summary>
        /// 是否循环播放.
        /// </summary>
        bool IsLoop { get; }

        /// <summary>
        /// 播放器是否准备就绪.
        /// </summary>
        bool IsPlayerReady { get; }

        /// <summary>
        /// 播放媒体.
        /// </summary>
        void Play();

        /// <summary>
        /// 暂停媒体.
        /// </summary>
        void Pause();

        /// <summary>
        /// 跳转进度.
        /// </summary>
        /// <param name="time">时长.</param>
        void SeekTo(TimeSpan time);

        /// <summary>
        /// 设置播放速率.
        /// </summary>
        /// <param name="rate">播放速率.</param>
        void SetPlayRate(double rate);

        /// <summary>
        /// 设置音量.
        /// </summary>
        /// <param name="volume">音量大小.</param>
        void SetVolume(double volume);

        /// <summary>
        /// 设置循环播放.
        /// </summary>
        /// <param name="isLoop">是否循环播放.</param>
        void SetLoop(bool isLoop);

        /// <summary>
        /// 设置显示在 SMTC 上的播放信息.
        /// </summary>
        /// <param name="cover">视频封面.</param>
        /// <param name="title">标题.</param>
        /// <param name="subtitle">副标题（说明文字）.</param>
        /// <param name="videoType">视频类型.</param>
        void SetDisplayProperties(string cover, string title, string subtitle, string videoType);

        /// <summary>
        /// 设置播放源.
        /// </summary>
        /// <param name="video">视频源.</param>
        /// <param name="audio">音频源.</param>
        /// <returns><see cref="Task"/>.</returns>
        Task SetSourceAsync(SegmentInformation video, SegmentInformation audio);

        /// <summary>
        /// 设置直播源.
        /// </summary>
        /// <param name="url">直播间地址.</param>
        /// <returns><see cref="Task"/>.</returns>
        Task SetSourceAsync(string url);

        /// <summary>
        /// 截图.
        /// </summary>
        /// <param name="targetFileStream">目标文件流.</param>
        /// <returns><see cref="Task"/>.</returns>
        Task ScreenshotAsync(Stream targetFileStream);
    }
}
