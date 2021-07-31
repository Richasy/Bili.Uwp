// Copyright (c) Richasy. All rights reserved.

using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bilibili.App.Playurl.V1;
using Richasy.Bili.Models.App.Constants;
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
                                  <Period  start=""PT0S"">
                                    <AdaptationSet>
                                      <ContentComponent contentType=""video"" id=""1"" />
                                      <Representation bandwidth=""{_currentVideo.DashVideo.Bandwidth}"" codecs=""{_currentVideo.DashVideo.Codecid}"" height=""{_detail.Arc.Dimension.Height}"" mimeType=""video/mp4"" id=""{_currentVideo.StreamInfo.Quality}"" width=""{_detail.Arc.Dimension.Width}"">
                                        <BaseURL>{_currentVideo.DashVideo.BaseUrl}</BaseURL>
                                        <SegmentBase indexRange=""0"">
                                            <Initialization range=""0"" />
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
                                      <Representation bandwidth=""{_currentAudio.Bandwidth}"" codecs=""{_currentAudio.Codecid}"" id=""{_currentAudio.Id}"" mimeType=""audio/mp4"" >
                                        <BaseURL>{_currentAudio.BaseUrl}</BaseURL>
                                        <SegmentBase indexRange=""0"">
                                            <Initialization range=""0"" />
                                        </SegmentBase>
                                      </Representation>
                                    </AdaptationSet>";
                    mpdStr = mpdStr.Replace("{audio}", audioStr);
                }

                var stream = new MemoryStream(Encoding.UTF8.GetBytes(mpdStr)).AsInputStream();
                var soure = await AdaptiveMediaSource.CreateFromStreamAsync(stream, new Uri(_currentVideo.DashVideo.BaseUrl), "application/dash+xml", httpClient);
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

        private async Task InitializeVideoPlayInformationAsync(PlayViewReply videoPlayView)
        {
            _audioList = videoPlayView.VideoInfo.DashAudio.ToList();
            _streamList = videoPlayView.VideoInfo.StreamList.ToList();

            _currentAudio = null;
            _currentVideo = null;

            await Task.CompletedTask;

            var selectedStream = _streamList.Where(p => p.StreamInfo.Quality == CurrentQuality).FirstOrDefault();
            if (selectedStream == null)
            {
                var maxQuality = _streamList.Max(p => p.StreamInfo.Quality);
                selectedStream = _streamList.Where(p => p.StreamInfo.Quality == maxQuality).FirstOrDefault();
            }

            _currentVideo = selectedStream;

            var selectedAudio = _audioList.Where(p => p.Id == _currentVideo.DashVideo.AudioId).FirstOrDefault();
            if (selectedAudio == null)
            {
                selectedAudio = _audioList.Last();
            }

            _currentAudio = selectedAudio;

            await CreateMediaSourceAsync();
        }
    }
}
