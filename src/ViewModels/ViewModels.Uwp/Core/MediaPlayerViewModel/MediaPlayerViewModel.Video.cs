// Copyright (c) Richasy. All rights reserved.

using System;
using System.Linq;
using System.Threading.Tasks;
using Bili.Models.Data.Player;
using Bili.Models.Data.Video;
using Bili.Models.Enums;

namespace Bili.ViewModels.Uwp.Core
{
    /// <summary>
    /// 媒体播放器视图模型.
    /// </summary>
    public sealed partial class MediaPlayerViewModel
    {
        private void ResetVideoData()
            => _currentPart = default;

        private async Task ChangeVideoPartAsync(VideoIdentifier part)
        {
            var view = _viewData as VideoPlayerView;
            if (string.IsNullOrEmpty(part.Id) || (!IsInteractionVideo && !view.SubVideos.Contains(part)))
            {
                return;
            }

            _currentPart = part;
            ResetPlayer();
            ResetMediaData();
            await LoadVideoAsync();
            StartTimersAndDisplayRequest();
        }

        private async Task LoadVideoAsync()
        {
            var view = _viewData as VideoPlayerView;
            IsInteractionVideo = view.InteractionVideo != null;
            if (string.IsNullOrEmpty(_currentPart.Id))
            {
                _currentPart = view.SubVideos.First();
                if (IsInteractionVideo)
                {
                    InteractionViewModel.SetData(view.Information.Identifier.Id, default, view.InteractionVideo.GraphVersion);
                }
            }

            SubtitleViewModel.SetData(view.Information.Identifier.Id, _currentPart.Id);
            DanmakuViewModel.SetData(view.Information.Identifier.Id, _currentPart.Id);
            await InitializeVideoMediaInformationAsync();
            await InitializeOrginalVideoSourceAsync();
            FillVideoPlaybackProperties();
            CheckVideoHistory();
        }

        private void CheckVideoHistory()
        {
            var view = _viewData as VideoPlayerView;
            if (view.Progress != null && view.Progress.Status == Models.Enums.Player.PlayedProgressStatus.Playing)
            {
                var history = view.Progress.Identifier;
                var ts = TimeSpan.FromSeconds(view.Progress.Progress);
                IsShowProgressTip = true;
                ProgressTip = $"{_resourceToolkit.GetLocaleString(LanguageNames.PreviousView)}{history.Title} {ts}";
            }
        }

        private async Task InitializeVideoMediaInformationAsync()
        {
            var view = _viewData as VideoPlayerView;
            _mediaInformation = await _playerProvider.GetVideoMediaInformationAsync(view.Information.Identifier.Id, _currentPart.Id);
            CheckVideoP2PUrls();
        }

        private async Task InitializeOrginalVideoSourceAsync()
        {
            var isVip = _accountViewModel.IsVip;
            if (isVip)
            {
                foreach (var item in _mediaInformation.Formats)
                {
                    item.IsLimited = false;
                }
            }

            foreach (var item in _mediaInformation.Formats)
            {
                Formats.Add(item);
            }

            var formatId = _settingsToolkit.ReadLocalSetting(SettingNames.IsPreferHighQuality, false)
                ? Formats.Where(p => !p.IsLimited).Max(p => p.Quality)
                : _settingsToolkit.ReadLocalSetting(SettingNames.DefaultVideoFormat, 64);
            if (!Formats.Any(p => p.Quality == formatId))
            {
                formatId = Formats.Where(p => !p.IsLimited).Max(p => p.Quality);
            }

            await SelectVideoFormatAsync(Formats.First(p => p.Quality == formatId));
        }

        private async Task SelectVideoFormatAsync(FormatInformation format)
        {
            MarkProgressBreakpoint();
            var codecId = GetVideoPreferCodecId();
            if (_mediaInformation.VideoSegments != null)
            {
                var filteredSegments = _mediaInformation.VideoSegments.Where(p => p.Id == format.Quality.ToString());
                if (!filteredSegments.Any())
                {
                    var maxQuality = _mediaInformation.VideoSegments.Max(p => Convert.ToInt32(p.Id));
                    _video = _mediaInformation.VideoSegments.First(p => p.Id == maxQuality.ToString());
                }
                else
                {
                    _video = filteredSegments.FirstOrDefault(p => p.Codecs.Contains(codecId))
                        ?? filteredSegments.First();
                }

                CurrentFormat = Formats.FirstOrDefault(p => p.Quality.ToString() == _video.Id);
            }

            if (_mediaInformation.AudioSegments != null)
            {
                // 音频直接上最大码率.
                var maxRate = _mediaInformation.AudioSegments.Max(p => Convert.ToInt32(p.Id));
                _audio = _mediaInformation.AudioSegments.First(p => p.Id == maxRate.ToString());
            }

            if (_video == null)
            {
                IsError = true;
                ErrorText = _resourceToolkit.GetLocaleString(LanguageNames.SourceNotSupported);
                return;
            }

            await InitializeVideoPlayerAsync();
        }

        private async Task InitializeVideoPlayerAsync()
        {
            InitializeMediaPlayer();
            var source = await GetDashVideoSourceAsync();
            _playbackItem = new Windows.Media.Playback.MediaPlaybackItem(source);
            _mediaPlayer.Source = _playbackItem;
        }

        private void CheckVideoP2PUrls()
        {
            if (!_settingsToolkit.ReadLocalSetting(SettingNames.DisableP2PCdn, false))
            {
                return;
            }

            // 剔除 P2P CDN URL
            if (_mediaInformation.AudioSegments != null)
            {
                var filteredAudios = _mediaInformation.AudioSegments.Where(p => !p.BaseUrl.Contains("bilivideo.com"));
                foreach (var item in filteredAudios)
                {
                    item.BaseUrl = item.BackupUrls.FirstOrDefault(p => p.Contains("bilivideo.com")) ?? item.BaseUrl;
                }
            }

            if (_mediaInformation.VideoSegments != null)
            {
                var filteredAudios = _mediaInformation.VideoSegments.Where(p => !p.BaseUrl.Contains("bilivideo.com"));
                foreach (var item in filteredAudios)
                {
                    item.BaseUrl = item.BackupUrls.FirstOrDefault(p => p.Contains("bilivideo.com")) ?? item.BaseUrl;
                }
            }
        }

        private void FillVideoPlaybackProperties()
        {
            var view = _viewData as VideoPlayerView;
            var props = _playbackItem.GetDisplayProperties();
            props.Type = Windows.Media.MediaPlaybackType.Video;
            props.Thumbnail = Windows.Storage.Streams.RandomAccessStreamReference.CreateFromUri(new Uri(view.Information.Identifier.Cover.GetSourceUri().ToString() + "@100w_100h_1c_100q.jpg"));
            props.VideoProperties.Title = view.Information.Identifier.Title;
            props.VideoProperties.Subtitle = string.Join(string.Empty, view.Information.Description.Take(20));
            props.VideoProperties.Genres.Add(_videoType.ToString());
            _playbackItem.ApplyDisplayProperties(props);
        }

        private void SelectInteractionChoice(InteractionInformation info)
        {
            IsShowInteractionChoices = false;
            IsInteractionEnd = false;
            if (_videoType != VideoType.Video)
            {
                return;
            }

            if (_viewData is not VideoPlayerView view || view.InteractionVideo == null)
            {
                return;
            }

            InteractionViewModel.SetData(view.Information.Identifier.Id, info.Id, view.InteractionVideo.GraphVersion);
            var part = new VideoIdentifier(info.PartId, default, default, default);
            ChangePartCommand.Execute(part).Subscribe();
        }

        private void BackToInteractionVideoStart()
        {
            IsShowInteractionChoices = false;
            IsInteractionEnd = false;
            if (_viewData is not VideoPlayerView view || view.InteractionVideo == null)
            {
                return;
            }

            InteractionViewModel.SetData(view.Information.Identifier.Id, default, view.InteractionVideo.GraphVersion);
            var part = view.SubVideos.FirstOrDefault();
            ChangePartCommand.Execute(part).Subscribe();
        }
    }
}
