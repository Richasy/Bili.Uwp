// Copyright (c) Richasy. All rights reserved.

using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Richasy.Bili.Models.App.Constants;
using Richasy.Bili.Models.BiliBili;
using Windows.Media.Core;
using Windows.Media.Streaming.Adaptive;
using Windows.Web.Http;

namespace Richasy.Bili.ViewModels.Uwp
{
    /// <summary>
    /// 播放器视图模型.
    /// </summary>
    public partial class PlayerViewModel
    {
        private async Task CreateMediaSourceAsync()
        {
            MediaSource result = null;

            try
            {
                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Referer = new Uri("https://www.bilibili.com");
                httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/69.0.3497.100 Safari/537.36");
                var mpdStr = $@"<MPD xmlns=""urn:mpeg:DASH:schema:MPD:2011""  profiles=""urn:mpeg:dash:profile:isoff-on-demand:2011"" type=""static"">
                                  <Period start=""PT0S"">
                                    <AdaptationSet>
                                      <ContentComponent contentType=""video"" id=""1"" />
                                      <Representation bandwidth=""{_currentVideo.BandWidth}"" codecs=""{_currentVideo.Codecs}"" height=""{_currentVideo.Height}"" mimeType=""{_currentVideo.MimeType}"" id=""{_currentVideo.Id}"" width=""{_currentVideo.Width}"">
                                        <BaseURL></BaseURL>
                                        <SegmentBase indexRange=""{_currentVideo.SegmentBase.IndexRange}"">
                                            <Initialization range=""{_currentVideo.SegmentBase.Initialization}"" />
                                        </SegmentBase>
                                      </Representation>
                                    </AdaptationSet>
                                    {{audio}}
                                  </Period>
                                </MPD>
                                ";
                if (_currentAudio == null)
                {
                    mpdStr = mpdStr.Replace("{audio}", string.Empty);
                }
                else
                {
                    var audioStr = $@"<AdaptationSet>
                                      <ContentComponent contentType=""audio"" id=""2"" />
                                      <Representation bandwidth=""{_currentAudio.BandWidth}"" codecs=""{_currentAudio.Codecs}"" id=""{_currentAudio.Id}"" mimeType=""{_currentAudio.MimeType}"">
                                        <BaseURL></BaseURL>
                                        <SegmentBase indexRange=""{_currentAudio.SegmentBase.IndexRange}"">
                                            <Initialization range=""{_currentAudio.SegmentBase.Initialization}"" />
                                        </SegmentBase>
                                      </Representation>
                                    </AdaptationSet>";
                    mpdStr = mpdStr.Replace("{audio}", audioStr);
                }

                var stream = new MemoryStream(Encoding.UTF8.GetBytes(mpdStr)).AsInputStream();
                var soure = await AdaptiveMediaSource.CreateFromStreamAsync(stream, new Uri(_currentVideo.BaseUrl), "application/dash+xml", httpClient);
                var s = soure.Status;
                soure.MediaSource.DownloadRequested += (sender, args) =>
                {
                    if (args.ResourceContentType == "audio/mp4" && _currentAudio != null)
                    {
                        args.Result.ResourceUri = new Uri(_currentAudio.BaseUrl);
                    }
                };

                result = MediaSource.CreateFromAdaptiveMediaSource(soure.MediaSource);
            }
            catch (Exception)
            {
                // Show error.
            }

            MediaSourceUpdated?.Invoke(this, result);
        }

        private void InitializeVideoDetail()
        {
            if (_detail == null)
            {
                return;
            }

            Title = _detail.Arc.Title;
            Subtitle = DateTimeOffset.FromUnixTimeSeconds(_detail.Arc.Pubdate).ToString("yy/MM/dd HH:mm");
            Description = _detail.Arc.Desc;
            Publisher = new PublisherViewModel(_detail.Arc.Author);
            AvId = _detail.Arc.Aid.ToString();
            BvId = _detail.Bvid;
            PlayCount = _numberToolkit.GetCountText(_detail.Arc.Stat.View);
            DanmakuCount = _numberToolkit.GetCountText(_detail.Arc.Stat.Danmaku);
            LikeCount = _numberToolkit.GetCountText(_detail.Arc.Stat.Like);
            CoinCount = _numberToolkit.GetCountText(_detail.Arc.Stat.Coin);
            FavoriteCount = _numberToolkit.GetCountText(_detail.Arc.Stat.Fav);
            ShareCount = _numberToolkit.GetCountText(_detail.Arc.Stat.Share);
            ReplyCount = _numberToolkit.GetCountText(_detail.Arc.Stat.Reply);

            foreach (var page in _detail.Pages)
            {
                PartCollection.Add(page);
            }

            CurrentPart = PartCollection.First();

            var relates = _detail.Relates.Where(p => p.Goto.Equals(ServiceConstants.Pgc, StringComparison.OrdinalIgnoreCase) || p.Goto.Equals(ServiceConstants.Av, StringComparison.OrdinalIgnoreCase));
            foreach (var video in relates)
            {
                RelatedVideoCollection.Add(new VideoViewModel(video));
            }
        }

        private async Task InitializeVideoPlayInformationAsync(PlayerDashInformation videoPlayView)
        {
            _audioList = videoPlayView.VideoInformation.Audio.ToList();
            _streamList = videoPlayView.VideoInformation.Video.ToList();

            _currentAudio = null;
            _currentVideo = null;

            var preferCodecId = GetPreferCodecId();
            var conditionStreams = _streamList.Where(p => p.Id == CurrentQuality).ToList();
            if (conditionStreams.Count == 0)
            {
                var maxQuality = _streamList.Max(p => p.Id);
                _currentVideo = _streamList.Where(p => p.Id == maxQuality).FirstOrDefault();
            }
            else
            {
                var tempVideo = conditionStreams.Where(p => p.CodecId == preferCodecId).FirstOrDefault();
                if (tempVideo == null)
                {
                    tempVideo = conditionStreams.First();
                }

                _currentVideo = tempVideo;
            }

            _currentAudio = _audioList.FirstOrDefault();

            await CreateMediaSourceAsync();
        }

        private int GetPreferCodecId()
        {
            var id = 7;
            switch (PreferCodec)
            {
                case Models.Enums.PreferCodec.H265:
                    id = 12;
                    break;
                case Models.Enums.PreferCodec.H264:
                case Models.Enums.PreferCodec.Flv:
                default:
                    break;
            }

            return id;
        }
    }
}
