// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Bili.Adapter.Interfaces;
using Bili.Lib.Interfaces;
using Bili.Models.App.Constants;
using Bili.Models.BiliBili;
using Bili.Models.Enums;
using static Bili.Models.App.Constants.ServiceConstants;

namespace Bili.Lib.Uwp
{
    /// <summary>
    /// 获取专业内容创作数据的工具.
    /// </summary>
    public partial class PgcProvider
    {
        private readonly IHttpProvider _httpProvider;
        private readonly IHomeProvider _partitionProvider;
        private readonly ICommunityAdapter _communityAdapter;
        private readonly IPgcAdapter _pgcAdapter;

        private readonly Dictionary<PgcType, string> _pgcOffsetCache;
        private int _indexPageNumber;

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

        private Dictionary<string, string> GetPageDetailQueryParameters(string tabId)
        {
            var queryParameters = new Dictionary<string, string>
            {
                { Query.Device, "phone" },
                { Query.Fnval, "976" },
                { Query.Fnver, "0" },
                { Query.Fourk, "1" },
                { Query.Qn, "112" },
                { Query.TabId, tabId },
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
            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Get, ApiConstants.Pgc.PageDetail, queryParameters, RequestClientType.IOS);
            var response = await _httpProvider.SendAsync(request);
            var data = await _httpProvider.ParseAsync<ServerResponse2<PgcResponse>>(response);
            return data.Result;
        }

        private Dictionary<string, string> GetPgcDetailInformationQueryParameters(int episodeId, int seasonId, string area)
        {
            var queryParameters = new Dictionary<string, string>
            {
                { Query.AutoPlay, "0" },
                { Query.IsShowAllSeries, "0" },
            };

            if (!string.IsNullOrEmpty(area))
            {
                queryParameters.Add(Query.Area, area);
            }

            if (episodeId > 0)
            {
                queryParameters.Add(Query.EpisodeId, episodeId.ToString());
            }

            if (seasonId > 0)
            {
                queryParameters.Add(Query.SeasonId, seasonId.ToString());
            }

            return queryParameters;
        }

        private Dictionary<string, string> GetEpisodeInteractionQueryParameters(int episodeId)
        {
            var queryParameters = new Dictionary<string, string>
            {
                { Query.EpisodeId, episodeId.ToString() },
            };

            return queryParameters;
        }

        private Dictionary<string, string> GetFollowQueryParameters(int seasonId)
        {
            var queryParameters = new Dictionary<string, string>
            {
                { Query.SeasonId, seasonId.ToString() },
            };

            return queryParameters;
        }

        private Dictionary<string, string> GetPgcIndexBaseQueryParameters(PgcType type)
        {
            var queryParameters = new Dictionary<string, string>();

            if (type == PgcType.Bangumi || type == PgcType.Domestic)
            {
                queryParameters.Add(Query.SeasonType, "1");
            }
            else
            {
                var indexType = string.Empty;
                switch (type)
                {
                    case PgcType.Movie:
                        indexType = "2";
                        break;
                    case PgcType.Documentary:
                        indexType = "3";
                        break;
                    case PgcType.TV:
                        indexType = "5";
                        break;
                    default:
                        break;
                }

                queryParameters.Add(Query.IndexType, indexType);
            }

            queryParameters.Add(Query.Type, "0");
            return queryParameters;
        }

        private Dictionary<string, string> GetPgcIndexResultQueryParameters(PgcType type, int page, Dictionary<string, string> additionalParameters)
        {
            var queryParameters = GetPgcIndexBaseQueryParameters(type);
            queryParameters.Add(Query.PageSizeFull, "21");
            queryParameters.Add(Query.Page, page.ToString());

            if (additionalParameters != null)
            {
                foreach (var item in additionalParameters)
                {
                    queryParameters.Add(item.Key, item.Value);
                }
            }

            return queryParameters;
        }

        private Dictionary<string, string> GetPgcTimeLineQueryParameters(PgcType type)
        {
            var typeStr = string.Empty;
            switch (type)
            {
                case PgcType.Bangumi:
                    typeStr = "2";
                    break;
                case PgcType.Domestic:
                    typeStr = "3";
                    break;
                case PgcType.Movie:
                case PgcType.Documentary:
                case PgcType.TV:
                default:
                    break;
            }

            var queryParameters = new Dictionary<string, string>
            {
                { Query.Type, typeStr },
                { Query.FilterType, "0" },
            };
            return queryParameters;
        }

        private Dictionary<string, string> GetPgcPlayListQueryParameters(string id)
        {
            var queryParameters = new Dictionary<string, string>
            {
                { Query.Id, id },
            };

            return queryParameters;
        }
    }
}
