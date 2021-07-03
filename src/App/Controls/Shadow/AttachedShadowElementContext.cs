// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using System.Numerics;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Hosting;

namespace Richasy.Bili.App.Controls.Shadow
{
    public sealed class AttachedShadowElementContext
    {
        private bool _isConnected;

        private Dictionary<string, object> _resources;

        /// <summary>
        /// Get whether or not this <see cref="AttachedShadowElementContext"/> has been initialized
        /// </summary>
        public bool IsInitialized { get; private set; }

        /// <summary>
        /// Get the <see cref="IAttachedShadow"/> that contains this <see cref="AttachedShadowElementContext"/>
        /// </summary>
        public IAttachedShadow Parent { get; private set; }

        /// <summary>
        /// Get the <see cref="FrameworkElement"/> this instance is attached to
        /// </summary>
        public FrameworkElement Element { get; private set; }

        /// <summary>
        /// Get the <see cref="Visual"/> for the <see cref="FrameworkElement"/> this instance is attached to
        /// </summary>
        public Visual ElementVisual { get; private set; }

        /// <summary>
        /// Get the <see cref="Windows.UI.Composition.Compositor"/> for this instance
        /// </summary>
        public Compositor Compositor { get; private set; }

        /// <summary>
        /// Get the <see cref="SpriteVisual"/> that contains the <see cref="DropShadow">shadow</see> for this instance
        /// </summary>
        public SpriteVisual SpriteVisual { get; private set; }

        /// <summary>
        /// Get the <see cref="DropShadow"/> that is rendered on this instance's <see cref="Element"/>
        /// </summary>
        public DropShadow Shadow { get; private set; }

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
        /// Force early creation of this instance's resources, otherwise they will be created automatically when <see cref="Element"/> is loaded.
        /// </summary>
        public void CreateResources() => Initialize(true);

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

        /// <summary>
        /// Adds a resource to this instance's resource dictionary with the specified key
        /// </summary>
        /// <returns>The resource that was added</returns>
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
        /// Retrieves a resource with the specified key and type if it exists
        /// </summary>
        /// <returns>True if the resource exists, false otherwise</returns>
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
        /// Retries a resource with the specified key and type
        /// </summary>
        public T GetResource<T>(string key)
        {
            if (TryGetResource(key, out T resource))
            {
                return resource;
            }

            return default;
        }

        /// <summary>
        /// Removes an existing resource with the specified key and type
        /// </summary>
        /// <returns>The resource that was removed, if any</returns>
        public T RemoveResource<T>(string key)
        {
            if (_resources.TryGetValue(key, out var objResource))
            {
                _resources.Remove(key);
                if (objResource is T resource)
                {
                    return resource;
                }
            }

            return default;
        }

        /// <summary>
        /// Removes an existing resource with the specified key and type, and <see cref="IDisposable.Dispose">disposes</see> it
        /// </summary>
        /// <returns>The resource that was removed, if any</returns>
        public T RemoveAndDisposeResource<T>(string key) where T : IDisposable
        {
            if (_resources.TryGetValue(key, out var objResource))
            {
                _resources.Remove(key);
                if (objResource is T resource)
                {
                    resource.Dispose();
                    return resource;
                }
            }

            return default;
        }

        /// <summary>
        /// Adds a resource to this instance's collection with the specified key
        /// </summary>
        /// <returns>The resource that was added</returns>
        public T AddResource<T>(TypedResourceKey<T> key, T resource) => AddResource(key.Key, resource);

        /// <summary>
        /// Retrieves a resource with the specified key and type if it exists
        /// </summary>
        /// <returns>True if the resource exists, false otherwise</returns>
        public bool TryGetResource<T>(TypedResourceKey<T> key, out T resource) => TryGetResource(key.Key, out resource);

        /// <summary>
        /// Retries a resource with the specified key and type
        /// </summary>
        public T GetResource<T>(TypedResourceKey<T> key) => GetResource<T>(key.Key);

        /// <summary>
        /// Removes an existing resource with the specified key and type
        /// </summary>
        /// <returns>The resource that was removed, if any</returns>
        public T RemoveResource<T>(TypedResourceKey<T> key) => RemoveResource<T>(key.Key);

        /// <summary>
        /// Removes an existing resource with the specified key and type, and <see cref="IDisposable.Dispose">disposes</see> it
        /// </summary>
        /// <returns>The resource that was removed, if any</returns>
        public T RemoveAndDisposeResource<T>(TypedResourceKey<T> key) where T : IDisposable => RemoveAndDisposeResource<T>(key.Key);

        /// <summary>
        /// Disposes of any resources that implement <see cref="IDisposable"/> and then clears all resources
        /// </summary>
        public void ClearAndDisposeResources()
        {
            foreach (var kvp in _resources)
            {
                (kvp.Value as IDisposable)?.Dispose();
            }
            _resources.Clear();
        }
    }
}
