// Copyright (c) Richasy. All rights reserved.

using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Bili.Lib.Interfaces;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Uwp.Base;
using Bili.ViewModels.Uwp.Video;
using Splat;

namespace Bili.ViewModels.Uwp
{
    /// <summary>
    /// 热门视图模型.
    /// </summary>
    public partial class PopularPageViewModel : InformationFlowViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PopularPageViewModel"/> class.
        /// </summary>
        internal PopularPageViewModel(
            IResourceToolkit resourceToolkit,
            IPopularProvider popularProvider)
            : base(resourceToolkit)
            => _popularProvider = popularProvider;

        /// <inheritdoc/>
        protected override void BeforeReload()
            => _popularProvider.Reset();

        /// <inheritdoc/>
        protected override async Task GetDataAsync()
        {
            var videos = await _popularProvider.RequestPopularVideosAsync();
            if (videos?.Any() ?? false)
            {
                foreach (var item in videos)
                {
                    var vm = Splat.Locator.Current.GetService<VideoItemViewModel>();
                    vm.SetInformation(item);
                    VideoCollection.Add(vm);
                }
            }
        }
    }
}
