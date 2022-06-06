// Copyright (c) Richasy. All rights reserved.

using Bili.Adapter.Interfaces;
using Bili.Lib.Interfaces;

namespace Bili.Lib
{
    /// <summary>
    /// 社区交互数据处理.
    /// </summary>
    public partial class CommunityProvider
    {
        private readonly IHttpProvider _httpProvider;
        private readonly IDynamicAdapter _dynamicAdapter;

        private (string Offset, string Baseline) _videoDynamicOffset;
        private (string Offset, string Baseline) _comprehensiveDynamicOffset;
    }
}
