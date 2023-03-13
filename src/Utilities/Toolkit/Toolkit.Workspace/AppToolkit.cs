// Copyright (c) Richasy. All rights reserved.

using System;
using System.Globalization;
using Bili.Toolkit.Interfaces;
using Windows.ApplicationModel;

namespace Bili.Toolkit.Workspace
{
    /// <summary>
    /// 应用工具组.
    /// </summary>
    public sealed class AppToolkit : IAppToolkit
    {
        /// <inheritdoc/>
        public string GetLanguageCode(bool isWindowsName = false)
        {
            var culture = CultureInfo.CurrentUICulture;
            return isWindowsName ? culture.ThreeLetterWindowsLanguageName : culture.Name;
        }

        /// <inheritdoc/>
        public string GetPackageVersion()
        {
            var appVersion = Package.Current.Id.Version;
            return $"{appVersion.Major}.{appVersion.Minor}.{appVersion.Build}.{appVersion.Revision}";
        }

        /// <inheritdoc/>
        public Tuple<string, string> GetProxyAndArea(string title, bool isVideo)
            => throw new NotImplementedException();

        /// <inheritdoc/>
        public IAppToolkit InitializeTheme()
            => throw new NotImplementedException();

        /// <inheritdoc/>
        public IAppToolkit InitializeTitleBar()
            => throw new NotImplementedException();
    }
}
