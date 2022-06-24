// Copyright (c) Richasy. All rights reserved.

using Bili.Adapter.Interfaces;
using Bili.Lib.Interfaces;
using Bilibili.Main.Community.Reply.V1;

namespace Bili.Lib
{
    /// <summary>
    /// 社区交互数据处理.
    /// </summary>
    public partial class CommunityProvider
    {
        private readonly IHttpProvider _httpProvider;
        private readonly IDynamicAdapter _dynamicAdapter;
        private readonly ICommentAdapter _commentAdapter;

        private (string Offset, string Baseline) _videoDynamicOffset;
        private (string Offset, string Baseline) _comprehensiveDynamicOffset;

        private CursorReq _mainCommentCursor;
        private CursorReq _detailCommentCursor;
    }
}
