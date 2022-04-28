// Copyright (c) Richasy. All rights reserved.

using System;

namespace Bili.ViewModels.Uwp
{
    /// <summary>
    /// 番剧数据视图模型.
    /// </summary>
    public class BangumiViewModel : AnimeViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BangumiViewModel"/> class.
        /// </summary>
        protected BangumiViewModel()
            : base(Models.Enums.PgcType.Bangumi)
        {
        }

        /// <summary>
        /// 单例.
        /// </summary>
        public static BangumiViewModel Instance { get; } = new Lazy<BangumiViewModel>(() => new BangumiViewModel()).Value;
    }
}
