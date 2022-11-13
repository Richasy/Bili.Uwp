// Copyright (c) Richasy. All rights reserved.

using System;
using System.Windows.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Bili.Desktop.App.Controls
{
    /// <summary>
    /// 播放器提示，通常从右侧弹出.
    /// </summary>
    public sealed class PlayerTip : Control
    {
        /// <summary>
        /// <see cref="IsOpen"/> 的依赖属性.
        /// </summary>
        public static readonly DependencyProperty IsOpenProperty =
            DependencyProperty.Register(nameof(IsOpen), typeof(bool), typeof(PlayerTip), new PropertyMetadata(default));

        /// <summary>
        /// <see cref="Message"/> 的依赖属性.
        /// </summary>
        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register(nameof(Message), typeof(string), typeof(PlayerTip), new PropertyMetadata(string.Empty));

        /// <summary>
        /// <see cref="Title"/> 的依赖属性.
        /// </summary>
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(nameof(Title), typeof(string), typeof(PlayerTip), new PropertyMetadata(string.Empty));

        /// <summary>
        /// <see cref="ActionContent"/> 的依赖属性.
        /// </summary>
        public static readonly DependencyProperty ActionContentProperty =
            DependencyProperty.Register(nameof(ActionContent), typeof(string), typeof(PlayerTip), new PropertyMetadata(string.Empty));

        /// <summary>
        /// <see cref="AdditionalTitle"/> 的依赖属性.
        /// </summary>
        public static readonly DependencyProperty AdditionalTitleProperty =
            DependencyProperty.Register(nameof(AdditionalTitle), typeof(string), typeof(PlayerTip), new PropertyMetadata(string.Empty));

        /// <summary>
        /// <see cref="Command"/> 的依赖属性.
        /// </summary>
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register(nameof(Command), typeof(ICommand), typeof(PlayerTip), new PropertyMetadata(default));

        /// <summary>
        /// <see cref="CloseCommand"/> 的依赖属性.
        /// </summary>
        public static readonly DependencyProperty CloseCommandProperty =
            DependencyProperty.Register(nameof(CloseCommand), typeof(ICommand), typeof(PlayerTip), new PropertyMetadata(default));

        private const string ActionButtonName = "ActionButton";
        private const string CloseButtonName = "CloseButton";

        private Button _actionButton;
        private Button _closeButton;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerTip"/> class.
        /// </summary>
        public PlayerTip()
        {
            this.DefaultStyleKey = typeof(PlayerTip);
        }

        /// <summary>
        /// 动作按钮被点击时触发.
        /// </summary>
        public event EventHandler ActionClick;

        /// <summary>
        /// 标题.
        /// </summary>
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        /// <summary>
        /// 消息内容.
        /// </summary>
        public string Message
        {
            get { return (string)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }

        /// <summary>
        /// 是否显示.
        /// </summary>
        public bool IsOpen
        {
            get { return (bool)GetValue(IsOpenProperty); }
            set { SetValue(IsOpenProperty, value); }
        }

        /// <summary>
        /// 动作按钮的内容.
        /// </summary>
        public string ActionContent
        {
            get { return (string)GetValue(ActionContentProperty); }
            set { SetValue(ActionContentProperty, value); }
        }

        /// <summary>
        /// 附加标题.
        /// </summary>
        public string AdditionalTitle
        {
            get { return (string)GetValue(AdditionalTitleProperty); }
            set { SetValue(AdditionalTitleProperty, value); }
        }

        /// <summary>
        /// 动作命令.
        /// </summary>
        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        /// <summary>
        /// 关闭命令.
        /// </summary>
        public ICommand CloseCommand
        {
            get { return (ICommand)GetValue(CloseCommandProperty); }
            set { SetValue(CloseCommandProperty, value); }
        }

        /// <inheritdoc/>
        protected override void OnApplyTemplate()
        {
            _actionButton = GetTemplateChild(ActionButtonName) as Button;
            _closeButton = GetTemplateChild(CloseButtonName) as Button;

            _actionButton.Click += OnActionButtonClick;
            _closeButton.Click += OnCloseButtonClick;
        }

        private void OnCloseButtonClick(object sender, RoutedEventArgs e)
            => IsOpen = false;

        private void OnActionButtonClick(object sender, RoutedEventArgs e)
            => ActionClick?.Invoke(this, EventArgs.Empty);
    }
}
