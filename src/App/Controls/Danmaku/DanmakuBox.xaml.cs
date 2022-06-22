﻿// Copyright (c) Richasy. All rights reserved.

using System;
using Bili.ViewModels.Uwp;
using Bili.ViewModels.Uwp.Common;
using Bili.ViewModels.Uwp.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Bili.App.Controls.Danmaku
{
    /// <summary>
    /// 弹幕输入控件.
    /// </summary>
    public sealed partial class DanmakuBox : UserControl
    {
        /// <summary>
        /// <see cref="ViewModel"/> 的依赖属性.
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel), typeof(DanmakuModuleViewModel), typeof(DanmakuBox), new PropertyMetadata(default));

        /// <summary>
        /// Initializes a new instance of the <see cref="DanmakuBox"/> class.
        /// </summary>
        public DanmakuBox() => InitializeComponent();

        /// <summary>
        /// 视图模型.
        /// </summary>
        public DanmakuModuleViewModel ViewModel
        {
            get { return (DanmakuModuleViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        private void OnDanmakuInputBoxSubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (!string.IsNullOrEmpty(args.QueryText))
            {
                sender.IsEnabled = false;
                ViewModel.SendDanmakuCommand.Execute(args.QueryText).Subscribe(async isSuccess =>
                {
                    await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                    {
                        sender.IsEnabled = true;
                        if (isSuccess)
                        {
                            sender.Text = string.Empty;
                        }
                        else
                        {
                            sender.Focus(Windows.UI.Xaml.FocusState.Programmatic);
                        }
                    });
                });
            }
        }
    }
}
