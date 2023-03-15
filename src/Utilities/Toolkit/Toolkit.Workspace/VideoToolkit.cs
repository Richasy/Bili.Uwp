// Copyright (c) Richasy. All rights reserved.
using System;
using Bili.Models.Enums;
using Bili.Toolkit.Interfaces;

namespace Bili.Toolkit.Workspace
{
    /// <summary>
    /// 视频工具.
    /// </summary>
    public sealed class VideoToolkit : IVideoToolkit
    {
        /// <inheritdoc/>
        public VideoIdType GetVideoIdType(string id, out string avId)
        {
            avId = string.Empty;
            if (id.StartsWith("bv", StringComparison.OrdinalIgnoreCase))
            {
                // 判定为 BV Id.
                return VideoIdType.Bv;
            }
            else
            {
                // 可能是 AV Id.
                id = id.Replace("av", string.Empty, StringComparison.OrdinalIgnoreCase);
                if (long.TryParse(id, out var validId))
                {
                    avId = id;
                    return VideoIdType.Av;
                }

                return VideoIdType.Invalid;
            }
        }
    }
}
