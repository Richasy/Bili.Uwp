// Copyright (c) Richasy. All rights reserved.

using Bili.Lib.Interfaces;
using Bili.Models.Enums;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Desktop.Base;
using Bili.ViewModels.Interfaces.Core;
using Bili.ViewModels.Interfaces.Pgc;

namespace Bili.ViewModels.Desktop.Pgc
{
    /// <summary>
    /// 电视剧视图模型.
    /// </summary>
    public sealed class TvPageViewModel : PgcPageViewModelBase, ITvPageViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TvPageViewModel"/> class.
        /// </summary>
        public TvPageViewModel(
            IPgcProvider pgcProvider,
            IResourceToolkit resourceToolkit,
            INavigationViewModel navigationViewModel)
            : base(pgcProvider, resourceToolkit, navigationViewModel, PgcType.TV)
        {
        }
    }
}
