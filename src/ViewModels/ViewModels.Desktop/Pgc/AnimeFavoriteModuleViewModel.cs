// Copyright (c) Richasy. All rights reserved.

using Bili.Lib.Interfaces;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Desktop.Base;
using Bili.ViewModels.Interfaces.Pgc;

namespace Bili.ViewModels.Desktop.Pgc
{
    /// <summary>
    /// 追番视图模型.
    /// </summary>
    public class AnimeFavoriteModuleViewModel : PgcFavoriteModuleViewModelBase, IAnimeFavoriteModuleViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AnimeFavoriteModuleViewModel"/> class.
        /// </summary>
        public AnimeFavoriteModuleViewModel(
            IFavoriteProvider favoriteProvider,
            IResourceToolkit resourceToolkit)
            : base(Models.Enums.App.FavoriteType.Anime, favoriteProvider, resourceToolkit)
        {
        }
    }
}
