// Copyright (c) Richasy. All rights reserved.

using Bili.Lib.Interfaces;
using Bili.Models.Enums;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Core;
using Bili.ViewModels.Interfaces.Pgc;
using Bili.ViewModels.Uwp.Base;

namespace Bili.ViewModels.Uwp.Pgc
{
    /// <summary>
    /// 番剧页视图模型.
    /// </summary>
    public sealed class BangumiPageViewModel : AnimePageViewModelBase, IBangumiPageViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BangumiPageViewModel"/> class.
        /// </summary>
        public BangumiPageViewModel(
            IPgcProvider pgcProvider,
            IAuthorizeProvider authorizeProvider,
            IHomeProvider homeProvider,
            IResourceToolkit resourceToolkit,
            INavigationViewModel navigationViewModel)
            : base(pgcProvider, authorizeProvider, homeProvider, resourceToolkit, navigationViewModel, PgcType.Bangumi)
        {
        }
    }
}
