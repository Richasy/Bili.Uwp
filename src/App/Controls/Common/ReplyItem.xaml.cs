﻿// Copyright (c) Richasy. All rights reserved.

using System;
using Bilibili.Main.Community.Reply.V1;
using Richasy.Bili.Locator.Uwp;
using Richasy.Bili.Toolkit.Interfaces;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace Richasy.Bili.App.Controls
{
    /// <summary>
    /// 评论条目.
    /// </summary>
    public sealed partial class ReplyItem : UserControl
    {
        /// <summary>
        /// <see cref="Data"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register(nameof(Data), typeof(ReplyInfo), typeof(ReplyItem), new PropertyMetadata(null, new PropertyChangedCallback(OnDataChanged)));

        /// <summary>
        /// Initializes a new instance of the <see cref="ReplyItem"/> class.
        /// </summary>
        public ReplyItem()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 数据源.
        /// </summary>
        public ReplyInfo Data
        {
            get { return (ReplyInfo)GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }

        private static void OnDataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is ReplyInfo data)
            {
                var instance = d as ReplyItem;
                instance.UserAvatar.UserName = instance.UserNameBlock.Text = data.Member.Name;
                instance.UserAvatar.Avatar = data.Member.Face;
                instance.LevelImage.Source = new BitmapImage(new Uri($"ms-appx:///Assets/Level/level_{data.Member.Level}.png"));
                instance.ReplyContentBlock.Text = data.Content.Message;
                var time = DateTimeOffset.FromUnixTimeSeconds(data.Ctime);
                instance.PublishTimeBlock.Text = time.ToString("HH:mm");
                ToolTipService.SetToolTip(instance.PublishTimeBlock, time.ToString("yyyy/MM/dd HH:mm:ss"));
                instance.LikeButton.IsChecked = data.ReplyControl.Action == 1;
                instance.LikeCountBlock.Text = ServiceLocator.Instance.GetService<INumberToolkit>().GetCountText(data.Like);
                instance.MoreButton.Visibility = data.Count > 0 ? Visibility.Visible : Visibility.Collapsed;
                instance.MoreBlock.Text = string.Format(ServiceLocator.Instance.GetService<IResourceToolkit>().GetLocaleString(Models.Enums.LanguageNames.MoreReplyDisplay), data.Count);
            }
        }
    }
}
