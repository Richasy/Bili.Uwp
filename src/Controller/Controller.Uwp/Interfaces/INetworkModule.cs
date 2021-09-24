// Copyright (c) GodLeaveMe. All rights reserved.

using System;
using Richasy.Bili.Controller.Uwp.Utilities;

namespace Richasy.Bili.Controller.Uwp.Interfaces
{
    /// <summary>
    /// 网络连接检测模块的定义.
    /// </summary>
    public interface INetworkModule
    {
        /// <summary>
        /// 当网络改变时发生.
        /// </summary>
        event EventHandler NetworkChanged;

        /// <summary>
        /// <see cref="Utilities.ConnectionInformation"/> 的实例.
        /// </summary>
        ConnectionInformation ConnectionInformation { get; }

        /// <summary>
        /// 获取网络是否可用.
        /// </summary>
        bool IsNetworkAvaliable { get; }
    }
}
