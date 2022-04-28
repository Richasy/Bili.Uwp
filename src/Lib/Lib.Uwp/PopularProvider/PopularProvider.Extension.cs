// Copyright (c) Richasy. All rights reserved.

using Bili.Lib.Interfaces;

namespace Bili.Lib.Uwp
{
    /// <summary>
    /// 热门数据处理.
    /// </summary>
    public partial class PopularProvider
    {
        private readonly IHttpProvider _httpProvider;
        private readonly IAuthorizeProvider _authorizeProvider;
    }
}
