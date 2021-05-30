// Copyright (c) Richasy. All rights reserved.

using Richasy.Bili.Models.Enums;

namespace Richasy.Bili.Toolkit.Interfaces
{
    /// <summary>
    /// Resource related toolkits, including text resources, styles, etc.
    /// </summary>
    public interface IResourceToolkit
    {
        /// <summary>
        /// Get localized text.
        /// </summary>
        /// <param name="languageName">Resource name corresponding to localized text.</param>
        /// <returns>Localized text.</returns>
        string GetLocaleString(LanguageNames languageName);

        /// <summary>
        /// Get the specified resource.
        /// </summary>
        /// <typeparam name="T">Resource type.</typeparam>
        /// <param name="resourceName">Resource name.</param>
        /// <returns>Specified type of resource.</returns>
        T GetResource<T>(string resourceName);
    }
}
