// Copyright (c) GodLeaveMe. All rights reserved.

namespace Richasy.Bili.Toolkit.Interfaces
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
    }
}
