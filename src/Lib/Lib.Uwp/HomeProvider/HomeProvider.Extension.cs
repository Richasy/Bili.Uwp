// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using Bili.Adapter.Interfaces;
using Bili.Lib.Interfaces;
using Bili.Toolkit.Interfaces;

namespace Bili.Lib.Uwp
{
    /// <summary>
    /// 分区及标签的相关定义和扩展方法.
    /// </summary>
    public partial class HomeProvider
    {
        private readonly IHttpProvider _httpProvider;
        private readonly IAuthorizeProvider _authorizeProvider;
        private readonly ICommunityAdapter _communityAdapter;
        private readonly IVideoAdapter _videoAdapter;
        private readonly IFileToolkit _fileToolkit;
        private readonly IPgcAdapter _pgcAdapter;

        private readonly Dictionary<string, (int OffsetId, int PageNumber)> _cacheVideoPartitionOffsets;

        private long _recommendOffsetId;
        private long _popularOffsetId;
        private int videoPartitionOffsetId = 0;
        private int videoPartitionPageNumber = 1;
        private string currentPartitionId = string.Empty;
    }
}
