// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Bili.Models.App.Constants;
using Bili.Models.Enums.App;
using FFmpegInteropX;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Media.Streaming.Adaptive;
using Windows.Web.Http;

namespace Bili.ViewModels.Uwp.Core
{
    /// <summary>
    /// 媒体播放器视图模型.
    /// </summary>
    public sealed partial class MediaPlayerViewModel
    {
        private async Task LoadDashVideoSourceFromFFmpegAsync()
        {
            var hasAudio = _audio != null;
            _videoConfig.VideoDecoderMode = GetDecoderMode();

            var tasks = new List<Task>
                {
                    Task.Run(async () =>
                    {
                        var client = GetVideoClient();
                        _videoStream = await HttpRandomAccessStream.CreateAsync(client, new Uri(_video.BaseUrl));
                        _videoFFSource = await FFmpegMediaSource.CreateFromStreamAsync(_videoStream, _videoConfig);
                    }),
                    Task.Run(async () =>
                    {
                        if (hasAudio)
                        {
                            var client = GetVideoClient();
                            _audioStream = await HttpRandomAccessStream.CreateAsync(client, new Uri(_audio.BaseUrl));
                            _audioFFSource = await FFmpegMediaSource.CreateFromStreamAsync(_audioStream, _videoConfig);
                        }
                    }),
                };

            await Task.WhenAll(tasks);
            _videoPlaybackItem = _videoFFSource.CreateMediaPlaybackItem();

            _videoPlayer = GetVideoPlayer();
            _videoPlayer.Source = _videoPlaybackItem;

            if (hasAudio)
            {
                _audioPlaybackItem = _audioFFSource.CreateMediaPlaybackItem();
                _mediaTimelineController = GetTimelineController();
                _videoPlayer.CommandManager.IsEnabled = false;
                _videoPlayer.TimelineController = _mediaTimelineController;
                _audioPlayer = new MediaPlayer();
                _audioPlayer.CommandManager.IsEnabled = false;
                _audioPlayer.Source = _audioPlaybackItem;
                _audioPlayer.TimelineController = _mediaTimelineController;
            }

            MediaPlayerChanged?.Invoke(this, _videoPlayer);
        }

        private async Task LoadDashVideoSourceFromNativeAsync()
        {
            var httpClient = GetVideoClient();
            var mpdFilePath = _audio == null
                    ? AppConstants.DashVideoWithoudAudioMPDFile
                    : AppConstants.DashVideoMPDFile;
            var mpdStr = await _fileToolkit.ReadPackageFile(mpdFilePath);

            var videoStr =
                    $@"<Representation bandwidth=""{_video.Bandwidth}"" codecs=""{_video.Codecs}"" height=""{_video.Height}"" mimeType=""{_video.MimeType}"" id=""{_video.Id}"" width=""{_video.Width}"">
                               <BaseURL></BaseURL>
                               <SegmentBase indexRange=""{_video.IndexRange}"">
                                   <Initialization range=""{_video.Initialization}"" />
                               </SegmentBase>
                           </Representation>";

            var audioStr = string.Empty;

            if (_audio != null)
            {
                audioStr =
                        $@"<Representation bandwidth=""{_audio.Bandwidth}"" codecs=""{_audio.Codecs}"" mimeType=""{_audio.MimeType}"" id=""{_audio.Id}"">
                               <BaseURL></BaseURL>
                               <SegmentBase indexRange=""{_audio.IndexRange}"">
                                   <Initialization range=""{_audio.Initialization}"" />
                               </SegmentBase>
                           </Representation>";
            }

            videoStr = videoStr.Trim();
            audioStr = audioStr.Trim();
            mpdStr = mpdStr.Replace("{video}", videoStr)
                             .Replace("{audio}", audioStr)
                             .Replace("{bufferTime}", $"PT{_mediaInformation.MinBufferTime}S");

            var stream = new MemoryStream(Encoding.UTF8.GetBytes(mpdStr)).AsInputStream();
            var soure = await AdaptiveMediaSource.CreateFromStreamAsync(stream, new Uri(_video.BaseUrl), "application/dash+xml", httpClient);
            Debug.Assert(soure.Status == AdaptiveMediaSourceCreationStatus.Success, "解析MPD失败");
            soure.MediaSource.DownloadRequested += (sender, args) =>
            {
                if (args.ResourceContentType == "audio/mp4" && _audio != null)
                {
                    args.Result.ResourceUri = new Uri(_audio.BaseUrl);
                }
            };

            var mediaSource = MediaSource.CreateFromAdaptiveMediaSource(soure.MediaSource);
            _videoPlaybackItem = new MediaPlaybackItem(mediaSource);
            _videoPlayer = GetVideoPlayer();
            _videoPlayer.Source = _videoPlaybackItem;
            MediaPlayerChanged?.Invoke(this, _videoPlayer);
        }

        private async Task LoadDashLiveSourceAsync(string url)
        {
            try
            {
                _liveConfig.VideoDecoderMode = GetDecoderMode();
                _videoFFSource?.Dispose();
                _videoFFSource = await FFmpegMediaSource.CreateFromUriAsync(url, _liveConfig);
                _videoPlaybackItem = _videoFFSource.CreateMediaPlaybackItem();

                _videoPlayer = GetVideoPlayer();
                _videoPlayer.Source = _videoPlaybackItem;

                MediaPlayerChanged?.Invoke(this, _videoPlayer);
            }
            catch (Exception ex)
            {
                IsError = true;
                ErrorText = _resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.RequestLivePlayInformationFailed);
                LogException(ex);
            }
        }

        private VideoDecoderMode GetDecoderMode()
        {
            var decodeType = _settingsToolkit.ReadLocalSetting(Models.Enums.SettingNames.DecodeType, DecodeType.Automatic);
            return decodeType switch
            {
                DecodeType.HardwareDecode => VideoDecoderMode.ForceSystemDecoder,
                DecodeType.SoftwareDecode => VideoDecoderMode.ForceFFmpegSoftwareDecoder,
                _ => VideoDecoderMode.Automatic
            };
        }

        private HttpClient GetVideoClient()
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Referer = new Uri("https://www.bilibili.com");
            httpClient.DefaultRequestHeaders.Add("User-Agent", ServiceConstants.DefaultUserAgentString);
            return httpClient;
        }
    }
}
