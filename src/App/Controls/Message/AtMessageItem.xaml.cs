// Copyright (c) GodLeaveMe. All rights reserved.

using System;
using Richasy.Bili.Locator.Uwp;
using Richasy.Bili.Toolkit.Interfaces;
using Windows.Foundation;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Richasy.Bili.App.Controls
{
    /// <summary>
    /// @我的条目.
    /// </summary>
    public sealed partial class AtMessageItem : UserControl, IRepeaterItem
    {
        /// <summary>
        /// <see cref="Data"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register(nameof(Data), typeof(Models.BiliBili.AtMessageItem), typeof(AtMessageItem), new PropertyMetadata(null, new PropertyChangedCallback(OnDataChanged)));

        /// <summary>
        /// Initializes a new instance of the <see cref="AtMessageItem"/> class.
        /// </summary>
        public AtMessageItem()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// @我的消息数据.
        /// </summary>
        public Models.BiliBili.AtMessageItem Data
        {
            get { return (Models.BiliBili.AtMessageItem)GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }

        /// <inheritdoc/>
        public Size GetHolderSize()
        {
            return new Size(300, 200);
        }

        private static void OnDataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is Models.BiliBili.AtMessageItem data)
            {
                var instance = d as AtMessageItem;
                var resourceToolkit = ServiceLocator.Instance.GetService<IResourceToolkit>();

                instance.UserAvatar.Avatar = data.User.Avatar;
                instance.UserAvatar.UserName = data.User.UserName;
                instance.UserNameBlock.Text = data.User.UserName;
                instance.DetailBlock.Text = data.Item.SourceContent;
                instance.TypeBlock.Text = string.Format(resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.AtMessageTypeDescription), data.Item.Business);
                var dateTime = DateTimeOffset.FromUnixTimeSeconds(data.AtTime).ToLocalTime();
                instance.TimeBlock.Text = dateTime.ToString("HH:mm");
                ToolTipService.SetToolTip(instance.TimeBlock, dateTime.ToString("yyyy/MM/dd HH:mm"));
                instance.TitleBlock.Text = string.IsNullOrEmpty(data.Item.Title) ? resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.NoSpecificData) : data.Item.Title;
            }
        }

        private async void OnMessageItemClickAsync(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri(Data.Item.Uri));
        }
    }
}
