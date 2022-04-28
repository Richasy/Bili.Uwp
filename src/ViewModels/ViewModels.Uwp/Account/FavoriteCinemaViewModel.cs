// Copyright (c) Richasy. All rights reserved.

using System;

namespace Bili.ViewModels.Uwp
{
    /// <summary>
    /// 追剧视图模型.
    /// </summary>
    public class FavoriteCinemaViewModel : PgcFavoriteViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FavoriteCinemaViewModel"/> class.
        /// </summary>
        protected FavoriteCinemaViewModel()
            : base(Models.Enums.App.FavoriteType.Cinema)
        {
        }

        /// <summary>
        /// 实例.
        /// </summary>
        public static FavoriteCinemaViewModel Instance { get; } = new Lazy<FavoriteCinemaViewModel>(() => new FavoriteCinemaViewModel()).Value;
    }
}
