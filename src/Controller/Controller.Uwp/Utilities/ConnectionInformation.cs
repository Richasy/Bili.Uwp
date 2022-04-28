// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using Windows.Networking.Connectivity;

namespace Bili.Controller.Uwp.Utilities
{
    /// <summary>
    /// This class exposes information about the network connectivity.
    /// </summary>
    public class ConnectionInformation
    {
        private readonly List<string> networkNames = new List<string>();

        /// <summary>
        /// Gets a value indicating whether if the current internet connection is metered.
        /// </summary>
        public bool IsInternetOnMeteredConnection
        {
            get
            {
                return ConnectionCost != null && ConnectionCost.NetworkCostType != NetworkCostType.Unrestricted;
            }
        }

        /// <summary>
        /// Gets a value indicating whether internet is available across all connections.
        /// </summary>
        /// <returns>True if internet can be reached.</returns>
        public bool IsInternetAvailable { get; private set; }

        /// <summary>
        /// Gets connectivity level for the current Internet Connection Profile.
        /// </summary>
        /// <returns>value of <see cref="NetworkConnectivityLevel"/>.</returns>
        public NetworkConnectivityLevel ConnectivityLevel { get; private set; }

        /// <summary>
        /// Gets connection cost for the current Internet Connection Profile.
        /// </summary>
        /// <returns>value of <see cref="NetworkConnectivityLevel"/>.</returns>
        public ConnectionCost ConnectionCost { get; private set; }

        /// <summary>
        /// Gets signal strength for the current Internet Connection Profile.
        /// </summary>
        /// <returns>value of <see cref="NetworkConnectivityLevel"/>.</returns>
        public byte? SignalStrength { get; private set; }

        /// <summary>
        /// Gets signal strength for the current Internet Connection Profile.
        /// </summary>
        /// <returns>value of <see cref="NetworkConnectivityLevel"/>.</returns>
        public IReadOnlyList<string> NetworkNames
        {
            get
            {
                return networkNames.AsReadOnly();
            }
        }

        /// <summary>
        /// Updates  the current object based on profile passed.
        /// </summary>
        /// <param name="profile">instance of <see cref="ConnectionProfile"/>.</param>
        public void UpdateConnectionInformation(ConnectionProfile profile)
        {
            if (profile == null)
            {
                Reset();

                return;
            }

            networkNames.Clear();

            var names = profile.GetNetworkNames();
            if (names?.Count > 0)
            {
                networkNames.AddRange(names);
            }

            ConnectivityLevel = profile.GetNetworkConnectivityLevel();

            switch (ConnectivityLevel)
            {
                case NetworkConnectivityLevel.None:
                case NetworkConnectivityLevel.LocalAccess:
                    IsInternetAvailable = false;
                    break;

                default:
                    IsInternetAvailable = true;
                    break;
            }

            ConnectionCost = profile.GetConnectionCost();
            SignalStrength = profile.GetSignalBars();
        }

        /// <summary>
        /// Resets the current object to default values.
        /// </summary>
        internal void Reset()
        {
            networkNames.Clear();

            ConnectivityLevel = NetworkConnectivityLevel.None;
            IsInternetAvailable = false;
            ConnectionCost = null;
            SignalStrength = null;
        }
    }
}
