// Copyright (c) Richasy. All rights reserved.

using System;
using Bili.ViewModels.Interfaces;
using Bili.ViewModels.Uwp;
using Splat;
using Windows.Foundation;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace Bili.App.Controls
{
    /// <summary>
    /// 视频条目.
    /// </summary>
    public sealed partial class VideoCard : Control, IRepeaterItem, IDynamicLayoutItem
    {
        private const string FlyoutName = "VideoFlyout";
        private const string AddViewLaterButtonName = "AddToViewLaterItem";
        private const string OpenBroswerButtonName = "OpenInBroswerItem";
        private const string VerticalAvatarName = "VerticalAvatar";
        private const string CoverContainerName = "CoverContainer";
        private const string RootCardName = "RootCard";

        private FlyoutBase _flyout;
        private AppBarButton _addViewLaterButton;
        private AppBarButton _openBroswerButton;
        private UserAvatar _verticalAvatar;
        private Grid _coverContainer;
        private CardPanel _rootCard;

        /// <summary>
        /// Initializes a new instance of the <see cref="VideoCard"/> class.
        /// </summary>
        public VideoCard()
        {
            DefaultStyleKey = typeof(VideoCard);
            Loaded += OnLoaded;
            AppViewModel = AppViewModel.Instance;
        }

        /// <inheritdoc/>
        public Size GetHolderSize()
        {
            return Orientation == Orientation.Horizontal
                ? new Size(double.PositiveInfinity, 180)
                : new Size(210, 248);
        }

        /// <inheritdoc/>
        protected override void OnApplyTemplate()
        {
            _flyout = GetTemplateChild(FlyoutName) as FlyoutBase;
            _addViewLaterButton = GetTemplateChild(AddViewLaterButtonName) as AppBarButton;
            _openBroswerButton = GetTemplateChild(OpenBroswerButtonName) as AppBarButton;
            _verticalAvatar = GetTemplateChild(VerticalAvatarName) as UserAvatar;
            _coverContainer = GetTemplateChild(CoverContainerName) as Grid;
            _rootCard = GetTemplateChild(RootCardName) as CardPanel;

            _flyout.Opening += OnFlyoutOpening;
            _addViewLaterButton.Click += OnAddToViewLaterItemClickAsync;
            _openBroswerButton.Click += OnOpenInBroswerItemClickAsync;
            _verticalAvatar.Click += OnAvatarClickAsync;
            _rootCard.Click += OnContainerClickAsync;

            CheckOrientation();
        }

        private static void OnOrientationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as VideoCard;
            instance.CheckOrientation();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            CheckOrientation();
            if (ContextFlyout != null)
            {
                _rootCard.ContextFlyout = null;
            }
        }

        private void CheckOrientation()
        {
            if (_coverContainer == null)
            {
                return;
            }

            switch (Orientation)
            {
                case Orientation.Vertical:
                    VisualStateManager.GoToState(this, "VerticalState", false);
                    _coverContainer.Width = double.NaN;
                    break;
                case Orientation.Horizontal:
                    VisualStateManager.GoToState(this, "HorizontalState", false);
                    _coverContainer.Width = HorizontalCoverWidth;
                    break;
                default:
                    break;
            }
        }

        private void OnContainerClickAsync(object sender, RoutedEventArgs e)
        {
            if (!ViewModel.IsSelected)
            {
                Splat.Locator.Current.GetService<INavigationViewModel>().NavigateToPlayView(ViewModel);
            }

            ItemClick?.Invoke(this, ViewModel);
        }

        private async void OnAddToViewLaterItemClickAsync(object sender, RoutedEventArgs e)
        {
            if (ViewModel.VideoType == Models.Enums.VideoType.Video)
            {
                await ViewLaterViewModel.Instance.AddAsync(ViewModel);
            }
        }

        private void OnFlyoutOpening(object sender, object e)
        {
            if (ViewModel.VideoType != Models.Enums.VideoType.Video)
            {
                ContextFlyout.Hide();
            }
        }

        private async void OnAvatarClickAsync(object sender, EventArgs e)
        {
            if (ViewModel.Publisher != null)
            {
                await UserView.Instance.ShowAsync(ViewModel.Publisher);
            }
        }

        private async void OnOpenInBroswerItemClickAsync(object sender, RoutedEventArgs e)
        {
            var uri = string.Empty;
            if (ViewModel.VideoType == Models.Enums.VideoType.Video)
            {
                uri = $"https://www.bilibili.com/video/av{ViewModel.VideoId}";
            }
            else if (ViewModel.VideoType == Models.Enums.VideoType.Pgc)
            {
                uri = $"https://www.bilibili.com/bangumi/play/{ViewModel.VideoId}";
            }
            else if (ViewModel.VideoType == Models.Enums.VideoType.Live)
            {
                uri = $"https://live.bilibili.com/{ViewModel.VideoId}";
            }

            await Launcher.LaunchUriAsync(new Uri(uri));
        }
    }
}
