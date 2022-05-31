// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;

namespace Bili.Models.Data.User
{
    /// <summary>
    /// 用户关系视图，比如粉丝列表或者关注列表等.
    /// </summary>
    public sealed class RelationView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RelationView"/> class.
        /// </summary>
        /// <param name="accounts">账户列表.</param>
        /// <param name="totalCount">总数.</param>
        public RelationView(
            IEnumerable<AccountInformation> accounts,
            int totalCount)
        {
            Accounts = accounts;
            TotalCount = totalCount;
        }

        /// <summary>
        /// 账户列表.
        /// </summary>
        public IEnumerable<AccountInformation> Accounts { get; }

        /// <summary>
        /// 总数.
        /// </summary>
        public int TotalCount { get; }
    }
}
