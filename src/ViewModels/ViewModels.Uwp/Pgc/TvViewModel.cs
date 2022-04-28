// Copyright (c) Richasy. All rights reserved.

using System;
using Bili.Models.Enums;

namespace Bili.ViewModels.Uwp
{
    /// <summary>
    /// 电视剧视图模型.
    /// </summary>
    public class TvViewModel : FeedPgcViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TvViewModel"/> class.
        /// </summary>
        public TvViewModel()
            : base(PgcType.TV)
        {
        }

        /// <summary>
        /// 单例.
        /// </summary>
        public static TvViewModel Instance { get; } = new Lazy<TvViewModel>(() => new TvViewModel()).Value;
    }
}
