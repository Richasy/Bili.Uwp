// Copyright (c) Richasy. All rights reserved.

using System;
using System.Linq;
using System.Threading.Tasks;
using Bili.Models.Data.Pgc;
using Bili.Models.Data.Video;
using Bili.Models.Enums;

namespace Bili.ViewModels.Uwp.Core
{
    /// <summary>
    /// 媒体播放器视图模型.
    /// </summary>
    public sealed partial class MediaPlayerViewModel
    {
        private async Task ChangeEpisodeAsync(VideoIdentifier identifier)
        {
            var view = _viewData as PgcPlayerView;
            Cover = view.Information.Identifier.Cover.GetSourceUri().ToString();
            if (string.IsNullOrEmpty(identifier.Id))
            {
                return;
            }

            if (view.Episodes != null && view.Episodes.Any(p => p.Identifier.Id == identifier.Id))
            {
                _currentEpisode = view.Episodes.First(p => p.Identifier.Id == identifier.Id);
            }
            else if (view.Extras != null)
            {
                var episodes = view.Extras.SelectMany(p => p.Value);
                _currentEpisode = episodes.FirstOrDefault(p => p.Identifier.Id == identifier.Id);
            }

            if (_currentEpisode == null)
            {
                IsError = true;
                ErrorText = view.Warning ?? _resourceToolkit.GetLocaleString(LanguageNames.RequestPgcFailed);
            }

            await LoadEpisodeAsync();
        }

        private async Task LoadEpisodeAsync()
        {
            var view = _viewData as PgcPlayerView;
            if (_currentEpisode == null)
            {
                IsError = true;
                ErrorText = view.Warning ?? _resourceToolkit.GetLocaleString(LanguageNames.RequestPgcFailed);
                return;
            }

            SubtitleViewModel.SetData(_currentEpisode.VideoId, _currentEpisode.PartId);
            DanmakuViewModel.SetData(_currentEpisode.VideoId, _currentEpisode.PartId, _videoType);
            await InitializeEpisodeMediaInformationAsync();
            await InitializeOrginalVideoSourceAsync();
            FillEpisodePlaybackProperties();
            CheckEpisodeHistory();
        }

        private void CheckEpisodeHistory()
        {
            var view = _viewData as PgcPlayerView;
            if (view.Progress != null && view.Progress.Status == Models.Enums.Player.PlayedProgressStatus.Playing)
            {
                var history = view.Progress.Identifier;
                var ts = TimeSpan.FromSeconds(view.Progress.Progress);
                IsShowProgressTip = true;
                ProgressTip = $"{_resourceToolkit.GetLocaleString(LanguageNames.PreviousView)}{history.Title} {ts}";
            }
        }

        private async Task InitializeEpisodeMediaInformationAsync()
        {
            var view = _viewData as PgcPlayerView;
            var proxy = _appToolkit.GetProxyAndArea(view.Information.Identifier.Title, true);
            _mediaInformation = await _playerProvider.GetPgcMediaInformationAsync(
                _currentEpisode.PartId,
                _currentEpisode.Identifier.Id,
                _currentEpisode.SeasonType,
                proxy.Item1,
                proxy.Item2);
        }

        private void FillEpisodePlaybackProperties()
        {
            var props = _playbackItem.GetDisplayProperties();
            props.Type = Windows.Media.MediaPlaybackType.Video;
            props.Thumbnail = Windows.Storage.Streams.RandomAccessStreamReference.CreateFromUri(new Uri(_currentEpisode.Identifier.Cover.GetSourceUri().ToString() + "@100w_100h_1c_100q.jpg"));
            props.VideoProperties.Title = _currentEpisode.Identifier.Title;
            props.VideoProperties.Genres.Add(_videoType.ToString());
            _playbackItem.ApplyDisplayProperties(props);
        }
    }
}
