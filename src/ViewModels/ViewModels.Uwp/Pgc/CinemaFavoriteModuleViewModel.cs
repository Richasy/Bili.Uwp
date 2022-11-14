// Copyright (c) Richasy. All rights reserved.

using Bili.Lib.Interfaces;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Pgc;
using Bili.ViewModels.Uwp.Base;

namespace Bili.ViewModels.Uwp.Pgc
{
    /// <summary>
    /// 追剧视图模型.
    /// </summary>
    public class CinemaFavoriteModuleViewModel : PgcFavoriteModuleViewModelBase, ICinemaFavoriteModuleViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CinemaFavoriteModuleViewModel"/> class.
        /// </summary>
        public CinemaFavoriteModuleViewModel(
            IFavoriteProvider favoriteProvider,
            IResourceToolkit resourceToolkit)
            : base(Models.Enums.App.FavoriteType.Cinema, favoriteProvider, resourceToolkit)
        {
        }
    }
}
