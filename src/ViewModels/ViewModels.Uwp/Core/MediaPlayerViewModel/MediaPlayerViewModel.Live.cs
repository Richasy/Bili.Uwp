// Copyright (c) Richasy. All rights reserved.

using System;
using System.Linq;
using System.Threading.Tasks;
using Bili.Models.Data.Live;
using Bili.Models.Data.Player;
using Bili.Models.Enums;

namespace Bili.ViewModels.Uwp.Core
{
    /// <summary>
    /// 媒体播放器视图模型.
    /// </summary>
    public sealed partial class MediaPlayerViewModel
    {
        private void ResetLiveData()
            => _currentPlayline = default;

        private async Task LoadLiveAsync()
        {
            await InitializeLiveMediaInformationAsync();
            await InitializeOrginalLiveSourceAsync();

            if (_playbackItem != null)
            {
                FillLivePlaybackProperties();
            }
        }

        private async Task InitializeLiveMediaInformationAsync()
        {
            var view = _viewData as LivePlayerView;
            var quality = _currentPlayline != null
                ? _currentPlayline.Quality
                : 150;

            DanmakuViewModel.SetData(view.Information.Identifier.Id, default, _videoType);
            _liveMediaInformation = await _liveProvider.GetLiveMediaInformationAsync(view.Information.Identifier.Id, quality, IsLiveAudioOnly);

            if (_currentPlayline == null)
            {
                _currentPlayline = _liveMediaInformation.Lines.FirstOrDefault(p => p.Quality == quality) ?? _liveMediaInformation.Lines.First();
            }
        }

        private async Task InitializeOrginalLiveSourceAsync()
        {
            var isVip = _accountViewModel.IsVip;
            if (isVip)
            {
                foreach (var item in _liveMediaInformation.Formats)
                {
                    item.IsLimited = false;
                }
            }

            foreach (var item in _liveMediaInformation.Formats)
            {
                Formats.Add(item);
            }

            var formatId = _settingsToolkit.ReadLocalSetting(SettingNames.IsPreferHighQuality, false)
                ? Formats.Where(p => !p.IsLimited).Max(p => p.Quality)
                : 150;
            if (!Formats.Any(p => p.Quality == formatId))
            {
                formatId = Formats.Where(p => !p.IsLimited).Max(p => p.Quality);
            }

            await SelectLiveFormatAsync(Formats.First(p => p.Quality == formatId));
        }

        private async Task SelectLiveFormatAsync(FormatInformation format)
        {
            CurrentFormat = format;
            var view = _viewData as LivePlayerView;
            var codecId = GetLivePreferCodecId();
            var quality = format.Quality;
            _liveMediaInformation = await _liveProvider.GetLiveMediaInformationAsync(view.Information.Identifier.Id, quality, IsLiveAudioOnly);
            if (_liveMediaInformation.Lines != null)
            {
                var playlines = _liveMediaInformation.Lines.Where(p => p.Name == codecId);
                if (playlines.Count() == 0)
                {
                    playlines = _liveMediaInformation.Lines.Where(p => p.Urls.Any(j => j.Host.EndsWith(".com")));
                }

                var url = playlines.SelectMany(p => p.Urls).FirstOrDefault(p => p.Host.EndsWith(".com"));
                if (url == null)
                {
                    IsError = true;
                    ErrorText = "无法获取正确的播放地址";

                    return;
                }

                await InitializeLivePlayerAsync(url.ToString());
            }
        }

        private async Task InitializeLivePlayerAsync(string url)
        {
            InitializeMediaPlayer();
            _playbackItem = await GetDashLiveSourceAsync(url.ToString());
            _mediaPlayer.Source = _playbackItem;
        }

        private async Task ChangeLiveAudioOnlyAsync(bool isAudioOnly)
        {
            IsLiveAudioOnly = isAudioOnly;
            _settingsToolkit.WriteLocalSetting(SettingNames.IsLiveAudioOnly, isAudioOnly);
            if (CurrentFormat != null)
            {
                await SelectLiveFormatAsync(CurrentFormat);
            }
        }

        private void FillLivePlaybackProperties()
        {
            var view = _viewData as LivePlayerView;
            var props = _playbackItem.GetDisplayProperties();
            props.Type = Windows.Media.MediaPlaybackType.Video;
            props.Thumbnail = Windows.Storage.Streams.RandomAccessStreamReference.CreateFromUri(new Uri(view.Information.User.Avatar.GetSourceUri() + "@100w_100h_1c_100q.jpg"));
            props.VideoProperties.Title = view.Information.Identifier.Title;
            props.VideoProperties.Subtitle = string.Join(string.Empty, view.Information.Description.Take(20));
            props.VideoProperties.Genres.Add(_videoType.ToString());
            _playbackItem.ApplyDisplayProperties(props);
        }
    }
}
