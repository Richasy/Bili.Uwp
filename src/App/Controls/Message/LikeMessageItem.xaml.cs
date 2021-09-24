// Copyright (c) GodLeaveMe. All rights reserved.

using System;
using System.Linq;
using Richasy.Bili.Locator.Uwp;
using Richasy.Bili.Toolkit.Interfaces;
using Windows.Foundation;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Richasy.Bili.App.Controls
{
    /// <summary>
    /// 点赞消息条目.
    /// </summary>
    public sealed partial class LikeMessageItem : UserControl, IRepeaterItem
    {
        /// <summary>
        /// <see cref="Data"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register(nameof(Data), typeof(Models.BiliBili.LikeMessageItem), typeof(LikeMessageItem), new PropertyMetadata(null, new PropertyChangedCallback(OnDataChanged)));

        /// <summary>
        /// Initializes a new instance of the <see cref="LikeMessageItem"/> class.
        /// </summary>
        public LikeMessageItem()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 点赞消息数据.
        /// </summary>
        public Models.BiliBili.LikeMessageItem Data
        {
            get { return (Models.BiliBili.LikeMessageItem)GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }

        /// <inheritdoc/>
        public Size GetHolderSize()
        {
            return new Size(300, 200);
        }

        private static void OnDataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is Models.BiliBili.LikeMessageItem data)
            {
                var instance = d as LikeMessageItem;
                var isMany = data.Users.Count > 1;
                instance.LatestContainer.Visibility = data.IsLatest ? Visibility.Visible : Visibility.Collapsed;
                var detail = string.Empty;

                var resourceToolkit = ServiceLocator.Instance.GetService<IResourceToolkit>();

                var firstUser = data.Users.First();
                instance.UserAvatar.Avatar = firstUser.Avatar;
                instance.UserAvatar.UserName = firstUser.UserName;
                if (isMany)
                {
                    var first = data.Users.First().UserName;
                    var second = data.Users[1].UserName;

                    instance.MultipleHolder.Visibility = Visibility.Visible;
                    detail = string.Format(
                        resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.LikeMessageMultipleDescription),
                        first,
                        second,
                        data.Count,
                        data.Item.Business);
                }
                else
                {
                    instance.MultipleHolder.Visibility = Visibility.Collapsed;
                    detail = string.Format(
                        resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.LikeMessageSingleDescription),
                        data.Users.First().UserName,
                        data.Item.Business);
                }

                instance.DetailBlock.Text = detail;
                var dateTime = DateTimeOffset.FromUnixTimeSeconds(data.LikeTime).ToLocalTime();
                instance.TimeBlock.Text = dateTime.ToString("HH:mm");
                ToolTipService.SetToolTip(instance.TimeBlock, dateTime.ToString("yyyy/MM/dd HH:mm"));
                instance.TitleBlock.Text = string.IsNullOrEmpty(data.Item.Title) ? data.Item.Description : data.Item.Title;
            }
        }

        private async void OnMessageItemClickAsync(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri(Data.Item.Uri));
        }
    }
}
