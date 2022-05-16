// Copyright (c) Richasy. All rights reserved.

using System;
using System.Threading.Tasks;
using Bili.Lib.Interfaces;
using Bili.Models.Data.Video;
using Bili.Toolkit.Interfaces;
using ReactiveUI;
using Windows.System;

namespace Bili.ViewModels.Uwp.Video
{
    /// <summary>
    /// 视频条目视图模型.
    /// </summary>
    public sealed partial class VideoItemViewModel : ViewModelBase, IVideoBaseViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VideoItemViewModel"/> class.
        /// </summary>
        /// <param name="numberToolkit">数字转换工具.</param>
        /// <param name="accountProvider">账户服务提供工具.</param>
        /// <param name="authorizeProvider">授权服务提供工具.</param>
        public VideoItemViewModel(
            INumberToolkit numberToolkit,
            IAccountProvider accountProvider,
            IAuthorizeProvider authorizeProvider)
        {
            _numberToolkit = numberToolkit;
            _accountProvider = accountProvider;
            _authorizeProvider = authorizeProvider;

            PlayCommand = ReactiveCommand.Create(Play, outputScheduler: RxApp.MainThreadScheduler);
            AddToViewLaterCommand = ReactiveCommand.CreateFromTask(AddToViewLaterAsync, outputScheduler: RxApp.MainThreadScheduler);
            OpenInBroswerCommand = ReactiveCommand.CreateFromTask(OpenInBroswerAsync, outputScheduler: RxApp.MainThreadScheduler);
        }

        /// <summary>
        /// 设置视频信息，并进行视图模型的初始化.
        /// </summary>
        /// <param name="information">视频信息.</param>
        public void SetInformation(IVideoBase information)
        {
            Information = information as VideoInformation;
            InitializeData();
        }

        private void InitializeData()
        {
            PlayCountText = _numberToolkit.GetCountText(Information.CommunityInformation.PlayCount);
            DanmakuCountText = _numberToolkit.GetCountText(Information.CommunityInformation.DanmakuCount);
            LikeCountText = _numberToolkit.GetCountText(Information.CommunityInformation.LikeCount);

            IsShowScore = Information.CommunityInformation?.Score > 0;
            if (Information.Identifier.Duration > 0)
            {
                DurationText = _numberToolkit.GetDurationText(TimeSpan.FromSeconds(Information.Identifier.Duration));
            }
        }

        private void Play()
            => AppViewModel.Instance.OpenPlayer(Information);

        private async Task AddToViewLaterAsync()
        {
            if (_authorizeProvider.State == Models.Enums.AuthorizeState.SignedIn)
            {
                var result = await _accountProvider.AddVideoToViewLaterAsync(Information.Identifier.Id);
                if (result)
                {
                    // 显示添加成功的消息.
                }
                else
                {
                    // 显示添加失败的消息.
                }
            }
            else
            {
                // 显示需要登录的消息.
            }
        }

        private async Task OpenInBroswerAsync()
        {
            var uri = $"https://www.bilibili.com/video/av{Information.Identifier.Id}";
            await Launcher.LaunchUriAsync(new Uri(uri));
        }
    }
}
