// Copyright (c) Richasy. All rights reserved.

namespace Bili.Models.Data.Live
{
    /// <summary>
    /// 直播地址拼接信息.
    /// </summary>
    public sealed class LivePlayUrl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LivePlayUrl"/> class.
        /// </summary>
        /// <param name="host">域名.</param>
        /// <param name="route">路由.</param>
        /// <param name="query">查询参数.</param>
        public LivePlayUrl(string host, string route, string query)
        {
            Host = host;
            Route = route;
            Query = query;
        }

        /// <summary>
        /// 域名.
        /// </summary>
        public string Host { get; }

        /// <summary>
        /// 路由.
        /// </summary>
        public string Route { get; }

        /// <summary>
        /// 查询参数.
        /// </summary>
        public string Query { get; }

        /// <inheritdoc/>
        public override string ToString()
            => $"{Host}{Route}{Query}";
    }
}
