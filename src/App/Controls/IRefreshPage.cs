// Copyright (c) GodLeaveMe. All rights reserved.

using System.Threading.Tasks;

namespace Richasy.Bili.App.Controls
{
    /// <summary>
    /// 可以刷新的页面.
    /// </summary>
    public interface IRefreshPage
    {
        /// <summary>
        /// 刷新.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        Task RefreshAsync();
    }
}
