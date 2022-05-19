// Copyright (c) Richasy. All rights reserved.

namespace Bili.Models.Enums.App
{
    /// <summary>
    /// 后退行为.
    /// </summary>
    public enum BackBehavior
    {
        /// <summary>
        /// 导航到主界面.
        /// </summary>
        MainView,

        /// <summary>
        /// 导航到了二级页面.
        /// </summary>
        SecondaryView,

        /// <summary>
        /// 打开了播放器.
        /// </summary>
        OpenPlayer,

        /// <summary>
        /// 打开了浮窗.
        /// </summary>
        ShowHolder,

        /// <summary>
        /// 播放器模式更改.
        /// </summary>
        PlayerModeChange,
    }
}
