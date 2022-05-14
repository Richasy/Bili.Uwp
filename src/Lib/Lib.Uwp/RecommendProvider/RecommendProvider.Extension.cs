// Copyright (c) Richasy. All rights reserved.

using Bili.Adapter.Interfaces;
using Bili.Lib.Interfaces;

namespace Bili.Lib.Uwp
{
    /// <summary>
    /// <see cref="RecommendProvider"/>的属性定义及扩展.
    /// </summary>
    public partial class RecommendProvider
    {
        private readonly IHttpProvider _httpProvider;
        private readonly IVideoAdapter _videoAdapter;
        private readonly IPgcAdapter _pgcAdapter;

        private long _offsetId;
    }
}
