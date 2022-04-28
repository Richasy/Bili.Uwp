// Copyright (c) Richasy. All rights reserved.

using Bili.Lib.Interfaces;

namespace Bili.Lib.Uwp
{
    /// <summary>
    /// 提供直播相关的操作.
    /// </summary>
    public partial class LiveProvider
    {
        private readonly IHttpProvider _httpProvider;
        private readonly IAccountProvider _accountProvider;
    }
}
