// Copyright (c) Richasy. All rights reserved.

using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Bili.Lib.Interfaces;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Uwp.Base;
using Bili.ViewModels.Uwp.Video;
using Splat;
using Windows.UI.Core;

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
        /// <param name="resourceToolkit">资源管理工具.</param>
        /// <param name="popularProvider">热门服务提供工具.</param>
        /// <param name="coreDispatcher">UI 调度器.</param>
        internal PopularPageViewModel(
            IResourceToolkit resourceToolkit,
            IPopularProvider popularProvider,
            CoreDispatcher coreDispatcher)
            : base(coreDispatcher)
        {
            _resourceToolkit = resourceToolkit;
            _popularProvider = popularProvider;
        }

        /// <inheritdoc/>
        protected override void BeforeReload()
            => _popularProvider.Reset();

        /// <inheritdoc/>
        protected override string FormatException(string errorMsg)
            => $"{_resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.RequestPopularFailed)}\n{errorMsg}";

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
