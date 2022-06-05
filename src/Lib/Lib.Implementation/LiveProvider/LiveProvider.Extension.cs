// Copyright (c) Richasy. All rights reserved.

using Bili.Adapter.Interfaces;
using Bili.Lib.Interfaces;

namespace Bili.Lib
{
    /// <summary>
    /// 提供直播相关的操作.
    /// </summary>
    public partial class LiveProvider
    {
        private readonly IHttpProvider _httpProvider;
        private readonly IAccountProvider _accountProvider;
        private readonly ILiveAdapter _liveAdapter;
        private readonly ICommunityAdapter _communityAdapter;

        private int _feedPageNumber;
        private int _partitionPageNumber;
    }
}
