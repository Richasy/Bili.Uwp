// Copyright (c) Richasy. All rights reserved.

using Richasy.Bili.Lib.Interfaces;

namespace Richasy.Bili.Lib.Uwp
{
    /// <summary>
    /// 提供视频相关操作.
    /// </summary>
    public partial class PlayerProvider
    {
        private readonly IHttpProvider _httpProvider;
        private readonly IAccountProvider _accountProvider;
    }
}
