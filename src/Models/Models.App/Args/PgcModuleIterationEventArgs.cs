// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using Richasy.Bili.Models.BiliBili;

namespace Richasy.Bili.Models.App.Args
{
    /// <summary>
    /// PGC内容模块迭代事件参数.
    /// </summary>
    public class PgcModuleIterationEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PgcModuleIterationEventArgs"/> class.
        /// </summary>
        protected PgcModuleIterationEventArgs()
        {
            Modules = new List<PgcModule>();
        }

        /// <summary>
        /// 模块列表.
        /// </summary>
        public List<PgcModule> Modules { get; set; }

        /// <summary>
        /// 偏移指针.
        /// </summary>
        public string Cursor { get; set; }

        /// <summary>
        /// 标签页Id.
        /// </summary>
        public int TabId { get; set; }

        /// <summary>
        /// 根据PGC响应结果创建事件参数.
        /// </summary>
        /// <param name="response">响应结果.</param>
        /// <param name="tabId">标签页Id.</param>
        /// <returns><see cref="PgcModuleIterationEventArgs"/>.</returns>
        public static PgcModuleIterationEventArgs Create(PgcResponse response, int tabId)
        {
            var args = new PgcModuleIterationEventArgs();
            args.Cursor = response.NextCursor;
            args.TabId = tabId;
            foreach (var module in response.Modules)
            {
                if (module.Style == "v-card" || module.Style.Contains("feed"))
                {
                    args.Modules.Add(module);
                }
            }

            return args;
        }
    }
}
