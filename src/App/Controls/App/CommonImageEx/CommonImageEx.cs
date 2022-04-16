// Copyright (c) Richasy. All rights reserved.

using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using HN.Controls;
using Richasy.Bili.ViewModels.Uwp;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace Richasy.Bili.App.Controls
{
    /// <summary>
    /// 通用图片扩展.
    /// </summary>
    public sealed class CommonImageEx : Control
    {
        /// <summary>
        /// <see cref="ImageUrl"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty ImageUrlProperty =
            DependencyProperty.Register(nameof(ImageUrl), typeof(string), typeof(CommonImageEx), new PropertyMetadata(null, new PropertyChangedCallback(OnImageUrlChangedAsync)));

        /// <summary>
        /// <see cref="Stretch"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty StretchProperty =
            DependencyProperty.Register(nameof(Stretch), typeof(Stretch), typeof(CommonImageEx), new PropertyMetadata(Stretch.UniformToFill));

        /// <summary>
        /// <see cref="RetryCount"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty RetryCountProperty =
            DependencyProperty.Register(nameof(RetryCount), typeof(int), typeof(CommonImageEx), new PropertyMetadata(2));

        private Image _image;
        private CancellationTokenSource _loadTokenSource;
        private IRandomAccessStream _imgStream;
        private bool _isInitializing;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommonImageEx"/> class.
        /// </summary>
        public CommonImageEx()
        {
            DefaultStyleKey = typeof(CommonImageEx);
            Loaded += OnLoadedAsync;
            Unloaded += OnUnload;
        }

        /// <summary>
        /// 图片地址.
        /// </summary>
        public string ImageUrl
        {
            get { return (string)GetValue(ImageUrlProperty); }
            set { SetValue(ImageUrlProperty, value); }
        }

        /// <summary>
        /// 图片拉伸.
        /// </summary>
        public Stretch Stretch
        {
            get { return (Stretch)GetValue(StretchProperty); }
            set { SetValue(StretchProperty, value); }
        }

        /// <summary>
        /// 获取数据失败后的重试次数.
        /// </summary>
        public int RetryCount
        {
            get { return (int)GetValue(RetryCountProperty); }
            set { SetValue(RetryCountProperty, value); }
        }

        /// <inheritdoc/>
        protected override void OnApplyTemplate()
            => _image = GetTemplateChild("Image") as Image;

        private static async void OnImageUrlChangedAsync(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as CommonImageEx;
            if (e.NewValue != null && e.OldValue != null && e.NewValue != e.OldValue)
            {
                await instance.LoadImageAsync();
            }
        }

        private async void OnLoadedAsync(object sender, RoutedEventArgs e)
        {
            if (!_isInitializing)
            {
                await LoadImageAsync();
            }
        }

        private void OnUnload(object sender, RoutedEventArgs e) => ClearCache();

        private async Task LoadImageAsync()
        {
            ClearCache();
            if (_image == null || string.IsNullOrEmpty(ImageUrl))
            {
                return;
            }

            _loadTokenSource = new CancellationTokenSource();
            var bitmapImage = new BitmapImage();
            _image.Source = bitmapImage;
            var url = ImageUrl;
            _isInitializing = true;
            await Task.Run(async () =>
            {
                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync(url, _loadTokenSource.Token);
                    if (response.IsSuccessStatusCode)
                    {
                        var httpStream = await response.Content.ReadAsStreamAsync();
                        _imgStream = httpStream.AsRandomAccessStream();
                    }
                }
            }).ContinueWith(async t =>
            {
                if (t.IsCompletedSuccessfully)
                {
                    await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
                    {
                        if (_imgStream != null
                            && _imgStream.CanRead
                            && (!_loadTokenSource?.IsCancellationRequested ?? false))
                        {
                            _imgStream.Seek(0);
                            await bitmapImage.SetSourceAsync(_imgStream).AsTask();
                        }
                    });
                }
            }).ContinueWith(t =>
            {
                if (!t.IsCompletedSuccessfully)
                {
                }

                _isInitializing = false;
            });
        }

        private void ClearCache()
        {
            if (_loadTokenSource != null && !_loadTokenSource.IsCancellationRequested)
            {
                _loadTokenSource?.Cancel();
                _loadTokenSource?.Dispose();
                _loadTokenSource = null;
            }

            if (_imgStream != null)
            {
                _imgStream?.Dispose();
                _imgStream = null;
            }

            _image.Source = null;
            _isInitializing = false;
        }
    }
}
