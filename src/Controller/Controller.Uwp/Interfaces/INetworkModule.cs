// Copyright (c) Richasy. All rights reserved.

using System;
using Richasy.Bili.Controller.Uwp.Utilities;

namespace Richasy.Bili.Controller.Uwp.Interfaces
{
    /// <summary>
    /// Definition of network module.
    /// </summary>
    public interface INetworkModule
    {
        /// <summary>
        /// Event raised when the network changes.
        /// </summary>
        event EventHandler NetworkChanged;

        /// <summary>
        /// Gets instance of <see cref="Utilities.ConnectionInformation"/>.
        /// </summary>
        ConnectionInformation ConnectionInformation { get; }

        /// <summary>
        /// Gets a value indicating whether internet is available across all connections.
        /// </summary>
        bool IsNetworkAvaliable { get; }
    }
}
