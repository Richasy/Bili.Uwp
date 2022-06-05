// Copyright (c) Richasy. All rights reserved.

using System;
using Bili.Lib.Interfaces;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Uwp.Base;
using Windows.UI.Core;

namespace Bili.ViewModels.Uwp
{
    /// <summary>
    /// 追番视图模型.
    /// </summary>
    public class AnimeFavoriteModuleViewModel : PgcFavoriteModuleViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AnimeFavoriteModuleViewModel"/> class.
        /// </summary>
        internal AnimeFavoriteModuleViewModel(
            IFavoriteProvider favoriteProvider,
            IResourceToolkit resourceToolkit,
            CoreDispatcher dispatcher)
            : base(Models.Enums.App.FavoriteType.Anime, favoriteProvider, resourceToolkit, dispatcher)
        {
        }
    }
}
