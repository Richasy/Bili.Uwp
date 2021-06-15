// Copyright (c) Richasy. All rights reserved.

using Richasy.Bili.Controller.Uwp;
using Richasy.Bili.Models.Enums;

namespace Richasy.Bili.ViewModels.Uwp
{
    /// <summary>
    /// 应用ViewModel.
    /// </summary>
    public partial class AppViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppViewModel"/> class.
        /// </summary>
        internal AppViewModel()
        {
            _ = BiliController.Instance;
            IsNavigatePaneOpen = true;
            CurrentMainContentId = PageIds.Partition;
        }

        /// <summary>
        /// 修改当前主内容标识.
        /// </summary>
        /// <param name="pageId">主内容标识.</param>
        public void SetMainContentId(PageIds pageId)
        {
            CurrentMainContentId = pageId;
            IsShowOverlay = false;
        }

        /// <summary>
        /// 修改当前覆盖内容标识.
        /// </summary>
        /// <param name="pageId">覆盖内容标识.</param>
        /// <param name="param">导航参数.</param>
        public void SetOverlayContentId(PageIds pageId, object param = null)
        {
            CurrentOverlayContentId = pageId;
            IsShowOverlay = true;
            RequestOverlayNavigation?.Invoke(this, param);
        }
    }
}
