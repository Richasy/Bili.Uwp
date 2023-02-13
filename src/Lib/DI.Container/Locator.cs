// Copyright (c) Richasy. All rights reserved.

using System;
using Autofac;
using NLog;

namespace Bili.DI.Container
{
    /// <summary>
    /// 服务定位器.
    /// </summary>
    public sealed class Locator
    {
        private static IContainer _container;
        private static ContainerBuilder _containerBuilder;

        private Locator()
            => _containerBuilder = new ContainerBuilder();

        /// <summary>
        /// DI 容器实例.
        /// </summary>
        public static Locator Instance { get; } = new Lazy<Locator>(() => new Locator()).Value;

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
            _containerBuilder.RegisterType<TImplementation>()
                .As<TInterface>()
                .SingleInstance();
            return this;
        }

        /// <summary>
        /// 注册单例.
        /// </summary>
        /// <param name="data">常量数据.</param>
        /// <returns>定位器.</returns>
        public Locator RegisterConstant(object data)
        {
            _containerBuilder.RegisterInstance(data);
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
            _containerBuilder.RegisterType<TImplementation>()
                .As<TInterface>();
            return this;
        }

        /// <summary>
        /// 构建服务提供器，使注册的服务生效.
        /// </summary>
        public void Build()
            => _container = _containerBuilder.Build();

        /// <summary>
        /// 获取注册的服务.
        /// </summary>
        /// <typeparam name="T">服务注册标识.</typeparam>
        /// <returns>注册的服务.</returns>
        public T GetService<T>()
            => _container.Resolve<T>();

        /// <summary>
        /// 获取日志记录器.
        /// </summary>
        /// <returns><see cref="ILogger"/>的实例.</returns>
        public ILogger GetLogger()
            => LogManager.GetLogger("Richasy.Bili");
    }
}
