// Copyright (c) Richasy. All rights reserved.

using System;
using System.Drawing;

namespace Bili.Toolkit.Interfaces
{
    /// <summary>
    /// Application related toolkit.
    /// </summary>
    public interface IAppToolkit
    {
        /// <summary>
        /// Initialize application theme,
        /// this method is used to switch to the specified theme when the application starts.
        /// </summary>
        /// <returns>Toolkit self.</returns>
        IAppToolkit InitializeTheme();

        /// <summary>
        /// Initialize application title bar style,
        /// this method is used to rewrite default title bar style.
        /// </summary>
        /// <param name="titleBar">If there is a title bar object to pass in (for WinUI).</param>
        /// <returns>Toolkit self.</returns>
        IAppToolkit InitializeTitleBar(object titleBar);

        /// <summary>
        /// Get the current environment language code.
        /// </summary>
        /// <param name="isWindowsName">
        /// Whether it is the Windows display name,
        /// for example, Simplified Chinese is CHS,
        /// if not, it is displayed as the default name,
        /// for example, Simplified Chinese is zh-Hans.
        /// </param>
        /// <returns>Language code.</returns>
        string GetLanguageCode(bool isWindowsName = false);

        /// <summary>
        /// 获取应用设置的代理以及对应内容的区域.
        /// </summary>
        /// <param name="title">视频标题.</param>
        /// <param name="isVideo">是否为 UGC 视频.</param>
        /// <returns>代理及区域.</returns>
        Tuple<string, string> GetProxyAndArea(string title, bool isVideo);

        /// <summary>
        /// 获取应用包版本.
        /// </summary>
        /// <returns>包版本.</returns>
        string GetPackageVersion();

        /// <summary>
        /// 获取窗口图标路径（对 WinUI）.
        /// </summary>
        /// <returns>图标路径.</returns>
        string GetWindowIconPath();

        /// <summary>
        /// Initialize a window that is always on top.
        /// </summary>
        /// <param name="window">Window object.</param>
        /// <param name="width">Window width.</param>
        /// <param name="height">Window height.</param>
        /// <param name="title">Window title.</param>
        void InitializeAotWindow(object window, double width, double height, string title = "");

        /// <summary>
        /// Confirm that there is a DispatcherQueue when the application calls related methods (such as setting the Mica background),
        /// if not, create one.
        /// </summary>
        void EnsureWindowsSystemDispatcherQueueController();

        /// <summary>
        /// Resize and center the window.
        /// </summary>
        /// <param name="width">Window width.</param>
        /// <param name="height">Window height.</param>
        /// <param name="windowHandle">Window handle.</param>
        void ResizeAndCenterWindow(double width, double height, IntPtr windowHandle);

        /// <summary>
        /// Get the magnified pixels under the DPI of the current window.
        /// </summary>
        /// <param name="pixel">Pixel value.</param>
        /// <param name="windowHandle">Window handle.</param>
        /// <returns>Converted value.</returns>
        int GetScalePixel(double pixel, IntPtr windowHandle);

        /// <summary>
        /// Get the monitor size on which the app is located.
        /// </summary>
        /// <param name="windowHandle">Window handle.</param>
        /// <returns>Monitor size.</returns>
        Size GetMonitorSize(IntPtr windowHandle);
    }
}
