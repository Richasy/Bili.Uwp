// Copyright (c) Richasy. All rights reserved.

using Bili.Lib.Interfaces;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Desktop.Base;
using Bili.ViewModels.Interfaces.Pgc;

namespace Bili.ViewModels.Desktop.Pgc
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
