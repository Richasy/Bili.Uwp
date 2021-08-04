// Copyright (c) Richasy. All rights reserved.

using System;
using System.Linq;
using System.Threading.Tasks;
using Richasy.Bili.Models.App.Constants;
using Richasy.Bili.Models.BiliBili;

namespace Richasy.Bili.ViewModels.Uwp
{
    /// <summary>
    /// 播放器视图模型.
    /// </summary>
    public partial class PlayerViewModel
    {
        private void InitializeVideoDetail(long partId = 0)
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
            CoverUrl = _detail.Arc.Pic;

            foreach (var page in _detail.Pages)
            {
                PartCollection.Add(new VideoPartViewModel(page));
            }

            IsShowParts = PartCollection.Count > 1;

            var relates = _detail.Relates.Where(p => p.Goto.Equals(ServiceConstants.Pgc, StringComparison.OrdinalIgnoreCase) || p.Goto.Equals(ServiceConstants.Av, StringComparison.OrdinalIgnoreCase));
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

            var preferCodecId = GetPreferCodecId();
            var qualityId = CurrentQuality == null ?
                _settingsToolkit.ReadLocalSetting(Models.Enums.SettingNames.DefaultVideoQuality, 64) :
                CurrentQuality.Quality;

            var conditionStreams = _videoList.Where(p => p.Id == qualityId).ToList();
            if (conditionStreams.Count == 0)
            {
                var maxQuality = _videoList.Max(p => p.Id);
                _currentVideo = _videoList.Where(p => p.Id == maxQuality).FirstOrDefault();
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

            await InitializeOnlineDashVideoAsync();
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
    }
}
