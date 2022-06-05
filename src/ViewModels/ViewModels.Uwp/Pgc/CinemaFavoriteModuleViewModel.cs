// Copyright (c) Richasy. All rights reserved.

using System;
using Bili.Lib.Interfaces;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Uwp.Base;
using Windows.UI.Core;

namespace Bili.ViewModels.Uwp
{
    /// <summary>
    /// 追剧视图模型.
    /// </summary>
    public class CinemaFavoriteModuleViewModel : PgcFavoriteModuleViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CinemaFavoriteModuleViewModel"/> class.
        /// </summary>
        internal CinemaFavoriteModuleViewModel(
            IFavoriteProvider favoriteProvider,
            IResourceToolkit resourceToolkit,
            CoreDispatcher dispatcher)
            : base(Models.Enums.App.FavoriteType.Cinema, favoriteProvider, resourceToolkit, dispatcher)
        {
        }
    }
}
