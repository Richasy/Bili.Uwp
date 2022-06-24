// Copyright (c) Richasy. All rights reserved.

using System.Linq;
using System.Threading.Tasks;
using Bili.Lib.Interfaces;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Uwp.Community;
using Splat;
using Windows.UI.Core;

namespace Bili.ViewModels.Uwp.Base
{
    /// <summary>
    /// 动态模块视图模型基类.
    /// </summary>
    public partial class DynamicModuleViewModelBase : InformationFlowViewModelBase<DynamicItemViewModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicAllModuleViewModel"/> class.
        /// </summary>
        internal DynamicModuleViewModelBase(
            ICommunityProvider communityProvider,
            IResourceToolkit resourceToolkit,
            ISettingsToolkit settingsToolkit,
            bool isOnlyVideo,
            CoreDispatcher dispatcher)
            : base(dispatcher)
        {
            _communityProvider = communityProvider;
            _resourceToolkit = resourceToolkit;
            _settingsToolkit = settingsToolkit;
            _isOnlyVideo = isOnlyVideo;
        }

        /// <inheritdoc/>
        protected override void BeforeReload()
        {
            if (_isOnlyVideo)
            {
                _communityProvider.ResetVideoDynamicStatus();
            }
            else
            {
                _communityProvider.ResetComprehensiveDynamicStatus();
            }

            _isEnd = false;
            IsEmpty = false;
        }

        /// <inheritdoc/>
        protected override string FormatException(string errorMsg)
            => $"{_resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.RequestDynamicFailed)}\n{errorMsg}";

        /// <inheritdoc/>
        protected override async Task GetDataAsync()
        {
            if (_isEnd)
            {
                return;
            }

            var data = _isOnlyVideo
                ? await _communityProvider.GetDynamicVideoListAsync()
                : await _communityProvider.GetDynamicComprehensiveListAsync();

            _isEnd = data.Dynamics == null || data.Dynamics.Count() == 0;

            if (!_isEnd)
            {
                if (Items.Count == 0)
                {
                    // 这是一次新的请求，将最新的动态Id写入本地设置，作为是否已阅读的判断依据.
                    _settingsToolkit.WriteLocalSetting(Models.Enums.SettingNames.LastReadVideoDynamicId, data.Dynamics.First().Id);
                }

                foreach (var item in data.Dynamics)
                {
                    var dynamicVM = Splat.Locator.Current.GetService<DynamicItemViewModel>();
                    dynamicVM.SetInformation(item);
                    Items.Add(dynamicVM);
                }
            }

            IsEmpty = Items.Count == 0;
        }
    }
}
