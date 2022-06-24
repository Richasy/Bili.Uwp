// Copyright (c) Richasy. All rights reserved.

using System;
using System.Threading.Tasks;
using Bili.Lib.Interfaces;
using Bili.Models.Data.Video;
using Bili.Models.Enums;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces;
using Bili.ViewModels.Uwp.Account;
using Bili.ViewModels.Uwp.Core;
using ReactiveUI;
using Splat;
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
        public VideoItemViewModel(
            INumberToolkit numberToolkit,
            IAccountProvider accountProvider,
            IAuthorizeProvider authorizeProvider,
            IFavoriteProvider favoriteProvider,
            IResourceToolkit resourceToolkit,
            NavigationViewModel navigationViewModel,
            AppViewModel appViewModel)
        {
            _numberToolkit = numberToolkit;
            _accountProvider = accountProvider;
            _authorizeProvider = authorizeProvider;
            _favoriteProvider = favoriteProvider;
            _resourceToolkit = resourceToolkit;
            _navigationViewModel = navigationViewModel;
            _appViewModel = appViewModel;

            PlayCommand = ReactiveCommand.Create(Play, outputScheduler: RxApp.MainThreadScheduler);
            AddToViewLaterCommand = ReactiveCommand.CreateFromTask(AddToViewLaterAsync, outputScheduler: RxApp.MainThreadScheduler);
            RemoveFromViewLaterCommand = ReactiveCommand.CreateFromTask(RemoveFromViewLaterAsync, outputScheduler: RxApp.MainThreadScheduler);
            RemoveFromHistoryCommand = ReactiveCommand.CreateFromTask(RemoveFromHistoryAsync, outputScheduler: RxApp.MainThreadScheduler);
            OpenInBroswerCommand = ReactiveCommand.CreateFromTask(OpenInBroswerAsync, outputScheduler: RxApp.MainThreadScheduler);
            RemoveFromFavoriteCommand = ReactiveCommand.CreateFromTask(RemoveFromFavoriteAsync, outputScheduler: RxApp.MainThreadScheduler);
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

        /// <summary>
        /// 设置附加动作，该动作通常发生在删除视频的过程中，连带删除调用源集合中的视频.
        /// </summary>
        /// <param name="action">附加动作.</param>
        public void SetAdditionalAction(Action<VideoItemViewModel> action)
            => _additionalAction = action;

        /// <summary>
        /// 设置附加数据.
        /// </summary>
        /// <param name="data">附加数据.</param>
        public void SetAdditionalData(object data)
            => _additionalData = data;

        private void InitializeData()
        {
            IsShowCommunity = Information.CommunityInformation != null;
            var userVM = Splat.Locator.Current.GetService<UserItemViewModel>();
            userVM.SetProfile(Information.Publisher);
            Publisher = userVM;
            if (IsShowCommunity)
            {
                PlayCountText = _numberToolkit.GetCountText(Information.CommunityInformation.PlayCount);
                DanmakuCountText = _numberToolkit.GetCountText(Information.CommunityInformation.DanmakuCount);
                LikeCountText = _numberToolkit.GetCountText(Information.CommunityInformation.LikeCount);

                IsShowScore = Information.CommunityInformation?.Score > 0;
                ScoreText = IsShowScore ?
                    Information.CommunityInformation.Score.ToString("0")
                    : default;
            }

            if (Information.Identifier.Duration > 0)
            {
                DurationText = _numberToolkit.GetDurationText(TimeSpan.FromSeconds(Information.Identifier.Duration));
            }
        }

        private void Play()
        {
            var snapshot = new Models.Data.Local.PlaySnapshot(Information.Identifier.Id, "0", VideoType.Video);
            if (_navigationViewModel.IsPlayViewShown && _navigationViewModel.PlayViewId == PageIds.VideoPlayer)
            {
                var videoPlayerPageVM = Splat.Locator.Current.GetService<VideoPlayerPageViewModel>();
                videoPlayerPageVM.SetSnapshot(snapshot);
            }
            else
            {
                _navigationViewModel.NavigateToPlayView(snapshot);
            }
        }

        private async Task AddToViewLaterAsync()
        {
            if (_authorizeProvider.State == AuthorizeState.SignedIn)
            {
                var result = await _accountProvider.AddVideoToViewLaterAsync(Information.Identifier.Id);
                if (result)
                {
                    // 显示添加成功的消息.
                    _appViewModel.ShowTip(
                        _resourceToolkit.GetLocaleString(LanguageNames.AddViewLaterSucceseded),
                        Models.Enums.App.InfoType.Success);
                }
                else
                {
                    // 显示添加失败的消息.
                    _appViewModel.ShowTip(
                        _resourceToolkit.GetLocaleString(LanguageNames.AddViewLaterFailed),
                        Models.Enums.App.InfoType.Error);
                }
            }
            else
            {
                // 显示需要登录的消息.
                _appViewModel.ShowTip(
                        _resourceToolkit.GetLocaleString(LanguageNames.NeedLoginFirst),
                        Models.Enums.App.InfoType.Warning);
            }
        }

        private async Task RemoveFromViewLaterAsync()
        {
            if (_authorizeProvider.State == AuthorizeState.SignedIn)
            {
                var result = await _accountProvider.RemoveVideoFromViewLaterAsync(Information.Identifier.Id);
                if (!result)
                {
                    // 显示移除失败的消息.
                    _appViewModel.ShowTip(
                        _resourceToolkit.GetLocaleString(LanguageNames.RemoveViewLaterFailed),
                        Models.Enums.App.InfoType.Error);
                }
                else
                {
                    _additionalAction?.Invoke(this);
                }
            }
            else
            {
                // 显示需要登录的消息.
                _appViewModel.ShowTip(
                        _resourceToolkit.GetLocaleString(LanguageNames.NeedLoginFirst),
                        Models.Enums.App.InfoType.Warning);
            }
        }

        private async Task RemoveFromHistoryAsync()
        {
            if (_authorizeProvider.State == AuthorizeState.SignedIn)
            {
                var result = await _accountProvider.RemoveHistoryItemAsync(Information.Identifier.Id);
                if (!result)
                {
                    // 显示移除失败的消息.
                    _appViewModel.ShowTip(
                        _resourceToolkit.GetLocaleString(LanguageNames.FailedToRemoveVideoFromHistory),
                        Models.Enums.App.InfoType.Error);
                }
                else
                {
                    _additionalAction?.Invoke(this);
                }
            }
            else
            {
                // 显示需要登录的消息.
                _appViewModel.ShowTip(
                        _resourceToolkit.GetLocaleString(LanguageNames.NeedLoginFirst),
                        Models.Enums.App.InfoType.Warning);
            }
        }

        private async Task RemoveFromFavoriteAsync()
        {
            if (_authorizeProvider.State == AuthorizeState.SignedIn)
            {
                var folderId = _additionalData.ToString();
                var result = await _favoriteProvider.RemoveFavoriteVideoAsync(folderId, Information.Identifier.Id);
                if (!result)
                {
                    // 显示移除失败的消息.
                    _appViewModel.ShowTip(
                        _resourceToolkit.GetLocaleString(LanguageNames.FailedToRemoveVideoFromFavorite),
                        Models.Enums.App.InfoType.Error);
                }
                else
                {
                    _additionalAction?.Invoke(this);
                }
            }
            else
            {
                // 显示需要登录的消息.
                _appViewModel.ShowTip(
                        _resourceToolkit.GetLocaleString(LanguageNames.NeedLoginFirst),
                        Models.Enums.App.InfoType.Warning);
            }
        }

        private async Task OpenInBroswerAsync()
        {
            var uri = $"https://www.bilibili.com/video/av{Information.Identifier.Id}";
            await Launcher.LaunchUriAsync(new Uri(uri));
        }
    }
}
