// Copyright (c) Richasy. All rights reserved.

using Bili.Models.Enums;
using Bili.Toolkit.Interfaces;

namespace Bili.Toolkit.Fake
{
    /// <summary>
    /// 不包含有效返回值的 <see cref="IResourceToolkit"/> 实现.
    /// </summary>
    public sealed class FakeResourceToolkit : IResourceToolkit
    {
        /// <inheritdoc/>
        public string GetLocaleString(LanguageNames languageName)
            => string.Empty;

        /// <inheritdoc/>
        public T GetResource<T>(string resourceName)
            => default;
    }
}
