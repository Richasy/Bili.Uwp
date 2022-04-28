// Copyright (c) Richasy. All rights reserved.

using System;
using Bili.Controller.Uwp.Interfaces;
using Bili.Controller.Uwp.Utilities;
using Windows.Networking.Connectivity;

namespace Bili.Controller.Uwp.Modules
{
    /// <summary>
    /// Module used to detect whether the current network is available.
    /// </summary>
    public class NetworkModule : INetworkModule
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NetworkModule"/> class.
        /// </summary>
        public NetworkModule()
        {
            ConnectionInformation = new ConnectionInformation();
            UpdateConnectionInformation();
            NetworkInformation.NetworkStatusChanged += OnNetworkStatusChanged;
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="NetworkModule"/> class.
        /// </summary>
        ~NetworkModule()
        {
            NetworkInformation.NetworkStatusChanged -= OnNetworkStatusChanged;
        }

        /// <summary>
        /// Event raised when the network changes.
        /// </summary>
        public event EventHandler NetworkChanged;

        /// <summary>
        /// Gets instance of <see cref="ConnectionInformation"/>.
        /// </summary>
        public ConnectionInformation ConnectionInformation { get; }

        /// <inheritdoc/>
        public bool IsNetworkAvaliable => ConnectionInformation?.IsInternetAvailable ?? false;

        /// <summary>
        /// Checks the current connection information and raises <see cref="NetworkChanged"/> if needed.
        /// </summary>
        private void UpdateConnectionInformation()
        {
            lock (ConnectionInformation)
            {
                try
                {
                    ConnectionInformation.UpdateConnectionInformation(NetworkInformation.GetInternetConnectionProfile());

                    NetworkChanged?.Invoke(this, EventArgs.Empty);
                }
                catch
                {
                    ConnectionInformation.Reset();
                }
            }
        }

        /// <summary>
        /// Invokes <see cref="UpdateConnectionInformation"/> when the current network status changes.
        /// </summary>
        private void OnNetworkStatusChanged(object sender)
        {
            UpdateConnectionInformation();
        }
    }
}
