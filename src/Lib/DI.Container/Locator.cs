// Copyright (c) Richasy. All rights reserved.

using System;
using Microsoft.Extensions.DependencyInjection;

namespace Bili.DI.Container
{
    /// <summary>
    /// 服务定位器.
    /// </summary>
    public sealed class Locator
    {
        private readonly IServiceCollection _serviceDescriptors;

        private Locator() => _serviceDescriptors = new ServiceCollection();

        /// <summary>
        /// DI 容器实例.
        /// </summary>
        public static Locator Instance { get; } = new Locator();

        /// <summary>
        /// 服务注册提供器.
        /// </summary>
        public IServiceProvider Provider { get; private set; }

        /// <summary>
        /// 注册单例.
        /// </summary>
        /// <typeparam name="TInterface">单例接口.</typeparam>
        /// <typeparam name="TImplementation">单例实现.</typeparam>
        /// <returns>定位器.</returns>
        public Locator RegisterSingleton<TInterface, TImplementation>()
            where TInterface : class
            where TImplementation : class, TInterface
        {
            _serviceDescriptors.AddSingleton<TInterface, TImplementation>();
            return this;
        }

        /// <summary>
        /// 注册单例.
        /// </summary>
        /// <param name="implementation">接口的实现实例.</param>
        /// <typeparam name="TInterface">单例接口.</typeparam>
        /// <returns>定位器.</returns>
        public Locator RegisterSingleton<TInterface>(TInterface implementation)
            where TInterface : class
        {
            _serviceDescriptors.AddSingleton(implementation);
            return this;
        }

        /// <summary>
        /// 注册单例.
        /// </summary>
        /// <param name="data">常量数据.</param>
        /// <returns>定位器.</returns>
        public Locator RegisterConstant(object data)
        {
            _serviceDescriptors.AddSingleton(data);
            return this;
        }

        /// <summary>
        /// 注册瞬态.
        /// </summary>
        /// <typeparam name="TInterface">瞬态接口.</typeparam>
        /// <typeparam name="TImplementation">瞬态实现.</typeparam>
        /// <returns>定位器.</returns>
        public Locator RegisterTransient<TInterface, TImplementation>()
            where TInterface : class
            where TImplementation : class, TInterface
        {
            _serviceDescriptors.AddTransient<TInterface, TImplementation>();
            return this;
        }

        /// <summary>
        /// 构建服务提供器，使注册的服务生效.
        /// </summary>
        public void Build()
            => Provider = _serviceDescriptors.BuildServiceProvider();

        /// <summary>
        /// 获取注册的服务.
        /// </summary>
        /// <typeparam name="T">服务注册标识.</typeparam>
        /// <returns>注册的服务.</returns>
        public T GetService<T>()
            => Provider.GetService<T>();
    }
}
