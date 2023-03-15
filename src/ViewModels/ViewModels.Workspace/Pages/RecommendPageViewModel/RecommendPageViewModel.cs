// Copyright (c) Richasy. All rights reserved.

using System.Linq;
using System.Threading.Tasks;
using Bili.DI.Container;
using Bili.Lib.Interfaces;
using Bili.Models.Data.Pgc;
using Bili.Models.Data.Video;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces;
using Bili.ViewModels.Interfaces.Home;
using Bili.ViewModels.Interfaces.Pgc;
using Bili.ViewModels.Interfaces.Video;
using Bili.ViewModels.Workspace.Core;

namespace Bili.ViewModels.Workspace.Pages
{
    /// <summary>
    /// 视频推荐视图模型.
    /// </summary>
    public partial class RecommendPageViewModel : InformationFlowViewModelBase<IVideoBaseViewModel>, IRecommendPageViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RecommendPageViewModel"/> class.
        /// </summary>
        public RecommendPageViewModel(
            IResourceToolkit resourceToolkit,
            IHomeProvider recommendProvider)
        {
            _resourceToolkit = resourceToolkit;
            _homeProvider = recommendProvider;
        }

        /// <inheritdoc/>
        protected override void BeforeReload()
            => _homeProvider.ResetRecommendState();

        /// <inheritdoc/>
        protected override async Task GetDataAsync()
        {
            var videos = await _homeProvider.RequestRecommendVideosAsync();
            if (videos?.Any() ?? false)
            {
                foreach (var item in videos)
                {
                    IVideoBaseViewModel vm = null;
                    if (item is VideoInformation videoInfo)
                    {
                        var videoVM = Locator.Instance.GetService<IVideoItemViewModel>();
                        videoInfo.Publisher = default;
                        videoVM.InjectData(videoInfo);
                        vm = videoVM;
                    }
                    else if (item is EpisodeInformation episodeInfo)
                    {
                        var episodeVM = Locator.Instance.GetService<IEpisodeItemViewModel>();
                        episodeVM.InjectData(episodeInfo);
                        vm = episodeVM;
                    }

                    if (vm != null)
                    {
                        Items.Add(vm);
                    }
                }
            }
        }

        /// <inheritdoc/>
        protected override string FormatException(string errorMsg)
            => $"{_resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.RequestRecommendFailed)}\n{errorMsg}";
    }
}
