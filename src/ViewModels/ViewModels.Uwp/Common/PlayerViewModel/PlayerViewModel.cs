// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Bilibili.App.View.V1;
using Richasy.Bili.Locator.Uwp;
using Richasy.Bili.Models.App.Constants;
using Richasy.Bili.Models.Enums;

namespace Richasy.Bili.ViewModels.Uwp
{
    /// <summary>
    /// 播放器视图模型.
    /// </summary>
    public partial class PlayerViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerViewModel"/> class.
        /// </summary>
        public PlayerViewModel()
        {
            RelatedVideoCollection = new ObservableCollection<VideoViewModel>();
            PartCollection = new ObservableCollection<ViewPage>();
            ServiceLocator.Instance.LoadService(out _numberToolkit)
                                   .LoadService(out _resourceToolkit);
        }

        /// <summary>
        /// 视频加载.
        /// </summary>
        /// <param name="vm">视频视图模型.</param>
        /// <returns><see cref="Task"/>.</returns>
        public async Task LoadAsync(VideoViewModel vm)
        {
            if (_detail == null || vm.VideoId != AvId)
            {
                IsLoading = true;
                PartCollection.Clear();
                RelatedVideoCollection.Clear();
                Title = vm.Title;
                try
                {
                    var detail = await Controller.GetVideoDetailAsync(Convert.ToInt32(vm.VideoId));
                    _detail = detail;
                }
                catch (Exception ex)
                {
                    IsError = true;
                    ErrorText = _resourceToolkit.GetLocaleString(LanguageNames.RequestVideoFailed) + $"\n{ex.Message}";
                    IsLoading = false;
                    return;
                }

                Title = _detail.Arc.Title;
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

                var relates = _detail.Relates.Where(p => p.Goto.Equals(ServiceConstants.Pgc, StringComparison.OrdinalIgnoreCase) || p.Goto.Equals(ServiceConstants.Av, StringComparison.OrdinalIgnoreCase));
                foreach (var video in relates)
                {
                    RelatedVideoCollection.Add(new VideoViewModel(video));
                }

                IsLoading = false;
            }
        }
    }
}
