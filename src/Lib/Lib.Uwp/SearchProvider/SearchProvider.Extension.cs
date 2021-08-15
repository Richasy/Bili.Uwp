// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using System.Threading.Tasks;
using Richasy.Bili.Lib.Interfaces;
using Richasy.Bili.Models.BiliBili;

using static Richasy.Bili.Models.App.Constants.ServiceConstants;

namespace Richasy.Bili.Lib.Uwp
{
    /// <summary>
    /// 搜索工具.
    /// </summary>
    public partial class SearchProvider
    {
        private readonly IHttpProvider _httpProvider;
    }

    private async Task<SubModuleSearchResultResponse<T>> GetSubModuleResultAsync<T>(int typeId, int pageNumber = 0)
    {
        var queryParameters = new Dictionary<string, string>();
        queryParameters.Add(Query.)
    }
}
