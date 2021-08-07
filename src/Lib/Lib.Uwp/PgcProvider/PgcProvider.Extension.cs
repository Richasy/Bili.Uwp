// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Richasy.Bili.Lib.Interfaces;
using Richasy.Bili.Models.BiliBili;
using Richasy.Bili.Models.Enums;
using static Richasy.Bili.Models.App.Constants.ServiceConstants;

namespace Richasy.Bili.Lib.Uwp
{
    /// <summary>
    /// 获取专业内容创作数据的工具.
    /// </summary>
    public partial class PgcProvider
    {
        private readonly IHttpProvider _httpProvider;

        private readonly IPartitionProvider _partitionProvider;

        private Dictionary<string, string> GetTabQueryParameters(PgcType type)
        {
            var queryParameters = new Dictionary<string, string>
            {
                { Query.Device, "phone" },
                { Query.IsHideRecommendTab, "0" },
            };
            var parentTab = string.Empty;
            switch (type)
            {
                case PgcType.Bangumi:
                    parentTab = BangumiOperation;
                    break;
                case PgcType.Domestic:
                    parentTab = DomesticOperation;
                    break;
                default:
                    break;
            }

            if (!string.IsNullOrEmpty(parentTab))
            {
                queryParameters.Add(Query.ParentTab, parentTab);
            }

            return queryParameters;
        }

        private Dictionary<string, string> GetPageDetailQueryParameters(int tabId)
        {
            var queryParameters = new Dictionary<string, string>
            {
                { Query.Device, "phone" },
                { Query.Fnval, "976" },
                { Query.Fnver, "0" },
                { Query.Fourk, "1" },
                { Query.Qn, "112" },
                { Query.TabId, tabId.ToString() },
                { Query.TeenagersMode, "0" },
            };

            return queryParameters;
        }

        private Dictionary<string, string> GetPageDetailQueryParameters(PgcType type, string cursor)
        {
            var queryParameters = new Dictionary<string, string>
            {
                { Query.Device, "phone" },
                { Query.Fnval, "976" },
                { Query.Fnver, "0" },
                { Query.Fourk, "1" },
                { Query.Qn, "112" },
            };

            switch (type)
            {
                case PgcType.Movie:
                    queryParameters.Add(Query.Name, MovieOperation);
                    break;
                case PgcType.Documentary:
                    queryParameters.Add(Query.Name, DocumentaryOperation);
                    break;
                case PgcType.TV:
                    queryParameters.Add(Query.Name, TvOperation);
                    break;
                default:
                    break;
            }

            if (!string.IsNullOrEmpty(cursor))
            {
                queryParameters.Add(Query.Cursor, cursor);
            }

            return queryParameters;
        }

        private async Task<PgcResponse> GetPgcResponseInternalAsync(Dictionary<string, string> queryParameters)
        {
            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Get, Api.Pgc.PageDetail, queryParameters, RequestClientType.IOS);
            var response = await _httpProvider.SendAsync(request);
            var data = await _httpProvider.ParseAsync<ServerResponse2<PgcResponse>>(response);
            return data.Result;
        }
    }
}
