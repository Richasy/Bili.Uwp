// Copyright (c) GodLeaveMe. All rights reserved.

using System;

namespace Richasy.Bili.ViewModels.Uwp
{
    /// <summary>
    /// 追番视图模型.
    /// </summary>
    public class FavoriteAnimeViewModel : PgcFavoriteViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FavoriteAnimeViewModel"/> class.
        /// </summary>
        protected FavoriteAnimeViewModel()
            : base(Models.Enums.App.FavoriteType.Anime)
        {
        }

        /// <summary>
        /// 实例.
        /// </summary>
        public static FavoriteAnimeViewModel Instance { get; } = new Lazy<FavoriteAnimeViewModel>(() => new FavoriteAnimeViewModel()).Value;
    }
}
