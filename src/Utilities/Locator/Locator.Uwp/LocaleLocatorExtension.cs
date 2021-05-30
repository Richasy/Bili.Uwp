// Copyright (c) Richasy. All rights reserved.

using Richasy.Bili.Models.Enums;
using Richasy.Bili.Toolkit.Interfaces;
using Windows.UI.Xaml.Markup;

namespace Richasy.Bili.Locator.Uwp
{
    /// <summary>
    /// Localized text extension.
    /// </summary>
    [MarkupExtensionReturnType(ReturnType = typeof(string))]
    public sealed class LocaleLocatorExtension : MarkupExtension
    {
        /// <summary>
        /// Language name.
        /// </summary>
        public LanguageNames Name { get; set; }

        /// <inheritdoc/>
        protected override object ProvideValue()
        {
            return ServiceLocator.Instance.GetService<IResourceToolkit>()
                                          .GetLocaleString(this.Name);
        }
    }
}
