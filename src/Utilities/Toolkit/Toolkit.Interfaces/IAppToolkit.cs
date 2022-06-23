// Copyright (c) Richasy. All rights reserved.

using System;

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
        /// <returns>Toolkit self.</returns>
        IAppToolkit InitializeTitleBar();

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
    }
}
