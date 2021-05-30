// Copyright (c) Richasy. All rights reserved.

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
    }
}
