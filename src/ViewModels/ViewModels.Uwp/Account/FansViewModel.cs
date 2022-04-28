// Copyright (c) Richasy. All rights reserved.

using System;
using Bili.Models.Enums.App;

namespace Bili.ViewModels.Uwp
{
    /// <summary>
    /// 粉丝视图模型.
    /// </summary>
    public class FansViewModel : RelatedUserViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FansViewModel"/> class.
        /// </summary>
        protected FansViewModel()
            : base(RelatedUserType.Fans)
        {
        }

        /// <summary>
        /// 单例.
        /// </summary>
        public static FansViewModel Instance { get; } = new Lazy<FansViewModel>(() => new FansViewModel()).Value;
    }
}
