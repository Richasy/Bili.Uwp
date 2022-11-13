// Copyright (c) Richasy. All rights reserved.

using Bili.DI.Container;
using Bili.Models.Enums;
using Bili.Toolkit.Interfaces;
using Windows.UI.Xaml.Markup;

namespace Bili.Uwp.App.Resources.Extension
{
    /// <summary>
    /// Localized text extension.
    /// </summary>
    [MarkupExtensionReturnType(ReturnType = typeof(string))]
    public sealed class LocaleExtension : MarkupExtension
    {
        /// <summary>
        /// Language name.
        /// </summary>
        public LanguageNames Name { get; set; }

        /// <inheritdoc/>
        protected override object ProvideValue()
        {
            return Locator.Instance.GetService<IResourceToolkit>()
                                          .GetLocaleString(Name);
        }
    }
}
