// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Uwp.Core;
using Splat;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.System.UserProfile;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Imaging;

namespace Bili.App.Controls
{
    /// <summary>
    /// 图片查看器.
    /// </summary>
    public sealed partial class ImageViewer : UserControl
    {
        private readonly Dictionary<string, byte[]> _images;
        private int _currentIndex;
        private int _currentImageHeight;
        private bool _isControlShown;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageViewer"/> class.
        /// </summary>
        public ImageViewer()
        {
            InitializeComponent();
            _images = new Dictionary<string, byte[]>();
            Instance = this;
            Images = new ObservableCollection<Models.Data.Appearance.Image>();
        }

        /// <summary>
        /// 实例.
        /// </summary>
        public static ImageViewer Instance { get; private set; }

        /// <summary>
        /// 图片地址.
        /// </summary>
        public ObservableCollection<Models.Data.Appearance.Image> Images { get; }

        /// <summary>
        /// 加载图片.
        /// </summary>
        /// <param name="images">图片列表.</param>
        /// <param name="firstLoadImage">初始加载的图片索引.</param>
        /// <returns><see cref="Task"/>.</returns>
        public async Task LoadImagesAsync(IEnumerable<Models.Data.Appearance.Image> images, int firstLoadImage = 0)
        {
            Container.Visibility = Visibility.Visible;
            _images.Clear();
            Images.Clear();
            images.ToList().ForEach(url => Images.Add(url));
            FactoryBlock.Text = 1.ToString("p00");
            ShowControls();
            await ShowImageAsync(firstLoadImage);
        }

        /// <summary>
        /// 显示图片.
        /// </summary>
        /// <param name="index">图片索引.</param>
        /// <returns><see cref="Task"/>.</returns>
        public async Task ShowImageAsync(int index)
        {
            if (index >= 0 && Images.Count > index)
            {
                _currentIndex = index;
                if (ImageRepeater == null || !ImageRepeater.IsLoaded)
                {
                    await Task.Delay(200);
                }

                SetSelectedItem(index);
                await LoadImageAsync(Images[index]);
            }
        }

        private async Task LoadImageAsync(Models.Data.Appearance.Image image)
        {
            _currentImageHeight = 0;
            RotateTransform.Angle = 0;
            ImageScrollViewer.ChangeView(null, null, 1f);

            if (Image.Source != null)
            {
                Image.Source = null;
            }

            var imageUrl = image.GetSourceUri().ToString();
            var hasCache = _images.TryGetValue(imageUrl, out var imageBytes);

            if (!hasCache)
            {
                using var client = new HttpClient();
                imageBytes = await client.GetByteArrayAsync(imageUrl);

                // 避免重复多次请求下插入同源数据.
                if (!_images.ContainsKey(imageUrl))
                {
                    _images.Add(imageUrl, imageBytes);
                }
            }

            var bitmapImage = new BitmapImage();
            Image.Source = bitmapImage;
            var stream = new MemoryStream(imageBytes);
            await bitmapImage.SetSourceAsync(stream.AsRandomAccessStream());
            _currentImageHeight = bitmapImage.PixelHeight;

            UpdateLayout();
            var factor = ImageScrollViewer.ViewportHeight / _currentImageHeight;
            if (factor > 1)
            {
                factor = 1;
            }

            ImageScrollViewer.ChangeView(null, null, (float)factor);
        }

        private void CheckButtonStatus()
        {
            ZoomOutButton.IsEnabled = ImageScrollViewer.ZoomFactor > 0.2;
            ZoomInButton.IsEnabled = ImageScrollViewer.ZoomFactor < 1.5;
        }

        private void SetSelectedItem(int index)
        {
            if (Images.Count <= 1)
            {
                return;
            }

            for (var i = 0; i < Images.Count; i++)
            {
                var element = ImageRepeater.GetOrCreateElement(i);
                if (element is CardPanel panel)
                {
                    var image = panel.DataContext as Models.Data.Appearance.Image;
                    panel.IsEnableCheck = true;
                    panel.IsChecked = image == Images[index];
                    panel.IsEnableCheck = false;
                }
            }
        }

        private void OnScrollViewerTapped(object sender, TappedRoutedEventArgs e)
        {
            if (_isControlShown)
            {
                HideControls();
            }
            else
            {
                ShowControls();
            }
        }

        private void OnScrollViewerViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            if (e.IsIntermediate)
            {
                FactoryBlock.Text = ImageScrollViewer.ZoomFactor.ToString("p00");
                CheckButtonStatus();
            }
        }

        private void OnRotateButtonClick(object sender, Windows.UI.Xaml.RoutedEventArgs e)
            => RotateTransform.Angle += 90;

        private void OnZoomOutButtonClick(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (_currentImageHeight == 0)
            {
                return;
            }

            ImageScrollViewer.ChangeView(null, null, ImageScrollViewer.ZoomFactor - 0.1f);
        }

        private void OnZoomInButtonClick(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (_currentImageHeight == 0)
            {
                return;
            }

            ImageScrollViewer.ChangeView(null, null, ImageScrollViewer.ZoomFactor + 0.1f);
        }

        private async void OnImageItemClickAsync(object sender, RoutedEventArgs e)
        {
            var image = (sender as FrameworkElement).DataContext as Models.Data.Appearance.Image;
            var index = Images.IndexOf(image);
            await ShowImageAsync(index);
        }

        private async void OnNextButtonClickAsync(object sender, RoutedEventArgs e)
        {
            if (Images.Count - 1 <= _currentIndex)
            {
                return;
            }

            await ShowImageAsync(_currentIndex + 1);
        }

        private async void OnPrevButtonClickAsync(object sender, RoutedEventArgs e)
        {
            if (_currentIndex <= 0)
            {
                return;
            }

            await ShowImageAsync(_currentIndex - 1);
        }

        private void OnCopyButtonClickAysnc(object sender, RoutedEventArgs e)
        {
            if (_currentIndex < 0 || _currentIndex > Images.Count - 1)
            {
                return;
            }

            var image = Images[_currentIndex];
            var dp = new DataPackage();
            dp.SetBitmap(RandomAccessStreamReference.CreateFromUri(image.GetSourceUri()));
            Clipboard.SetContent(dp);
            var resourceToolkit = Locator.Current.GetService<IResourceToolkit>();
            Splat.Locator.Current.GetService<AppViewModel>().ShowTip(resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.Copied), Models.Enums.App.InfoType.Success);
        }

        private async void OnSaveButtonClickAsync(object sender, RoutedEventArgs e)
        {
            if (_currentIndex < 0 || _currentIndex > Images.Count - 1)
            {
                return;
            }

            var image = Images[_currentIndex];
            var imageUrl = image.GetSourceUri().ToString();
            var hasCache = _images.TryGetValue(imageUrl, out var cache);
            if (!hasCache)
            {
                return;
            }

            var savePicker = new FileSavePicker();
            savePicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            var fileName = Path.GetFileName(imageUrl);
            var extension = Path.GetExtension(imageUrl);
            savePicker.FileTypeChoices.Add($"{extension.TrimStart('.').ToUpper()} 图片", new string[] { extension });
            savePicker.SuggestedFileName = fileName;
            var file = await savePicker.PickSaveFileAsync();
            if (file != null)
            {
                await FileIO.WriteBytesAsync(file, cache);
                var resourceToolkit = Locator.Current.GetService<IResourceToolkit>();
                Splat.Locator.Current.GetService<AppViewModel>().ShowTip(resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.Saved), Models.Enums.App.InfoType.Success);
            }
        }

        private async void OnSettingToBackgroundClickAsync(object sender, RoutedEventArgs e)
            => await SetWallpaperOrLockScreenAsync(true);

        private async void OnSettingToLockScreenClickAsync(object sender, RoutedEventArgs e)
            => await SetWallpaperOrLockScreenAsync(false);

        private async Task SetWallpaperOrLockScreenAsync(bool isWallpaper)
        {
            if (_currentIndex < 0 || _currentIndex > Images.Count - 1)
            {
                return;
            }

            var image = Images[_currentIndex];
            var imageUrl = image.GetSourceUri().ToString();
            var hasCache = _images.TryGetValue(imageUrl, out var cache);
            if (!hasCache)
            {
                return;
            }

            var profileSettings = UserProfilePersonalizationSettings.Current;
            var fileName = Path.GetFileName(imageUrl);
            var file = await ApplicationData.Current.LocalFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteBytesAsync(file, cache);
            var result = isWallpaper
                ? await profileSettings.TrySetWallpaperImageAsync(file).AsTask()
                : await profileSettings.TrySetLockScreenImageAsync(file).AsTask();

            var resourceToolkit = Locator.Current.GetService<IResourceToolkit>();
            if (result)
            {
                Splat.Locator.Current.GetService<AppViewModel>().ShowTip(resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.SetSuccess), Models.Enums.App.InfoType.Success);
            }
            else
            {
                Splat.Locator.Current.GetService<AppViewModel>().ShowTip(resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.SetFailed), Models.Enums.App.InfoType.Error);
            }

            await Task.Delay(1000);
            await file.DeleteAsync();
        }

        private void OnShareButtonClick(object sender, RoutedEventArgs e)
        {
            var dataTransferManager = DataTransferManager.GetForCurrentView();
            dataTransferManager.DataRequested += OnDataRequested;
            DataTransferManager.ShowShareUI();
        }

        private void OnDataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            var data = args.Request.Data;
            var image = Images[_currentIndex];
            data.Properties.Title = "分享自哔哩的图片";
            data.SetWebLink(image.GetSourceUri());
            data.SetBitmap(RandomAccessStreamReference.CreateFromUri(image.GetSourceUri()));
        }

        private void OnCloseButtonClick(object sender, RoutedEventArgs e)
        {
            // 关闭控件.
            _images.Clear();
            Images.Clear();
            _currentIndex = 0;
            Image.Source = null;
            Container.Visibility = Visibility.Collapsed;
            Splat.Locator.Current.GetService<AppViewModel>().ShowImages(null, -1);
        }

        private void ShowControls()
        {
            TopContainer.Visibility = Visibility.Visible;
            ImageListContainer.Visibility = Images.Count > 1 ? Visibility.Visible : Visibility.Collapsed;
            _isControlShown = true;
        }

        private void HideControls()
        {
            TopContainer.Visibility = ImageListContainer.Visibility = Visibility.Collapsed;
            _isControlShown = false;
        }
    }
}
