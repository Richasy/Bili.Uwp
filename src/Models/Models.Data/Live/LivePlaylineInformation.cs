// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;

namespace Bili.Models.Data.Live
{
    /// <summary>
    /// 直播解码信息.
    /// </summary>
    public sealed class LivePlaylineInformation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LivePlaylineInformation"/> class.
        /// </summary>
        /// <param name="name">线路名.</param>
        /// <param name="quality">清晰度.</param>
        /// <param name="acceptQualities">支持的清晰度列表.</param>
        /// <param name="urls">地址列表.</param>
        public LivePlaylineInformation(
            string name,
            int quality,
            IEnumerable<int> acceptQualities,
            IEnumerable<LivePlayUrl> urls)
        {
            Name = name;
            Quality = quality;
            AcceptQualities = acceptQualities;
            Urls = urls;
        }

        /// <summary>
        /// 解码名.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// 清晰度标识.
        /// </summary>
        public int Quality { get; }

        /// <summary>
        /// 支持的清晰度标识.
        /// </summary>
        public IEnumerable<int> AcceptQualities { get; }

        /// <summary>
        /// 播放地址列表.
        /// </summary>
        public IEnumerable<LivePlayUrl> Urls { get; }
    }
}
