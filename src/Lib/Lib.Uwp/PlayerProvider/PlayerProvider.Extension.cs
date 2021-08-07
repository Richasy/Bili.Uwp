// Copyright (c) Richasy. All rights reserved.

using System;
using System.Threading;
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

        private CancellationToken GetExpiryToken(int seconds = 5)
        {
            var source = new CancellationTokenSource(TimeSpan.FromSeconds(seconds));
            return source.Token;
        }
    }
}
