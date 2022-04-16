// Copyright (c) Richasy. All rights reserved.

using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Richasy.Bili.App.Controls
{
    /// <summary>
    /// 用户头像.
    /// </summary>
    public sealed partial class UserAvatar : UserControl
    {
        /// <summary>
        /// <see cref="UserName"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty UserNameProperty =
            DependencyProperty.Register(nameof(UserName), typeof(string), typeof(UserAvatar), new PropertyMetadata(string.Empty));

        /// <summary>
        /// <see cref="Avatar"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty AvatarProperty =
            DependencyProperty.Register(nameof(Avatar), typeof(string), typeof(UserAvatar), new PropertyMetadata(string.Empty));

        /// <summary>
        /// <see cref="DecodeSize"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty DecodeSizeProperty =
            DependencyProperty.Register(nameof(DecodeSize), typeof(int), typeof(UserAvatar), new PropertyMetadata(50));

        /// <summary>
        /// Initializes a new instance of the <see cref="UserAvatar"/> class.
        /// </summary>
        public UserAvatar()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 点击时触发.
        /// </summary>
        public event EventHandler Click;

        /// <summary>
        /// 用户名.
        /// </summary>
        public string UserName
        {
            get { return (string)GetValue(UserNameProperty); }
            set { SetValue(UserNameProperty, value); }
        }

        /// <summary>
        /// 头像.
        /// </summary>
        public string Avatar
        {
            get { return (string)GetValue(AvatarProperty); }
            set { SetValue(AvatarProperty, value); }
        }

        /// <summary>
        /// 解析图像的大小.
        /// </summary>
        public int DecodeSize
        {
            get { return (int)GetValue(DecodeSizeProperty); }
            set { SetValue(DecodeSizeProperty, value); }
        }

        private void OnClick(object sender, RoutedEventArgs e)
            => Click?.Invoke(this, EventArgs.Empty);
    }
}
