// Copyright (c) Richasy. All rights reserved.

using System;
using System.Linq;
using System.Threading.Tasks;
using Richasy.Bili.Models.App.Constants;
using Richasy.Bili.Models.BiliBili;
using Richasy.Bili.Models.Enums;

namespace Richasy.Bili.ViewModels.Uwp
{
    /// <summary>
    /// 播放器视图模型.
    /// </summary>
    public partial class PlayerViewModel
    {
        private void Reset()
        {
            IsDetailError = false;
            _dashInformation = null;
            CurrentFormat = null;
            PartCollection.Clear();
            RelatedVideoCollection.Clear();
            FormatCollection.Clear();
            _audioList.Clear();
            _videoList.Clear();
            ClearPlayer();
        }

        private async Task LoadVideoDetailAsync(string videoId)
        {
            if (_videoDetail == null || videoId != AvId)
            {
                Reset();
                IsDetailLoading = true;
                _videoId = Convert.ToInt64(videoId);
                try
                {
                    var detail = await Controller.GetVideoDetailAsync(_videoId);
                    _videoDetail = detail;
                }
                catch (Exception ex)
                {
                    IsDetailError = true;
                    DetailErrorText = _resourceToolkit.GetLocaleString(LanguageNames.RequestVideoFailed) + $"\n{ex.Message}";
                    IsDetailLoading = false;
                    return;
                }

                InitializeVideoDetail();
                IsDetailLoading = false;
            }

            var partId = CurrentPart == null ? 0 : CurrentPart.Page.Cid;
            await ChangePartAsync(partId);
        }

        private void InitializeVideoDetail(long partId = 0)
        {
            if (_videoDetail == null)
            {
                return;
            }

            Title = _videoDetail.Arc.Title;
            Subtitle = DateTimeOffset.FromUnixTimeSeconds(_videoDetail.Arc.Pubdate).ToString("yy/MM/dd HH:mm");
            Description = _videoDetail.Arc.Desc;
            Publisher = new PublisherViewModel(_videoDetail.Arc.Author);
            AvId = _videoDetail.Arc.Aid.ToString();
            BvId = _videoDetail.Bvid;
            PlayCount = _numberToolkit.GetCountText(_videoDetail.Arc.Stat.View);
            DanmakuCount = _numberToolkit.GetCountText(_videoDetail.Arc.Stat.Danmaku);
            LikeCount = _numberToolkit.GetCountText(_videoDetail.Arc.Stat.Like);
            CoinCount = _numberToolkit.GetCountText(_videoDetail.Arc.Stat.Coin);
            FavoriteCount = _numberToolkit.GetCountText(_videoDetail.Arc.Stat.Fav);
            ShareCount = _numberToolkit.GetCountText(_videoDetail.Arc.Stat.Share);
            ReplyCount = _numberToolkit.GetCountText(_videoDetail.Arc.Stat.Reply);
            CoverUrl = _videoDetail.Arc.Pic;

            IsLikeChecked = _videoDetail.ReqUser.Like == 1;
            IsCoinChecked = _videoDetail.ReqUser.Coin == 1;
            IsFavoriteChecked = _videoDetail.ReqUser.Favorite == 1;

            foreach (var page in _videoDetail.Pages)
            {
                PartCollection.Add(new VideoPartViewModel(page));
            }

            IsShowParts = PartCollection.Count > 1;

            var relates = _videoDetail.Relates.Where(p => p.Goto.Equals(ServiceConstants.Pgc, StringComparison.OrdinalIgnoreCase) || p.Goto.Equals(ServiceConstants.Av, StringComparison.OrdinalIgnoreCase));
            foreach (var video in relates)
            {
                RelatedVideoCollection.Add(new VideoViewModel(video));
            }
        }

        private async Task InitializeVideoPlayInformationAsync(PlayerDashInformation videoPlayView)
        {
            _audioList = videoPlayView.VideoInformation.Audio.ToList();
            _videoList = videoPlayView.VideoInformation.Video.ToList();

            _currentAudio = null;
            _currentVideo = null;

            FormatCollection.Clear();
            videoPlayView.SupportFormats.ForEach(p => FormatCollection.Add(new VideoFormatViewModel(p, false)));

            var formatId = CurrentFormat == null ?
                _settingsToolkit.ReadLocalSetting(Models.Enums.SettingNames.DefaultVideoFormat, 64) :
                CurrentFormat.Quality;

            await ChangeFormatAsync(formatId);
        }

        private void InitializeTimer()
        {
            if (_progressTimer == null)
            {
                _progressTimer = new Windows.UI.Xaml.DispatcherTimer();
                _progressTimer.Interval = TimeSpan.FromSeconds(5);
                _progressTimer.Tick += OnProgressTimerTickAsync;
            }
        }

        private int GetPreferCodecId()
        {
            var id = 7;
            switch (PreferCodec)
            {
                case Models.Enums.PreferCodec.H265:
                    id = 12;
                    break;
                default:
                    break;
            }

            return id;
        }

        private void CheckPartSelection()
        {
            foreach (var item in PartCollection)
            {
                item.IsSelected = item.Data.Equals(CurrentPart);
            }
        }

        private void CheckFormatSelection()
        {
            foreach (var item in FormatCollection)
            {
                item.IsSelected = item.Data.Equals(CurrentFormat);
            }
        }

        private async void OnProgressTimerTickAsync(object sender, object e)
        {
            if (_videoDetail == null || CurrentPart == null)
            {
                return;
            }

            if (_currentVideoPlayer == null || _currentVideoPlayer.PlaybackSession == null)
            {
                return;
            }

            var progress = _currentVideoPlayer.PlaybackSession.Position;
            if (progress != _lastReportProgress)
            {
                await Controller.ReportHistoryAsync(_videoId, CurrentPart.Page.Cid, _currentVideoPlayer.PlaybackSession.Position);
                _lastReportProgress = progress;
            }
        }
    }
}
