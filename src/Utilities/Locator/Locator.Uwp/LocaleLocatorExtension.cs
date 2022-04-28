// Copyright (c) Richasy. All rights reserved.

using Bili.Models.Enums;
using Bili.Toolkit.Interfaces;
using Windows.UI.Xaml.Markup;

namespace Bili.Locator.Uwp
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
