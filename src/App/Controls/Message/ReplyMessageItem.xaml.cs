// Copyright (c) Richasy. All rights reserved.

using System;
using Bili.Locator.Uwp;
using Bili.Models.Enums.Bili;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Uwp;
using Humanizer;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Bili.App.Controls
{
    /// <summary>
    /// 回复我的消息条目.
    /// </summary>
    public sealed partial class ReplyMessageItem : UserControl, IRepeaterItem
    {
        /// <summary>
        /// <see cref="Data"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register(nameof(Data), typeof(Models.BiliBili.ReplyMessageItem), typeof(ReplyMessageItem), new PropertyMetadata(null, new PropertyChangedCallback(OnDataChanged)));

        /// <summary>
        /// Initializes a new instance of the <see cref="ReplyMessageItem"/> class.
        /// </summary>
        public ReplyMessageItem()
        {
            InitializeComponent();
        }

        /// <summary>
        /// @我的消息数据.
        /// </summary>
        public Models.BiliBili.ReplyMessageItem Data
        {
            get { return (Models.BiliBili.ReplyMessageItem)GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }

        /// <inheritdoc/>
        public Size GetHolderSize()
        {
            return new Size(300, 200);
        }

        private static void OnDataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is Models.BiliBili.ReplyMessageItem data)
            {
                var instance = d as ReplyMessageItem;
                var resourceToolkit = ServiceLocator.Instance.GetService<IResourceToolkit>();

                instance.UserAvatar.Avatar = data.User.Avatar;
                instance.UserAvatar.UserName = data.User.UserName;
                instance.UserNameBlock.Text = data.User.UserName;
                instance.DetailBlock.Text = data.Item.SourceContent;
                instance.MultipleBlock.Visibility = data.IsMultiple == 1 ? Visibility.Visible : Visibility.Collapsed;
                instance.TypeBlock.Text = string.Format(
                    resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.ReplyMessageTypeDescription),
                    data.Item.Business,
                    data.Counts);
                var dateTime = DateTimeOffset.FromUnixTimeSeconds(data.ReplyTime).ToLocalTime();
                instance.TimeBlock.Text = dateTime.Humanize();
                ToolTipService.SetToolTip(instance.TimeBlock, dateTime.ToString("yyyy/MM/dd HH:mm"));
                instance.TitleBlock.Text = string.IsNullOrEmpty(data.Item.Title) ? data.Item.Description : data.Item.Title;
            }
        }

        private async void OnMessageItemClickAsync(object sender, RoutedEventArgs e)
        {
            var type = ReplyType.None;
            var isParseFaield = false;
            try
            {
                type = (ReplyType)Convert.ToInt32(Data.Item.BusinessId);
            }
            catch (Exception)
            {
                isParseFaield = true;
            }

            if (isParseFaield || type == ReplyType.None)
            {
                var resourceToolkit = ServiceLocator.Instance.GetService<IResourceToolkit>();
                AppViewModel.Instance.ShowTip(resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.NotSupportReplyType), Models.Enums.App.InfoType.Warning);
                return;
            }

            ReplyModuleViewModel.Instance.SetInformation(Data.Item.SubjectId, type, Bilibili.Main.Community.Reply.V1.Mode.MainListTime);
            await ReplyDetailView.Instance.ShowAsync(ReplyModuleViewModel.Instance);
        }
    }
}
