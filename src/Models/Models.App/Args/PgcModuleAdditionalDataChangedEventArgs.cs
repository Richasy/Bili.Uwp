// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using Richasy.Bili.Models.BiliBili;
using Richasy.Bili.Models.Enums;

namespace Richasy.Bili.Models.App.Args
{
    /// <summary>
    /// PGC模块附加数据改变事件参数.
    /// </summary>
    public class PgcModuleAdditionalDataChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PgcModuleAdditionalDataChangedEventArgs"/> class.
        /// </summary>
        protected PgcModuleAdditionalDataChangedEventArgs()
        {
            Banners = new List<PgcModuleItem>();
            Rank = new List<PgcModuleItem>();
        }

        /// <summary>
        /// 横幅列表.
        /// </summary>
        public List<PgcModuleItem> Banners { get; set; }

        /// <summary>
        /// 排行榜.
        /// </summary>
        public List<PgcModuleItem> Rank { get; set; }

        /// <summary>
        /// 需要请求的分区Id.
        /// </summary>
        public int RequestPartitionId { get; set; }

        /// <summary>
        /// 标签页Id.
        /// </summary>
        public int TabId { get; set; }

        /// <summary>
        /// PGC类型.
        /// </summary>
        public PgcType PgcType { get; set; }

        /// <summary>
        /// 根据PGC响应结果创建事件参数.
        /// </summary>
        /// <param name="response">PGC响应结果.</param>
        /// <param name="tabId">标签页Id.</param>
        /// <returns><see cref="PgcModuleAdditionalDataChangedEventArgs"/>.</returns>
        public static PgcModuleAdditionalDataChangedEventArgs Create(PgcResponse response, int tabId)
        {
            var args = new PgcModuleAdditionalDataChangedEventArgs();
            args.TabId = tabId;
            foreach (var item in response.Modules)
            {
                if (item.Style.Contains("banner"))
                {
                    item.Items.ForEach(p => args.Banners.Add(p));
                }

                if (item.Style.Contains("rank"))
                {
                    item.Items.ForEach(p => args.Rank.Add(p));
                }
            }

            if (response.FeedIdentifier != null)
            {
                args.RequestPartitionId = response.FeedIdentifier.PartitionIds.First();
            }

            if (!args.Banners.Any() && !args.Rank.Any() && args.RequestPartitionId == 0)
            {
                return null;
            }

            args.PgcType = PgcType.Bangumi | PgcType.Domestic;
            return args;
        }

        /// <summary>
        /// 根据PGC响应结果创建事件参数.
        /// </summary>
        /// <param name="response">PGC响应结果.</param>
        /// <param name="type">PGC类型.</param>
        /// <returns><see cref="PgcModuleAdditionalDataChangedEventArgs"/>.</returns>
        public static PgcModuleAdditionalDataChangedEventArgs Create(PgcResponse response, PgcType type)
        {
            var args = new PgcModuleAdditionalDataChangedEventArgs();
            args.PgcType = type;
            foreach (var item in response.Modules)
            {
                if (item.Style.Contains("banner"))
                {
                    item.Items.ForEach(p => args.Banners.Add(p));
                }
            }

            if (!args.Banners.Any())
            {
                return null;
            }

            args.PgcType = type;
            return args;
        }
    }
}
