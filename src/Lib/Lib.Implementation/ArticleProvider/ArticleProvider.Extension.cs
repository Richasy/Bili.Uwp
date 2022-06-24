// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using Bili.Adapter.Interfaces;
using Bili.Lib.Interfaces;
using Bili.Models.Enums;

namespace Bili.Lib
{
    /// <summary>
    /// 提供专栏文章相关的操作.
    /// </summary>
    public partial class ArticleProvider
    {
        private readonly IHttpProvider _httpProvider;
        private readonly IAccountProvider _accountProvider;
        private readonly IArticleAdapter _articleAdapter;
        private readonly ICommunityAdapter _communityAdapter;

        private readonly Dictionary<string, (ArticleSortType Sort, int PageNumber)> _partitionCache;
    }
}
