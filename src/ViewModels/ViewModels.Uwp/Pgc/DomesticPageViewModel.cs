// Copyright (c) Richasy. All rights reserved.

using Bili.Lib.Interfaces;
using Bili.Models.Enums;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Core;
using Bili.ViewModels.Uwp.Base;

namespace Bili.ViewModels.Uwp.Pgc
{
    /// <summary>
    /// 国创页面视图模型.
    /// </summary>
    public sealed class DomesticPageViewModel : AnimePageViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DomesticPageViewModel"/> class.
        /// </summary>
        /// <param name="pgcProvider">PGC 服务提供工具.</param>
        /// <param name="authorizeProvider">授权服务提供工具.</param>
        /// <param name="homeProvider">主页数据服务提供工具.</param>
        /// <param name="resourceToolkit">本地资源管理工具.</param>
        public DomesticPageViewModel(
            IPgcProvider pgcProvider,
            IAuthorizeProvider authorizeProvider,
            IHomeProvider homeProvider,
            IResourceToolkit resourceToolkit,
            INavigationViewModel navigationViewModel)
            : base(pgcProvider, authorizeProvider, homeProvider, resourceToolkit, navigationViewModel, PgcType.Domestic)
        {
        }
    }
}
