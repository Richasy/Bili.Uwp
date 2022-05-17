// Copyright (c) Richasy. All rights reserved.

using System.Linq;
using System.Threading.Tasks;
using Bili.Lib.Interfaces;
using Bili.Models.Data.Pgc;
using Bili.Models.Data.Video;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces;
using Bili.ViewModels.Uwp.Base;
using Bili.ViewModels.Uwp.Pgc;
using Bili.ViewModels.Uwp.Video;
using Splat;

namespace Bili.ViewModels.Uwp
{
    /// <summary>
    /// 视频推荐视图模型.
    /// </summary>
    public partial class RecommendPageViewModel : InformationFlowViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RecommendPageViewModel"/> class.
        /// </summary>
        internal RecommendPageViewModel(
            IResourceToolkit resourceToolkit,
            IRecommendProvider recommendProvider)
            : base(resourceToolkit)
            => _recommendProvider = recommendProvider;

        /// <inheritdoc/>
        protected override void BeforeReload()
            => _recommendProvider.Reset();

        /// <inheritdoc/>
        protected override async Task GetDataAsync()
        {
            var videos = await _recommendProvider.RequestRecommendVideosAsync();
            if (videos?.Any() ?? false)
            {
                foreach (var item in videos)
                {
                    IVideoBaseViewModel vm = null;
                    if (item is VideoInformation)
                    {
                        vm = Splat.Locator.Current.GetService<VideoItemViewModel>();
                    }
                    else if (item is EpisodeInformation)
                    {
                        vm = Splat.Locator.Current.GetService<EpisodeItemViewModel>();
                    }

                    if (vm != null)
                    {
                        vm.SetInformation(item);
                        VideoCollection.Add(vm);
                    }
                }
            }
        }
    }
}
