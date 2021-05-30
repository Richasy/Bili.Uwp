// Copyright (c) Richasy. All rights reserved.

using System;
using Microsoft.Extensions.DependencyInjection;

namespace Richasy.Bili.Locator.Uwp
{
    /// <summary>
    /// Service locator, used to obtain the container for dependency injection.
    /// </summary>
    public class ServiceLocator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceLocator"/> class.
        /// </summary>
        /// <param name="serviceCollection">Service provider instance.</param>
        public ServiceLocator(IServiceCollection serviceCollection)
        {
            this.ServiceCollection = serviceCollection;
            this.ServiceProvider = serviceCollection.BuildServiceProvider();
            Instance = this;
        }

        /// <summary>
        /// Instance of <see cref="ServiceLocator"/>.
        /// </summary>
        public static ServiceLocator Instance { get; private set; }

        /// <summary>
        /// Service provider instance.
        /// </summary>
        public IServiceProvider ServiceProvider { get; set; }

        /// <summary>
        /// Service collection instance.
        /// </summary>
        public IServiceCollection ServiceCollection { get; }

        /// <summary>
        /// Get registered service.
        /// </summary>
        /// <typeparam name="T">Service registration type.</typeparam>
        /// <returns>Service.</returns>
        public T GetService<T>()
        {
            if (ServiceProvider != null)
            {
                return ServiceProvider.GetService<T>();
            }

            return default;
        }

        /// <summary>
        /// Try to load the service.
        /// </summary>
        /// <typeparam name="T">Service registration type.</typeparam>
        /// <param name="defineService">Definition of need to load the service.</param>
        /// <returns>Whether the loading is successful.</returns>
        public ServiceLocator LoadService<T>(out T defineService)
        {
            if (ServiceProvider == null)
            {
                defineService = default;
            }
            else
            {
                var service = GetService<T>();
                defineService = service;
            }

            return this;
        }
    }
}
