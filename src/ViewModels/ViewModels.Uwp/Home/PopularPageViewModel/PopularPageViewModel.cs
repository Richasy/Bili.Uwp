// Copyright (c) Richasy. All rights reserved.

using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Bili.Lib.Interfaces;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Video;
using Bili.ViewModels.Uwp.Base;
using Splat;
using Windows.UI.Core;

namespace Bili.ViewModels.Uwp.Home
{
    /// <summary>
    /// 热门视图模型.
    /// </summary>
    public sealed partial class PopularPageViewModel : InformationFlowViewModelBase<IVideoItemViewModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PopularPageViewModel"/> class.
        /// </summary>
        public PopularPageViewModel(
            IResourceToolkit resourceToolkit,
            IHomeProvider homeProvider,
            CoreDispatcher coreDispatcher)
            : base(coreDispatcher)
        {
            _resourceToolkit = resourceToolkit;
            _homeProvider = homeProvider;
        }

        /// <inheritdoc/>
        protected override void BeforeReload()
            => _homeProvider.ResetPopularState();

        /// <inheritdoc/>
        protected override string FormatException(string errorMsg)
            => $"{_resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.RequestPopularFailed)}\n{errorMsg}";

        /// <inheritdoc/>
        protected override async Task GetDataAsync()
        {
            var videos = await _homeProvider.RequestPopularVideosAsync();
            if (videos?.Any() ?? false)
            {
                foreach (var item in videos)
                {
                    var vm = Splat.Locator.Current.GetService<IVideoItemViewModel>();
                    vm.InjectData(item);
                    Items.Add(vm);
                }
            }
        }
    }
}
