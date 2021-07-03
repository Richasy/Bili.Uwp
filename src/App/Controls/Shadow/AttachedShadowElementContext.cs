// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using System.Numerics;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Hosting;

namespace Richasy.Bili.App.Controls.Shadow
{
    /// <summary>
    /// 附加阴影的元素上下文定义.
    /// </summary>
    public sealed class AttachedShadowElementContext
    {
        private bool _isConnected;

        private Dictionary<string, object> _resources;

        /// <summary>
        /// 获取当前上下文是否已经加载完成.
        /// </summary>
        public bool IsInitialized { get; private set; }

        /// <summary>
        /// 获取包含当前上下文的<see cref="IAttachedShadow"/>.
        /// </summary>
        public IAttachedShadow Parent { get; private set; }

        /// <summary>
        /// 获取附加的目标元素.
        /// </summary>
        public FrameworkElement Element { get; private set; }

        /// <summary>
        /// 获取附加的目标元素的视觉层.
        /// </summary>
        public Visual ElementVisual { get; private set; }

        /// <summary>
        /// 获取当前实例的<see cref="Windows.UI.Composition.Compositor"/>.
        /// </summary>
        public Compositor Compositor { get; private set; }

        /// <summary>
        /// 获取包含<see cref="DropShadow"/>的<see cref="Windows.UI.Composition.SpriteVisual"/>层.
        /// </summary>
        public SpriteVisual SpriteVisual { get; private set; }

        /// <summary>
        /// 获取当前目标元素的<see cref="DropShadow"/>.
        /// </summary>
        public DropShadow Shadow { get; private set; }

        /// <summary>
        /// 连接到UI元素.
        /// </summary>
        /// <param name="parent">附加的阴影.</param>
        /// <param name="element">目标元素.</param>
        public void ConnectToElement(IAttachedShadow parent, FrameworkElement element)
        {
            if (_isConnected)
            {
                throw new InvalidOperationException("This AttachedShadowElementContext has already been connected to an element");
            }

            _isConnected = true;
            Parent = parent ?? throw new ArgumentNullException(nameof(parent));
            Element = element ?? throw new ArgumentNullException(nameof(element));
            Element.Loaded += OnElementLoaded;
            Element.Unloaded += OnElementUnloaded;
            Initialize();
        }

        /// <summary>
        /// 与UI元素断开链接.
        /// </summary>
        public void DisconnectFromElement()
        {
            if (!_isConnected)
            {
                return;
            }

            Uninitialize();

            Element.Loaded -= OnElementLoaded;
            Element.Unloaded -= OnElementUnloaded;
            Element = null;

            Parent = null;

            _isConnected = false;
        }

        /// <summary>
        /// 强制提前创建此实例的资源，否则它们将在加载 <see cref="Element"/> 时自动创建.
        /// </summary>
        public void CreateResources() => Initialize(true);

        /// <summary>
        /// 使用指定的键将资源添加到此实例的资源字典中.
        /// </summary>
        /// <param name="key">资源的键.</param>
        /// <param name="resource">资源值.</param>
        /// <typeparam name="T">资源类型.</typeparam>
        /// <returns>已被添加的资源.</returns>
        public T AddResource<T>(string key, T resource)
        {
            _resources = _resources ?? new Dictionary<string, object>();
            if (_resources.ContainsKey(key))
            {
                _resources[key] = resource;
            }
            else
            {
                _resources.Add(key, resource);
            }

            return resource;
        }

        /// <summary>
        /// 检索具有指定键和类型的资源（如果存在）.
        /// </summary>
        /// <param name="key">资源键.</param>
        /// <param name="resource">资源值.</param>
        /// <typeparam name="T">资源类型.</typeparam>
        /// <returns>如果资源存在则为<c>True</c>，否则为<c>False</c>.</returns>
        public bool TryGetResource<T>(string key, out T resource)
        {
            if (_resources != null && _resources.TryGetValue(key, out var objResource) && objResource is T tResource)
            {
                resource = tResource;
                return true;
            }

            resource = default;
            return false;
        }

        /// <summary>
        /// 获取具有指定键和类型的资源.
        /// </summary>
        /// <param name="key">资源键.</param>
        /// <typeparam name="T">资源类型.</typeparam>
        /// <returns>返回的资源.</returns>
        public T GetResource<T>(string key)
        {
            if (TryGetResource(key, out T resource))
            {
                return resource;
            }

            return default;
        }

        /// <summary>
        /// 移除具有指定键的资源.
        /// </summary>
        /// <param name="key">资源键.</param>
        /// <returns>被移除的资源.</returns>
        public object RemoveResource(string key)
        {
            if (_resources.TryGetValue(key, out var objResource))
            {
                _resources.Remove(key);
                return objResource;
            }

            return default;
        }

        /// <summary>
        /// 移除具有指定键的资源，如果该资源可被释放，则释放它.
        /// </summary>
        /// <param name="key">资源键.</param>
        /// <returns>被移除的资源.</returns>
        public object RemoveAndDisposeResource(string key)
        {
            if (_resources.TryGetValue(key, out var objResource))
            {
                _resources.Remove(key);
                if (objResource is IDisposable resource)
                {
                    resource.Dispose();
                    return resource;
                }
            }

            return default;
        }

        /// <summary>
        /// 清除全部资源，如果某资源可被释放，则在清除过程中释放它.
        /// </summary>
        public void ClearAndDisposeResources()
        {
            foreach (var kvp in _resources)
            {
                (kvp.Value as IDisposable)?.Dispose();
            }

            _resources.Clear();
        }

        private void Initialize(bool forceIfNotLoaded = false)
        {
            if (IsInitialized || !_isConnected || (!Element.IsLoaded && !forceIfNotLoaded))
            {
                return;
            }

            IsInitialized = true;

            ElementVisual = ElementCompositionPreview.GetElementVisual(Element);
            Compositor = ElementVisual.Compositor;

            Shadow = Compositor.CreateDropShadow();

            SpriteVisual = Compositor.CreateSpriteVisual();
            SpriteVisual.RelativeSizeAdjustment = Vector2.One;
            SpriteVisual.Shadow = Shadow;

            if (Parent.SupportsOnSizeChangedEvent)
            {
                Element.SizeChanged += OnElementSizeChanged;
            }

            Parent?.OnElementContextInitialized(this);
        }

        private void Uninitialize()
        {
            if (!IsInitialized)
            {
                return;
            }

            IsInitialized = false;

            Parent.OnElementContextUninitialized(this);

            SpriteVisual.Shadow = null;
            SpriteVisual.Dispose();

            Shadow.Dispose();

            ElementCompositionPreview.SetElementChildVisual(Element, null);

            Element.SizeChanged -= OnElementSizeChanged;

            SpriteVisual = null;
            Shadow = null;
            ElementVisual = null;
        }

        private void OnElementUnloaded(object sender, RoutedEventArgs e)
        {
            Uninitialize();
        }

        private void OnElementLoaded(object sender, RoutedEventArgs e)
        {
            Initialize();
        }

        private void OnElementSizeChanged(object sender, SizeChangedEventArgs e)
        {
            Parent?.OnSizeChanged(this, e.NewSize, e.PreviousSize);
        }
    }
}
