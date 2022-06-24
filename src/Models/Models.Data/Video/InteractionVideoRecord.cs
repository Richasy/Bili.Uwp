// Copyright (c) Richasy. All rights reserved.

namespace Bili.Models.Data.Video
{
    /// <summary>
    /// 互动视频记录点.
    /// </summary>
    public sealed class InteractionVideoRecord
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InteractionVideoRecord"/> class.
        /// </summary>
        /// <param name="version">版本号.</param>
        /// <param name="partId">分集 Id.</param>
        /// <param name="nodeId">节点 Id.</param>
        public InteractionVideoRecord(
            string version,
            string partId = "",
            string nodeId = "")
        {
            GraphVersion = version;
            PartId = partId;
            NodeId = nodeId;
        }

        /// <summary>
        /// 版本号.
        /// </summary>
        public string GraphVersion { get; }

        /// <summary>
        /// 分集 Id.
        /// </summary>
        public string PartId { get; set; }

        /// <summary>
        /// 节点 Id.
        /// </summary>
        public string NodeId { get; set; }
    }
}
