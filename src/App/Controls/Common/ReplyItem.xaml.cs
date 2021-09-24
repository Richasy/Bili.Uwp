// Copyright (c) GodLeaveMe. All rights reserved.

using System;
using Bilibili.Main.Community.Reply.V1;
using Richasy.Bili.Locator.Uwp;
using Richasy.Bili.Toolkit.Interfaces;
using Richasy.Bili.ViewModels.Uwp;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace Richasy.Bili.App.Controls
{
    /// <summary>
    /// 评论条目.
    /// </summary>
    public sealed partial class ReplyItem : UserControl, IRepeaterItem, IDynamicLayoutItem
    {
        /// <summary>
        /// <see cref="Data"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register(nameof(Data), typeof(ReplyInfo), typeof(ReplyItem), new PropertyMetadata(null, new PropertyChangedCallback(OnDataChanged)));

        /// <summary>
        /// <see cref="DetailCountVisibility"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty DetailCountVisibilityProperty =
            DependencyProperty.Register(nameof(DetailCountVisibility), typeof(Visibility), typeof(ReplyItem), new PropertyMetadata(Visibility.Visible));

        /// <summary>
        /// Initializes a new instance of the <see cref="ReplyItem"/> class.
        /// </summary>
        public ReplyItem()
        {
            this.InitializeComponent();
            Orientation = Orientation.Horizontal;
        }

        public event EventHandler<ReplyInfo> MoreButtonClick;

        /// <summary>
        /// 条目被点击时发生.
        /// </summary>
        public event EventHandler Click;

        /// <summary>
        /// 数据源.
        /// </summary>
        public ReplyInfo Data
        {
            get { return (ReplyInfo)GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }

        /// <summary>
        /// 子评论数目显示或隐藏.
        /// </summary>
        public Visibility DetailCountVisibility
        {
            get { return (Visibility)GetValue(DetailCountVisibilityProperty); }
            set { SetValue(DetailCountVisibilityProperty, value); }
        }

        /// <summary>
        /// 布局方式（附加项）.
        /// </summary>
        public Orientation Orientation { get; set; }

        /// <inheritdoc/>
        public Size GetHolderSize()
        {
            return new Size(350, 280);
        }

        private static void OnDataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is ReplyInfo data)
            {
                var instance = d as ReplyItem;
                var isTop = data.ReplyControl.IsAdminTop || data.ReplyControl.IsUpTop;
                instance.TopContainer.Visibility = isTop ? Visibility.Visible : Visibility.Collapsed;
                instance.UserAvatar.UserName = instance.UserNameBlock.Text = data.Member.Name;
                instance.UserAvatar.Avatar = data.Member.Face;
                instance.LevelImage.Source = new BitmapImage(new Uri($"ms-appx:///Assets/Level/level_{data.Member.Level}.png"));
                instance.ReplyContentBlock.Text = data.Content.Message;
                var time = DateTimeOffset.FromUnixTimeSeconds(data.Ctime).ToLocalTime();
                instance.PublishTimeBlock.Text = time.ToString("HH:mm");
                ToolTipService.SetToolTip(instance.PublishTimeBlock, time.ToString("yyyy/MM/dd HH:mm:ss"));
                instance.LikeButton.IsChecked = data.ReplyControl.Action == 1;
                instance.LikeCountBlock.Text = ServiceLocator.Instance.GetService<INumberToolkit>().GetCountText(data.Like);
                instance.MoreButton.Visibility = data.Count > 0 ? Visibility.Visible : Visibility.Collapsed;
                instance.MoreBlock.Text = string.Format(ServiceLocator.Instance.GetService<IResourceToolkit>().GetLocaleString(Models.Enums.LanguageNames.MoreReplyDisplay), data.Count);
            }
        }

        private void OnMoreButtonClick(object sender, RoutedEventArgs e)
        {
            MoreButtonClick?.Invoke(this, Data);
        }

        private async void OnLikeButtonClickAsync(object sender, RoutedEventArgs e)
        {
            var isLike = !(Data.ReplyControl.Action == 1);
            this.Focus(FocusState.Programmatic);
            LikeButton.IsEnabled = false;
            var result = await ReplyModuleViewModel.Instance.LikeReplyAysnc(isLike, Data.Id);
            LikeButton.IsEnabled = true;
            if (result)
            {
                LikeButton.IsChecked = isLike;
                Data.ReplyControl.Action = isLike ? 1 : 0;
                Data.Like = isLike ? Data.Like + 1 : Data.Like - 1;
            }
            else
            {
                LikeButton.IsChecked = Data.ReplyControl.Action == 1;
            }

            LikeCountBlock.Text = ServiceLocator.Instance.GetService<INumberToolkit>().GetCountText(Data.Like);
        }

        private void OnCardClick(object sender, RoutedEventArgs e)
        {
            Click?.Invoke(this, EventArgs.Empty);
        }
    }
}
