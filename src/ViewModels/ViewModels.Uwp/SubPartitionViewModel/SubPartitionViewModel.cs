// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Richasy.Bili.Locator.Uwp;
using Richasy.Bili.Models.BiliBili;

namespace Richasy.Bili.ViewModels.Uwp
{
    /// <summary>
    /// 子分区视图模型.
    /// </summary>
    public partial class SubPartitionViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SubPartitionViewModel"/> class.
        /// </summary>
        /// <param name="partition">子分区数据.</param>
        public SubPartitionViewModel(Partition partition)
        {
            ServiceLocator.Instance.LoadService(out _resourceToolkit);
            BannerCollection = new ObservableCollection<Banner>();
            VideoCollection = new ObservableCollection<VideoViewModel>();
            TagCollection = new ObservableCollection<Tag>();
            this._partition = partition;
            if (this._partition != null)
            {
                this.Title = this._partition.Name;
                this._isRecommendPartition = false;
            }
            else
            {
                this.Title = this._resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.Recommend);
                this._isRecommendPartition = true;
            }
        }

        /// <summary>
        /// 请求数据.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        public async Task RequestDataAsync()
        {
            if (IsRequested)
            {
                await DeltaRequestAsync();
            }
            else
            {
                await InitializeRequestAsync();
            }
        }

        /// <summary>
        /// 执行初始请求.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        internal async Task InitializeRequestAsync()
        {
            IsInitializeLoading = true;
            VideoCollection.Clear();
            BannerCollection.Clear();
            TagCollection.Clear();
            var videos = new List<Video>();
            SubPartition source = null;
            if (_isRecommendPartition)
            {
                var data = await LoadMockDataAsync<ServerResponse<SubPartitionRecommend>>("RecommendSubpartitionFirstRequest");
                source = data.Data;
                if (data.Data.Banner != null && data.Data.Banner.TopBanners != null)
                {
                    data.Data.Banner.TopBanners.ForEach(p => BannerCollection.Add(p));
                }
            }
            else
            {
                var data = await LoadMockDataAsync<ServerResponse<SubPartitionDefault>>("SubpartitionFirstRequest");
                source = data.Data;
                TagCollection.Clear();
                if (data.Data.TopTags != null && data.Data.TopTags.Count > 0)
                {
                    data.Data.TopTags.ForEach(p => TagCollection.Add(p));
                }
            }

            if (source != null)
            {
                if (source.NewVideos != null && source.RecommendVideos.Count > 0)
                {
                    videos = videos.Concat(source.NewVideos).ToList();
                }

                if (source.RecommendVideos != null && source.RecommendVideos.Count > 0)
                {
                    videos = videos.Concat(source.RecommendVideos).ToList();
                }
            }

            if (videos.Count > 0)
            {
                videos.ForEach(p => VideoCollection.Add(new VideoViewModel(p)));
            }

            IsInitializeLoading = false;
            IsRequested = true;
        }

        /// <summary>
        /// 执行增量请求.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        internal Task DeltaRequestAsync()
        {
            return Task.CompletedTask;
        }
    }
}
