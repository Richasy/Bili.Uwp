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
            StartTimers();
            InitializeSmtc();
        }

        private async Task LoadVideoAsync()
        {
            InitializeVideoInformation();
            await InitializeVideoMediaInformationAsync();
            CheckVideoHistory();
            await InitializeOrginalVideoSourceAsync();

            var view = _viewData as VideoPlayerView;
            SubtitleViewModel.SetData(view.Information.Identifier.Id, _currentPart.Id);
            DanmakuViewModel.SetData(view.Information.Identifier.Id, _currentPart.Id, _videoType);
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

        private void InitializeVideoInformation()
        {
            var view = _viewData as VideoPlayerView;
            Cover = view.Information.Identifier.Cover.GetSourceUri().ToString();
            IsInteractionVideo = view.InteractionVideo != null;
            if (string.IsNullOrEmpty(_currentPart.Id))
            {
                _currentPart = view.SubVideos.First();
                if (IsInteractionVideo)
                {
                    InteractionViewModel.SetData(view.Information.Identifier.Id, default, view.InteractionVideo.GraphVersion);
                }
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
                if (!item.IsLimited)
                {
                    Formats.Add(item);
                }
            }

            var formatId = GetFormatId();
            await SelectVideoFormatAsync(Formats.First(p => p.Quality == formatId));
        }

        private async Task SelectVideoFormatAsync(FormatInformation format)
        {
            MarkProgressBreakpoint();
            var codecId = GetVideoPreferCodecId();
            ResetPlayer();
            InitializePlayer();
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
                _settingsToolkit.WriteLocalSetting(SettingNames.DefaultVideoFormat, CurrentFormat.Quality);
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

            try
            {
                await _player.SetSourceAsync(_video, _audio);
                StartTimers();
            }
            catch (Exception ex)
            {
                IsError = true;
                ErrorText = _resourceToolkit.GetLocaleString(LanguageNames.RequestVideoFailed);
                LogException(ex);
            }
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
                var filteredAudios = _mediaInformation.AudioSegments.Where(p => !(p.BaseUrl?.Contains("bilivideo.com") ?? false));
                if (filteredAudios.Any())
                {
                    foreach (var item in filteredAudios)
                    {
                        item.BaseUrl = item.BackupUrls?.FirstOrDefault(p => p?.Contains("bilivideo.com") ?? false) ?? item.BaseUrl;
                    }
                }
            }

            if (_mediaInformation.VideoSegments != null)
            {
                var filteredVideos = _mediaInformation.VideoSegments.Where(p => !(p.BaseUrl?.Contains("bilivideo.com") ?? false));
                if (filteredVideos.Any())
                {
                    foreach (var item in filteredVideos)
                    {
                        item.BaseUrl = item.BackupUrls?.FirstOrDefault(p => p?.Contains("bilivideo.com") ?? false) ?? item.BaseUrl;
                    }
                }
            }
        }

        private void FillVideoPlaybackProperties()
        {
            var view = _viewData as VideoPlayerView;
            SetDisplayProperties(
                view.Information.Identifier.Cover.GetSourceUri().ToString() + "@100w_100h_1c_100q.jpg",
                view.Information.Identifier.Title,
                string.Join(string.Empty, view.Information.Description.Take(20)),
                _videoType.ToString());
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
            ChangePartCommand.ExecuteAsync(part);
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
            ChangePartCommand.ExecuteAsync(part);
        }
    }
}
