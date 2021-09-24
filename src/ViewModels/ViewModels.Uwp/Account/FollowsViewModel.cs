// Copyright (c) GodLeaveMe. All rights reserved.

using System;
using Richasy.Bili.Models.Enums.App;

namespace Richasy.Bili.ViewModels.Uwp
{
    /// <summary>
    /// 粉丝视图模型.
    /// </summary>
    public class FollowsViewModel : RelatedUserViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FansViewModel"/> class.
        /// </summary>
        protected FollowsViewModel()
            : base(RelatedUserType.Follows)
        {
        }

        /// <summary>
        /// 单例.
        /// </summary>
        public static FollowsViewModel Instance { get; } = new Lazy<FollowsViewModel>(() => new FollowsViewModel()).Value;
    }
}
