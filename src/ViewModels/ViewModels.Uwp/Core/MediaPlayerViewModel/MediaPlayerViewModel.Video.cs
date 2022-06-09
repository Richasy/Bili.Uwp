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
        {
            Formats.Clear();
            _currentPart = default;
            IsShowProgressTip = false;
            ProgressTip = default;
            _video = null;
            _audio = null;
            CurrentFormat = null;
        }

        private async Task LoadVideoAsync()
        {
            var view = _viewData as VideoView;
            if (string.IsNullOrEmpty(_currentPart.Id) || !view.SubVideos.Contains(_currentPart))
            {
                _currentPart = view.SubVideos.First();
            }

            // 处理历史记录.
            if (view.Progress != null && view.Progress.Status == Models.Enums.Player.PlayedProgressStatus.Playing)
            {
                var history = view.Progress.Identifier;
                var ts = TimeSpan.FromSeconds(view.Progress.Progress);
                IsShowProgressTip = true;
                ProgressTip = $"{_resourceToolkit.GetLocaleString(LanguageNames.PreviousView)}{history.Title} {ts}";
            }

            _mediaInformation = await _playerProvider.GetVideoMediaInformationAsync(view.Information.Identifier.Id, _currentPart.Id);
            CheckP2PUrls();
            await InitializeOrginalVideoSourceAsync();
        }

        private void CheckP2PUrls()
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

        /// <summary>
        /// 选择初始片源.
        /// </summary>
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

            await SelectFormatAsync(Formats.First(p => p.Quality == formatId));
        }

        private async Task SelectFormatAsync(FormatInformation format)
        {
            var codecId = GetPreferCodecId();
            if (_mediaInformation.VideoSegments != null)
            {
                var filteredSegments = _mediaInformation.VideoSegments.Where(p => p.Id == CurrentFormat.Quality.ToString());
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
            var source = await GetDashVideoSourceAsync();
            _playbackItem = new Windows.Media.Playback.MediaPlaybackItem(source);
            FillVideoPlaybackProperties();

            // 初始化MediaPlayer.
            InitializeMediaPlayer();
        }

        private string GetPreferCodecId()
        {
            var id = "avc";
            var preferCodec = _settingsToolkit.ReadLocalSetting(SettingNames.PreferCodec, PreferCodec.H264);
            switch (preferCodec)
            {
                case PreferCodec.H265:
                    id = "hev";
                    break;
                case PreferCodec.Av1:
                    id = "av01";
                    break;
                default:
                    break;
            }

            return id;
        }

        private void FillVideoPlaybackProperties()
        {
            var view = _viewData as VideoView;
            var props = _playbackItem.GetDisplayProperties();
            props.Type = Windows.Media.MediaPlaybackType.Video;
            props.Thumbnail = Windows.Storage.Streams.RandomAccessStreamReference.CreateFromUri(new Uri(view.Information.Identifier.Cover.GetSourceUri().ToString() + "@100w_100h_1c_100q.jpg"));
            props.VideoProperties.Title = view.Information.Identifier.Title;
            props.VideoProperties.Subtitle = string.Join(string.Empty, view.Information.Description.Take(20));
            props.VideoProperties.Genres.Add(_videoType.ToString());
            _playbackItem.ApplyDisplayProperties(props);
        }
    }
}
