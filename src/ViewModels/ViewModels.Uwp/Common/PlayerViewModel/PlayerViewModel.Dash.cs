// Copyright (c) Richasy. All rights reserved.

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FFmpegInterop;
using Richasy.Bili.Models.App.Constants;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Media.Streaming.Adaptive;
using Windows.Web.Http;

namespace Richasy.Bili.ViewModels.Uwp
{
    /// <summary>
    /// 播放器视图模型.
    /// </summary>
    public partial class PlayerViewModel
    {
        private async Task InitializeOnlineDashVideoAsync()
        {
            try
            {
                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Referer = new Uri("https://www.bilibili.com");
                httpClient.DefaultRequestHeaders.Add("User-Agent", ServiceConstants.DefaultUserAgentString);
                var mpdStr = await _fileToolkit.ReadPackageFile(AppConstants.DashVideoMPDFile);

                var videos = _videoList.Where(p => p.CodecId == GetPreferCodecId()).ToList();
                if (videos.Count == 0)
                {
                    videos = _videoList;
                }

                var videoStr =
                        $@"<Representation bandwidth=""{_currentVideo.BandWidth}"" codecs=""{_currentVideo.Codecs}"" height=""{_currentVideo.Height}"" mimeType=""{_currentVideo.MimeType}"" id=""{_currentVideo.Id}"" width=""{_currentVideo.Width}"">
                               <BaseURL></BaseURL>
                               <SegmentBase indexRange=""{_currentVideo.SegmentBase.IndexRange}"">
                                   <Initialization range=""{_currentVideo.SegmentBase.Initialization}"" />
                               </SegmentBase>
                           </Representation>";

                var audioStr = string.Empty;

                if (_currentAudio != null)
                {
                    audioStr =
                        $@"<Representation bandwidth=""{_currentAudio.BandWidth}"" codecs=""{_currentAudio.Codecs}"" mimeType=""{_currentAudio.MimeType}"" id=""{_currentAudio.Id}"">
                               <BaseURL></BaseURL>
                               <SegmentBase indexRange=""{_currentAudio.SegmentBase.IndexRange}"">
                                   <Initialization range=""{_currentAudio.SegmentBase.Initialization}"" />
                               </SegmentBase>
                           </Representation>";
                }

                videoStr = videoStr.Trim();
                audioStr = audioStr.Trim();

                mpdStr = mpdStr.Replace("{video}", videoStr)
                             .Replace("{audio}", audioStr)
                             .Replace("{bufferTime}", $"PT{_dashInformation.VideoInformation.MinBufferTime}S");

                var stream = new MemoryStream(Encoding.UTF8.GetBytes(mpdStr)).AsInputStream();
                var soure = await AdaptiveMediaSource.CreateFromStreamAsync(stream, new Uri(_currentVideo.BaseUrl), "application/dash+xml", httpClient);
                Debug.Assert(soure.Status == AdaptiveMediaSourceCreationStatus.Success, "解析MPD失败");
                soure.MediaSource.DownloadRequested += (sender, args) =>
                {
                    if (args.ResourceContentType == "audio/mp4" && _currentAudio != null)
                    {
                        args.Result.ResourceUri = new Uri(_currentAudio.BaseUrl);
                    }
                };

                if (_currentVideoPlayer == null)
                {
                    _currentVideoPlayer = InitializeMediaPlayer();
                }

                var position = TimeSpan.Zero;
                if (_currentVideoPlayer.PlaybackSession != null)
                {
                    position = _currentVideoPlayer.PlaybackSession.Position;
                }
                else if (_initializeProgress != TimeSpan.Zero)
                {
                    position = _initializeProgress;
                }

                var mediaSource = MediaSource.CreateFromAdaptiveMediaSource(soure.MediaSource);
                var playbackItem = new MediaPlaybackItem(mediaSource);
                _currentVideoPlayer.Source = playbackItem;

                var props = playbackItem.GetDisplayProperties();
                props.Type = Windows.Media.MediaPlaybackType.Video;
                props.Thumbnail = Windows.Storage.Streams.RandomAccessStreamReference.CreateFromUri(new Uri(CoverUrl + "@100w_100h_1c_100q.jpg"));
                props.VideoProperties.Title = Title;
                props.VideoProperties.Subtitle = IsPgc ? Subtitle : Description;
                props.VideoProperties.Genres.Add(_videoType.ToString());
                playbackItem.ApplyDisplayProperties(props);

                BiliPlayer.SetMediaPlayer(_currentVideoPlayer);
                MediaPlayerUpdated?.Invoke(this, EventArgs.Empty);
                _currentVideoPlayer.PlaybackSession.Position = position;
            }
            catch (Exception)
            {
                // Show error.
            }
        }

        private async Task InitializeLiveDashAsync(string url)
        {
            _interopMSS = await FFmpegInteropMSS.CreateFromUriAsync(url, _liveFFConfig);
            var playbackItem = _interopMSS.CreateMediaPlaybackItem();

            var props = playbackItem.GetDisplayProperties();
            props.Type = Windows.Media.MediaPlaybackType.Video;
            props.Thumbnail = Windows.Storage.Streams.RandomAccessStreamReference.CreateFromUri(new Uri(CoverUrl + "@100w_100h_1c_100q.jpg"));
            props.VideoProperties.Title = Title;
            props.VideoProperties.Subtitle = string.IsNullOrEmpty(Subtitle) ? Subtitle : Description;
            props.VideoProperties.Genres.Add(_videoType.ToString());
            playbackItem.ApplyDisplayProperties(props);
            if (_currentVideoPlayer == null)
            {
                _currentVideoPlayer = InitializeMediaPlayer();
            }

            _currentVideoPlayer.Source = playbackItem;
            BiliPlayer.SetMediaPlayer(_currentVideoPlayer);
            MediaPlayerUpdated?.Invoke(this, EventArgs.Empty);
        }
    }
}
