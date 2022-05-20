// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;

namespace Bili.Models.Data.Live
{
    /// <summary>
    /// 直播间视图.
    /// </summary>
    public sealed class LiveRoomView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LiveRoomView"/> class.
        /// </summary>
        /// <param name="information">直播间信息.</param>
        /// <param name="partition">直播间分区.</param>
        public LiveRoomView(LiveInformation information, string partition)
        {
            Information = information;
            Partition = partition;
        }

        /// <summary>
        /// 直播间信息.
        /// </summary>
        public LiveInformation Information { get; }

        /// <summary>
        /// 直播间所属分区及分类.
        /// </summary>
        public string Partition { get; }

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is LiveRoomView view && EqualityComparer<LiveInformation>.Default.Equals(Information, view.Information);

        /// <inheritdoc/>
        public override int GetHashCode() => Information.GetHashCode();
    }
}
