// Copyright (c) GodLeaveMe. All rights reserved.

using System;
using Richasy.Bili.Models.Enums;

namespace Richasy.Bili.ViewModels.Uwp
{
    /// <summary>
    /// 电影视图模型.
    /// </summary>
    public class MovieViewModel : FeedPgcViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MovieViewModel"/> class.
        /// </summary>
        public MovieViewModel()
            : base(PgcType.Movie)
        {
        }

        /// <summary>
        /// 单例.
        /// </summary>
        public static MovieViewModel Instance { get; } = new Lazy<MovieViewModel>(() => new MovieViewModel()).Value;
    }
}
