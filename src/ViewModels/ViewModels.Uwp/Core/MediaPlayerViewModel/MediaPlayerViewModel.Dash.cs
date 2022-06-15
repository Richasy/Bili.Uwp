// Copyright (c) Richasy. All rights reserved.

using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Bili.Models.App.Constants;
using FFmpegInterop;
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
        private async Task<MediaSource> GetDashVideoSourceAsync()
        {
            try
            {
                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Referer = new Uri("https://www.bilibili.com");
                httpClient.DefaultRequestHeaders.Add("User-Agent", ServiceConstants.DefaultUserAgentString);

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

                return MediaSource.CreateFromAdaptiveMediaSource(soure.MediaSource);
            }
            catch (Exception)
            {
                IsError = true;
                ErrorText = _resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.RequestVideoFailed);
            }

            return null;
        }

        private async Task<MediaPlaybackItem> GetDashLiveSourceAsync(string url)
        {
            try
            {
                if (_interopMSS != null)
                {
                    _interopMSS.Dispose();
                    _interopMSS = null;
                }

                _interopMSS = await FFmpegInteropMSS.CreateFromUriAsync(url, _liveConfig);
            }
            catch (Exception)
            {
                IsError = true;
                ErrorText = _resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.RequestLivePlayInformationFailed);
                return default;
            }

            var source = _interopMSS.CreateMediaPlaybackItem();
            return source;
        }
    }
}
