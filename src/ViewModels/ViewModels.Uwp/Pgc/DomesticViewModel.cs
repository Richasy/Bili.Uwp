// Copyright (c) Richasy. All rights reserved.

using System;

namespace Richasy.Bili.ViewModels.Uwp
{
    /// <summary>
    /// 国创数据视图模型.
    /// </summary>
    public class DomesticViewModel : AnimeViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DomesticViewModel"/> class.
        /// </summary>
        protected DomesticViewModel()
            : base(Models.Enums.PgcType.Domestic)
        {
        }

        /// <summary>
        /// 单例.
        /// </summary>
        public static DomesticViewModel Instance { get; } = new Lazy<DomesticViewModel>(() => new DomesticViewModel()).Value;
    }
}
