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
using Windows.UI.Core;

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
            IHomeProvider recommendProvider,
            CoreDispatcher coreDispatcher)
            : base(coreDispatcher)
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

        /// <inheritdoc/>
        protected override string FormatException(string errorMsg)
            => $"{_resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.RequestRecommendFailed)}\n{errorMsg}";
    }
}
