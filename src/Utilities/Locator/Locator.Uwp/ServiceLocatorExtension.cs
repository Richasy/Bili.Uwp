// Copyright (c) Richasy. All rights reserved.

using Microsoft.Extensions.DependencyInjection;

namespace Bili.Locator.Uwp
{
    /// <summary>
    /// <see cref="ServiceLocator"/> extension.
    /// </summary>
    public static class ServiceLocatorExtension
    {
        /// <summary>
        /// Rebuild <see cref="ServiceProvider"/> and assign it to <see cref="ServiceLocator.ServiceProvider"/>.
        /// </summary>
        /// <param name="serviceCollection">Instance of <see cref="IServiceCollection"/>.</param>
        public static void RebuildServiceProvider(this IServiceCollection serviceCollection)
        {
            if (ServiceLocator.Instance != null)
            {
                ServiceLocator.Instance.ServiceProvider = serviceCollection.BuildServiceProvider();
            }
        }
    }
}
